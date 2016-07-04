using Kappa.Riot.Domain;
using Kappa.Riot.Domain.TeambuilderDraft;
using Kappa.BackEnd.Server.Game.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Kappa.BackEnd.Server.Game.PlayLoopService;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Matchmaking")]
    public class MatchmakingService : JSONService {
        private Session session;

        private PlayLoopService loop;

        private Model.MatchmakingState state;

        [Async("/state")]
        public event EventHandler<Model.MatchmakingState> State;

        [Async("/lobby")]
        public event EventHandler AdvancedToLobby;

        [Async("/champselect")]
        public event EventHandler AdvancedToChampSelect;


        public MatchmakingService(PlayLoopService loop, Session session) : base("/playloop/matchmaking") {
            this.session = session;
            this.loop = loop;

            var messages = new MessageConsumer(session);

            messages.Consume<SearchingForMatchNotification>(OnSearchingForMatch);
            messages.Consume<GameNotification>(OnGameNotification);
            messages.Consume<GameDTO>(OnGameDTO);

            messages.Consume<GameDataObject>(OnGameData);
            messages.Consume<RemovedFromService>(OnRemovedFromService);
        }

        #region Standard

        private bool? accepted;
        private DateTime queueStart;
        private DateTime afkCheckEnd;
        private GameDTO lastGameDto;

        private bool OnSearchingForMatch(SearchingForMatchNotification search) {
            if (search.JoinedQueues.Count > 0) {
                queueStart = DateTime.Now;
                state = new Model.MatchmakingState {
                    Estimate = search.JoinedQueues.First().WaitTime,
                    Actual = state?.Actual ?? 0,
                };
                OnStateChanged();
            }

            return true;
        }

        private bool OnGameNotification(GameNotification arg) {
            OnAdvancedToLobby();

            return true;
        }

        private bool OnGameDTO(GameDTO data) {
            var changed = data.GameState != lastGameDto?.GameState;
            lastGameDto = data;

            if (state != null) state.AfkCheck = null;

            switch (data.GameState) {
            case GameState.JOINING_CHAMP_SELECT:
                if (changed) {
                    afkCheckEnd = DateTime.Now.AddSeconds(data.JoinTimerDuration);
                    state.Actual += (long) (DateTime.Now - queueStart).TotalMilliseconds;
                }

                state.AfkCheck = new AfkCheck {
                    Accepted = accepted,
                    Duration = data.JoinTimerDuration * 1000,
                    Remaining = (long) (afkCheckEnd - DateTime.Now).TotalMilliseconds
                };

                OnStateChanged();
                break;
            case GameState.TERMINATED:
                if (accepted != true) {
                    OnAdvancedToLobby();
                }
                accepted = null;
                queueStart = DateTime.Now;
                OnStateChanged();
                break;
            case GameState.PRE_CHAMP_SELECT:
            case GameState.CHAMP_SELECT:
                accepted = null;
                OnAdvancedToChampSelect();
                break;
            }

            return true;
        }

        #endregion

        #region Draft

        private GameData draftData;
        private bool OnGameData(GameDataObject lcds) {
            if (lcds != null)
                draftData = lcds.Content;

            switch (lcds.Content.CurrentPhase) {
            case Phase.DRAFT_PREMADE:
                state = new Model.MatchmakingState();
                OnStateChanged();
                OnAdvancedToLobby();
                break;
            case Phase.MATCHMAKING:
                state = new Model.MatchmakingState {
                    Actual = draftData.MatchmakingState.TimeInMatchmaking,
                    Estimate = draftData.MatchmakingState.EstimatedMatchmakingTime
                };
                OnStateChanged();
                break;
            case Phase.AFK_CHECK:
                state.AfkCheck = new AfkCheck {
                    Accepted = draftData.AfkCheckState.IsReady ? new bool?(true) : null,
                    Duration = draftData.AfkCheckState.MaxDuration,
                    Remaining = draftData.AfkCheckState.RemainingDuration
                };
                OnStateChanged();
                break;
            case Phase.CHAMPION_SELECT:
                OnAdvancedToChampSelect();
                break;
            }

            return true;
        }

        private bool OnRemovedFromService(RemovedFromService lcds) {
            Debug.WriteLine("REMOVED FROM SERVICE");
            //TODO Removed from lobby
            return true;
        }

        #endregion

        #region Endpoints

        [Endpoint("/cancel")]
        public async Task CancelQueue() {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                var success = await this.session.MatchmakingService.CancelFromQueueIfPossible(loop.CurrentQueueId);
                if (success) {
                    OnAdvancedToLobby();
                }
                break;

            case PlayLoopType.DRAFT:
                var result = await this.session.TeambuilderDraftService.LeaveMatchmaking();
                session.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/afkcheck")]
        public async Task AcceptAfkCheck(bool accept) {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                this.accepted = accept;
                await this.session.GameService.AcceptPoppedGame(accept);
                break;

            case PlayLoopType.DRAFT:
                var result = await this.session.TeambuilderDraftService.IndicateAfkReadiness(accept);
                session.HandleMessage(result);
                break;
            }
        }

        #endregion

        internal void Reset() {
            state = new Model.MatchmakingState();
            OnStateChanged();

            accepted = null;
            queueStart = afkCheckEnd = default(DateTime);
            lastGameDto = null;

            draftData = null;
        }

        private void OnStateChanged() {
            State?.Invoke(this, state);
        }

        private void OnAdvancedToLobby() {
            AdvancedToLobby?.Invoke(this, new EventArgs());
        }

        private void OnAdvancedToChampSelect() {
            AdvancedToChampSelect?.Invoke(this, new EventArgs());
        }
    }
}
