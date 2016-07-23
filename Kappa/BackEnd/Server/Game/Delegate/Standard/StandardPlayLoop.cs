using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;

namespace Kappa.BackEnd.Server.Game.Delegate.Standard {
    internal class StandardPlayLoop : PlayLoopDelegate {
        public StandardLobby Lobby { get; }
        public int QueueId { get; }

        private Session session;

        public StandardPlayLoop(Session session, ChatRoomService rooms, int queue) {
            this.session = session;
            this.QueueId = queue;

            Lobby = new StandardLobby(session, this, rooms);
        }

        public override async Task Abandon() {
            await this.session.GameInvitationService.Leave();
            await this.session.GameService.QuitGame();
            await this.session.MatchmakingService.CancelFromQueueIfPossible(QueueId);
        }
    }
}
