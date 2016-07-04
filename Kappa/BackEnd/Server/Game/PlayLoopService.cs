using Kappa.BackEnd.Server.Game.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Play Loop")]
    public class PlayLoopService : JSONService {
        internal enum PlayLoopType {
            NONE,
            DRAFT,
            STANDARD
        }

        private Task<AvailableQueue[]> availableQueues;

        private Session session;

        internal PlayLoopType CurrentType { get; private set; }
        internal int CurrentQueueId { get; private set; }

        private LobbyService lobby;
        private CustomService custom;
        private MatchmakingService matchmaking;
        private ChampSelectService champselect;
        private ActiveGameService active;
        private PostGameService post;

        public PlayLoopService(Session session, ChatRoomService rooms) : base("/playloop") {
            this.session = session;
            session.Authed += Kappa_Authed;

            lobby = new LobbyService(this, rooms, session);
            custom = new CustomService(this, rooms, session);
            matchmaking = new MatchmakingService(this, session);
            champselect = new ChampSelectService(this, rooms, session);
            active = new ActiveGameService(this, session);
            post = new PostGameService(rooms, session);

            Reset();
        }

        private void Kappa_Authed(object sender, EventArgs e) {
            availableQueues = Task.Run(async () => {
                var raw = await this.session.MatchmakingService.GetAvailableQueues();
                return raw.Select(r => new AvailableQueue(r)).ToArray();
            });
        }

        [Endpoint("/current")]
        public async Task<CurrentPlayLoopState> GetCurrentQueue() {
            var queue = (await availableQueues).SingleOrDefault(q => q.Id == CurrentQueueId);
            var state = new CurrentPlayLoopState(CurrentType != PlayLoopType.NONE, CurrentQueueId, queue?.Config ?? 0);
            if (state.InPlayLoop && state.QueueId == 0) {
                state.QueueConfigId = custom.CurrentConfig;
            }
            return state;
        }

        [Endpoint("/quit")]
        public async Task Quit() {
            await Abandon();
        }

        [Endpoint("/abandon")]
        public async Task Abandon() {
            switch (CurrentType) {
            case PlayLoopType.STANDARD:
                await this.session.GameInvitationService.Leave();
                await this.session.GameService.QuitGame();
                await this.session.MatchmakingService.CancelFromQueueIfPossible(CurrentQueueId);
                break;
            case PlayLoopType.DRAFT:
                await this.session.GameInvitationService.Leave();
                await this.session.TeambuilderDraftService.Quit();
                break;
            }
            Reset();
        }

        [Endpoint("/listqueues")]
        public async Task<AvailableQueue[]> GetAvailableQueues() {
            return await availableQueues;
        }

        public Task JoinLobby(Invitation invite) {
            if (invite.MetaData.GameType == "PRACTICE_GAME")
                return custom.Join(invite);
            return lobby.Join(invite);
        }

        internal void Setup(PlayLoopType type, int queue) {
            CurrentType = type;
            CurrentQueueId = queue;
        }

        internal void Reset() {
            CurrentType = PlayLoopType.NONE;
            CurrentQueueId = -1;

            lobby.Reset();
            matchmaking.Reset();
            champselect.Reset();
            custom.Reset();
            active.Reset();
        }
    }
}
