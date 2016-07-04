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
        private Process process;

        private Session session;
        private PlayLoopService loop;

        private ActiveGameState state;

        [Async("/state")]
        public event EventHandler<ActiveGameState> State;

        public ActiveGameService(PlayLoopService loop, Session session) : base("/playloop/ingame") {
            this.session = session;
            this.loop = loop;

            var messages = new MessageConsumer(session);

            messages.Consume<GameDTO>(OnGameDTO);
            messages.Consume<EndOfGameStats>(OnEndOfGameStats);
            messages.Consume<SimpleDialogMessage>(OnSimpleDialogMessage);
        }

        private bool OnSimpleDialogMessage(SimpleDialogMessage msg) {
            if (msg.TitleCode != "championMastery") return false;

            //var arg = JSONParser.ParseObject((string) msg.Params[0]);
            //var data = JSONDeserializer.Deserialize<EogChampionMasteryDTO>(arg);

            //if (data.LevelUpList.Any()) { }

            //var stats = new EndOfGameChampionMastery {
            //    Champion = data.ChampionId,
            //    Grade = data.PlayerGrade,
            //    Before = new ChampionMasteryState {
            //        Level = data.ChampionLevelUp ? data.ChampionLevel - 1 : data.ChampionLevel,
            //        PointsInLevel = data.ChampionPointsSinceLastLevelBeforeGame + data.ChampionPointsUntilNextLevelBeforeGame,
            //        PointsSinceLevel = data.ChampionPointsSinceLastLevelBeforeGame,
            //        TotalPoints = data.ChampionPointsBeforeGame
            //    },
            //    After = new ChampionMasteryState {
            //        Level = data.ChampionLevel,
            //        PointsInLevel = data.ChampionPointsUntilNextLevelAfterGame + (data.ChampionPointsGained - data.ChampionPointsUntilNextLevelBeforeGame) + 1,
            //        PointsSinceLevel = data.ChampionPointsGained - data.ChampionPointsUntilNextLevelBeforeGame + 1,
            //        TotalPoints = data.ChampionPointsBeforeGame + data.ChampionPointsGained
            //    }
            //};
            //if (!data.ChampionLevelUp) {
            //    stats.After.PointsInLevel = stats.Before.PointsInLevel;
            //    stats.After.PointsSinceLevel = data.ChampionPointsSinceLastLevelBeforeGame + data.ChampionPointsGained;
            //}
            //state.ChampionMastery = stats;
            //OnStateChanged();

            return true;
        }

        private bool OnEndOfGameStats(EndOfGameStats stats) {
            //state.Stats = stats;
            //OnStateChanged();
            return true;
        }

        private bool OnGameDTO(GameDTO game) {
            switch (game.GameState) {
            case GameState.TERMINATED:
                if (state.InGame) {
                    loop.Reset();
                    return true;
                }
                break;

            case GameState.TERMINATED_IN_ERROR:
                if (state.InGame) {
                    loop.Reset();
                    return true;
                }
                break;

            case GameState.START_REQUESTED:
                state.InGame = true;
                OnStateChanged();
                return true;
            }
            return false;
        }

        [Endpoint("/launch")]
        public async Task Launch() {
            //"[Maestro port]" "LoLPatcher.exe" "LolClient.exe" "[IP address] [Port] [Encryption key] [Game id]"
            if (process != null && !process.HasExited) return;

            var procs = Process.GetProcessesByName("League of Legends");
            if (procs.Length > 0) {
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
                state.InGame = true;
                OnStateChanged();
            }

            return inProgress;
        }

        private void OnStateChanged() {
            State?.Invoke(this, state);
        }
    }
}
