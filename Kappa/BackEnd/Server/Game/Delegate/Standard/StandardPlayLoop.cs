using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.BackEnd.Server.Game.Delegate.Standard {
    internal class StandardPlayLoop : PlayLoopDelegate {
        private Session session;
        private int queue;

        public StandardPlayLoop(Session session, int queue) {
            this.session = session;
        }

        public override async Task Abandon() {
            await this.session.GameInvitationService.Leave();
            await this.session.GameService.QuitGame();
            await this.session.MatchmakingService.CancelFromQueueIfPossible(queue);
        }
    }
}
