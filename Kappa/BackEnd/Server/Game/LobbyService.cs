using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON;
using Kappa.Riot.Domain.TeambuilderDraft;
using Kappa.BackEnd.Server.Game.Model;
using Kappa.Riot.Services;
using MFroehlich.Parsing.JSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;
using static Kappa.BackEnd.Server.Game.PlayLoopService;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Lobby")]
    public class LobbyService : JSONService {
        private Session session;

        private ChatRoomService rooms;
        private PlayLoopService loop;

        private LobbyState state;

        [Async("/state")]
        public event EventHandler<LobbyState> State;

        [Async("/matchmaking")]
        public event EventHandler AdvancedToMatchmaking;

        public LobbyService(PlayLoopService loop, ChatRoomService rooms, Session session) : base("/playloop/lobby") {
            this.session = session;
            this.rooms = rooms;
            this.loop = loop;

            var messages = new MessageConsumer(session);

            messages.Consume<LobbyStatus>(OnLobbyStatus);
            messages.Consume<InvitePrivileges>(OnInvitePrivelages);
            messages.Consume<SearchingForMatchNotification>(OnSearchingForMatch);
            messages.Consume<GameNotification>(OnGameNotification);

            messages.Consume<GameDataObject>(OnGameData);
            messages.Consume<RemovedFromService>(OnRemovedFromService);
        }

        #region Standard

        private LobbyStatus lastLobbyStatus;

        private bool OnLobbyStatus(LobbyStatus status) {
            if (state == null) return true;
            lastLobbyStatus = status;
            state.IsCaptain = status.Owner.SummonerId == session.Me.SummonerId;

            if (loop.CurrentType == PlayLoopType.STANDARD) {

                state.CanMatch = true;

                if (state.Chatroom == Guid.Empty) {
                    state.Chatroom = rooms.JoinStandard(status);
                }

                var members = new List<LobbyMember>();
                foreach (var old in status.Members) {
                    var member = new LobbyMember(old);
                    if (old.SummonerId == session.Me.SummonerId)
                        state.Me = member;
                    members.Add(member);
                }
                state.Members = members;
            }

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

        #endregion

        #region Draft

        private GameData draftData;

        private bool OnGameData(GameDataObject lcds) {
            if (lcds != null)
                draftData = lcds.Content;

            switch (lcds.Content.CurrentPhase) {
            case Phase.DRAFT_PREMADE:

                if (state.Chatroom == Guid.Empty) {
                    state.Chatroom = rooms.JoinDraft(draftData.PremadeState);
                }

                state.Members = new List<LobbyMember>();
                foreach (var slot in draftData.PremadeState.Slots) {
                    var member = new LobbyMember(slot);
                    if (slot.SlotId == draftData.PremadeState.MySlot)
                        state.Me = member;
                    state.Members.Add(member);
                }

                state.CanMatch = draftData.PremadeState.ReadyToMatchmake;
                OnStateChanged();
                break;

            case Phase.MATCHMAKING:
                OnAdvancedToMatchmaking();
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

        [Endpoint("/create")]
        public async Task CreateLobby(int id) {
            if (id == 400 || id == 410) {
                var lcds = await this.session.TeambuilderDraftService.CreateDraftPremade(id);

                var data = lcds as GameDataObject;
                var gate = lcds as GatekeeperExceptionObject;
                var fail = lcds as CallFailedObject;

                if (fail != null && fail.Status.Contains("INVALID_CONTEXT")) {
                    await this.session.TeambuilderDraftService.Quit();
                    await CreateLobby(id);
                }
                else if (gate != null) {
                    var reason = gate.Content.GatekeeperRestrictions.FirstOrDefault();
                    throw new InvalidOperationException(reason.Serialize().ToJSON());
                }
                else if (data != null) {
                    var status = await this.session.GameInvitationService.CreateGroupFinderLobby(id, data.Content.PremadeState.PremadeId);
                    loop.Setup(PlayLoopType.DRAFT, id);
                    OnLobbyStatus(status);
                    OnGameData(data);
                }
                else {
                    throw new Exception(lcds.Status);
                }
            }
            else {
                var service = new GameInvitationService(session);
                var lobby = await service.CreateArrangedTeamLobby(id);

                if (lobby == null) throw new Exception("???");

                loop.Setup(PlayLoopType.STANDARD, id);
                OnLobbyStatus(lobby);
            }
        }

        [Endpoint("/matchmake")]
        public async Task StartQueue() {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                var mmp = new MatchMakerParams {
                    InvitationId = lastLobbyStatus.InvitationId,
                    QueueIds = new List<int> { loop.CurrentQueueId }
                };

                var search = await this.session.MatchmakingService.AttachToQueue(mmp);
                session.HandleMessage(search);
                OnSearchingForMatch(search);
                break;

            case PlayLoopType.DRAFT:
                var result = await this.session.TeambuilderDraftService.StartMatchmaking();
                session.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/selectRoles")]
        public async Task SelectRoles(Position one, Position two) {
            if (one == Position.FILL)
                two = Position.UNSELECTED;

            var result = await this.session.TeambuilderDraftService.SpecifyDraftPositionPreferences(one, two);
            session.HandleMessage(result);
        }

        #endregion

        internal void Reset() {
            state = new LobbyState();
            OnStateChanged();
        }

        internal async Task Join(Invitation invite) {
            if (invite.MetaData.QueueId == 400 || invite.MetaData.QueueId == 410) {
                var status = await this.session.GameInvitationService.Accept(invite.Id);
                var metadata = JSONDeserializer.Deserialize<InvitationMetaData>(JSONParser.Parse(status.GameMetaData));
                var lcds = await this.session.TeambuilderDraftService.JoinDraftPremade(metadata.GroupFinderId);

                var data = lcds as GameDataObject;
                var gate = lcds as GatekeeperExceptionObject;

                if (data != null) {
                    loop.Setup(PlayLoopType.DRAFT, invite.MetaData.QueueId);
                    OnLobbyStatus(status);
                    OnGameData(data);
                }
                else if (gate != null) {
                    var reason = gate.Content.GatekeeperRestrictions.FirstOrDefault();
                    throw new InvalidOperationException(reason.Serialize().ToJSON());
                }
                else {
                    throw new Exception(lcds.Status);
                }
            }
            else {
                var status = await this.session.GameInvitationService.Accept(invite.Id);
                loop.Setup(PlayLoopType.STANDARD, invite.MetaData.QueueId);
                OnLobbyStatus(status);
            }
        }


        private void OnStateChanged() {
            State?.Invoke(this, state);
        }

        private void OnAdvancedToMatchmaking() {
            AdvancedToMatchmaking?.Invoke(this, new EventArgs());
        }
    }
}
