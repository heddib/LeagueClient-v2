using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;
using Kappa.BackEnd.Server.Game.Model;
using Kappa.Riot.Domain;
using Kappa.Riot.Services;

namespace Kappa.BackEnd.Server.Game.Delegate.Standard {
    internal class StandardLobby : LobbyDelegate {
        private StandardPlayLoop loop;
        private Session session;

        public StandardLobby(Session session, StandardPlayLoop loop, ChatRoomService rooms) : base(session, rooms) {
            this.session = session;
            this.loop = loop;

            Messages.Consume<LobbyStatus>(OnLobbyStatus);
            Messages.Consume<InvitePrivileges>(OnInvitePrivelages);
            Messages.Consume<SearchingForMatchNotification>(OnSearchingForMatch);
            Messages.Consume<GameNotification>(OnGameNotification);
        }

        private LobbyStatus lastLobbyStatus;

        private bool OnLobbyStatus(LobbyStatus status) {
            if (state == null) return true;
            lastLobbyStatus = status;

            state.IsCaptain = status.Owner.SummonerId == session.Me.SummonerId;
            state.CanMatch = true;

            if (state.Chatroom == Guid.Empty) {
                state.Chatroom = Rooms.JoinStandard(status);
            }

            var members = new List<LobbyMember>();
            foreach (var old in status.Members) {
                var member = new LobbyMember(old);
                if (old.SummonerId == session.Me.SummonerId)
                    state.Me = member;
                members.Add(member);
            }
            state.Members = members;

            state.Invitees = (from old in status.Invitees
                              where old.InviteeState != "CREATOR"
                              select new LobbyInvitee(old)).ToList();

            OnStateChanged();
            return true;
        }

        private bool OnInvitePrivelages(InvitePrivileges privelage) {
            state.CanInvite = privelage.CanInvite;

            OnStateChanged();
            return true;
        }

        private bool OnSearchingForMatch(SearchingForMatchNotification search) {
            if (search.JoinedQueues.Count > 0) {
                OnAdvancedToMatchmaking();
            }
            else {
                if (!search.PlayerJoinFailures.Any()) Debugger.Break();
                var fail = search.PlayerJoinFailures.First();
                //TODO OnFailedQueue(fail);
                var dodger = fail as QueueDodger;
                if (dodger != null) {
                    Debug.WriteLine(dodger.ReasonFailed + ": " + dodger.DodgePenaltyRemainingTime);
                }
                else {
                    Debug.WriteLine(fail.ReasonFailed);
                }
            }
            return true;
        }

        private bool OnGameNotification(GameNotification notify) {

            return true;
        }


        //public override async Task CreateLobby(int id) {
        //    var service = new GameInvitationService(session);
        //    var lobby = await service.CreateArrangedTeamLobby(id);

        //    if (lobby == null) throw new Exception("???");
        //    OnLobbyStatus(lobby);
        //}

        public override async Task StartQueue() {
            var mmp = new MatchMakerParams {
                InvitationId = lastLobbyStatus.InvitationId,
                QueueIds = new List<int> { loop.QueueId },
                Team = lastLobbyStatus.Members.Select(m => m.SummonerId).ToList()
            };

            var search = await this.session.MatchmakingService.AttachToQueue(mmp);
            session.HandleMessage(search);
            OnSearchingForMatch(search);
        }
    }
}
