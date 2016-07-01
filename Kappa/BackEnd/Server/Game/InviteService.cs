using Kappa.Riot.Domain;
using Kappa.BackEnd.Server.Game.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Play Loop")]
    public class InviteService : JSONService {
        private Dictionary<string, Invitation> invites = new Dictionary<string, Invitation>();

        private PlayLoopService loop;
        private Session session;

        [Async("/invite")]
        public event EventHandler<Invitation> Invite;

        public InviteService(Session session, PlayLoopService game) : base("/invite") {
            this.session = session;
            this.loop = game;
            session.Authed += Session_Authed;
            var messages = new MessageConsumer(session);

            messages.Consume<InvitationRequest>(OnInvitationRequest);
        }

        private async void Session_Authed(object sender, EventArgs e) {
            var pending = await this.session.GameInvitationService.GetPendingInvitations();
            foreach (var invite in pending) OnInvitationRequest(invite);
        }

        private bool OnInvitationRequest(InvitationRequest arg) {
            var invite = new Invitation(arg);
            invites[invite.Id] = invite;

            Invite?.Invoke(this, invite);
            return true;
        }

        [Endpoint("/invite")]
        public async Task SendInvitation(long summonerId) {
            await session.GameInvitationService.Invite(summonerId);
        }

        [Endpoint("/accept")]
        public async Task AcceptInvite(string id, bool accept) {
            var invite = invites[id];
            invites.Remove(id);
            if (accept) {
                await loop.JoinLobby(invite);
            }
            else {
                await this.session.GameInvitationService.Decline(invite.Id);
            }
        }
    }
}
