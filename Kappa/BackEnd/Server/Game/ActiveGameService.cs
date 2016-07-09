using Kappa.Riot.Domain;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Game.Model;
using Kappa.BackEnd.Server.Patcher;
using Kappa.Riot.Services.Http;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Active Game")]
    public class ActiveGameService : JSONService {
        private Session session;
        private PlayLoopService loop;

        private ActiveGameState state;

        [Async("/state")]
        public event EventHandler<ActiveGameState> State;

        [Async("/finished")]
        public event EventHandler<bool> AdvancedOutOfPlayLoop;

        private bool inGame;

        public ActiveGameService(PlayLoopService loop, Session session) : base("/playloop/ingame") {
            this.session = session;
            this.loop = loop;

            var messages = new MessageConsumer(session);

            messages.Consume<GameDTO>(OnGameDTO);
        }

        private bool OnGameDTO(GameDTO game) {
            switch (game.GameState) {
            case GameState.TERMINATED:
                if (inGame) {
                    OnAdvancedOutOfPlayLoop(false);
                    loop.Reset();
                    return true;
                }
                break;

            case GameState.TERMINATED_IN_ERROR:
                if (inGame) {
                    OnAdvancedOutOfPlayLoop(true);
                    loop.Reset();
                    return true;
                }
                break;

            case GameState.START_REQUESTED:
                inGame = true;
                OnStateChanged();
                return true;
            }
            return false;
        }

        [Endpoint("/launch")]
        public async Task Launch() {
            //"[Maestro port]" "LoLPatcher.exe" "LolClient.exe" "[IP address] [Port] [Encryption key] [Game id]"

            var procs = Process.GetProcessesByName("League of Legends");
            if (procs.Length > 0) {
                state.Launched = true;
                OnStateChanged();
                procs[0].Exited += (s, e) => OnGameClosed();
            }
            else {
                try {
                    await DownloadPreferences();
                } catch {
                    Debug.WriteLine("Preferences sync failed");
                }
                PlatformGameLifecycleDTO game;

                do {
                    game = await Check();

                    if (game == null) return;
                } while (game.PlayerCredentials == null);

                session.Maestro.JoinGame(game.PlayerCredentials);
                state.Launched = true;
                OnStateChanged();
            }
        }

        internal void Reset() {
            state = new ActiveGameState();
            OnStateChanged();
        }

        private async void OnGameClosed() {
            state.Launched = false;
            await UploadPreferences();
            await Check();
        }

        private async Task DownloadPreferences() {
            var prefs = await this.session.PlayerPreferencesService.GetPreferences(PlayerPreferencesService.PlayerPreferencesType.GamePreferences);
            var config = Path.Combine(PatcherService.Config, "PersistedSettings.json");
            Directory.CreateDirectory(PatcherService.Config);
            File.WriteAllText(config, prefs);
        }

        private async Task UploadPreferences() {
            var config = Path.Combine(PatcherService.Config, "PersistedSettings.json");
            var prefs = File.ReadAllText(config);
            await this.session.PlayerPreferencesService.SetPreferences(PlayerPreferencesService.PlayerPreferencesType.GamePreferences, prefs);
        }

        private async Task<PlatformGameLifecycleDTO> Check() {
            var inProgress = await this.session.GameService.RetrieveInProgressGameInfo();

            if (inProgress == null) {
                loop.Reset();
            }
            else {
                inGame = true;
                OnStateChanged();
            }

            return inProgress;
        }

        private void OnStateChanged() {
            State?.Invoke(this, state);
        }

        private void OnAdvancedOutOfPlayLoop(bool error) {
            AdvancedOutOfPlayLoop?.Invoke(this, error);
        }
    }
}
