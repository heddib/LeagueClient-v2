using Kappa.Riot.Domain;
using Kappa.BackEnd.Server.Game.Model;
using System;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;
using RtmpSharp.Messaging;
using static Kappa.BackEnd.Server.Game.PlayLoopService;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Custom Games")]
    public class CustomService : JSONService {
        private ChatRoomService rooms;
        private PlayLoopService loop;
        private Session session;

        private CustomState state;

        [Async("/state")]
        public event EventHandler<CustomState> State;

        [Async("/champselect")]
        public event EventHandler AdvancedToChampSelect;

        public CustomService(PlayLoopService loop, ChatRoomService rooms, Session session) : base("/playloop/custom") {
            this.session = session;
            this.rooms = rooms;
            this.loop = loop;

            var messages = new MessageConsumer(session);

            messages.Consume<GameDTO>(OnGameDTO);
        }

        #region LCDS

        private GameDTO lastGameDto;
        private bool OnGameDTO(GameDTO data) {
            lastGameDto = data;
            switch (data.GameState) {
            case GameState.TEAM_SELECT:
                state.BlueTeam.Clear();
                state.RedTeam.Clear();
                foreach (var raw in data.TeamOne) {
                    var member = new LobbyMember(raw);
                    if (raw.SummonerInternalName == session.Me.InternalName)
                        state.Me = member;
                    if (raw.SummonerInternalName == data.OwnerSummary.SummonerInternalName)
                        state.Owner = member;
                    state.BlueTeam.Add(member);
                }
                foreach (var raw in data.TeamTwo) {
                    var member = new LobbyMember(raw);
                    if (raw.SummonerInternalName == session.Me.InternalName)
                        state.Me = member;
                    if (raw.SummonerInternalName == data.OwnerSummary.SummonerInternalName)
                        state.Owner = member;
                    state.RedTeam.Add(member);
                }

                if (state.Chatroom == Guid.Empty) {
                    state.Chatroom = rooms.JoinCustom(data);
                }

                OnStateChanged();
                break;

            case GameState.PRE_CHAMP_SELECT:
            case GameState.CHAMP_SELECT:
            case GameState.POST_CHAMP_SELECT:
                OnAdvancedToChampSelect();
                break;
            }
            return true;
        }

        #endregion

        #region Endpoints

        [Endpoint("/create")]
        public async Task CreateLobby() {
            try {
                var game = await this.session.GameService.CreatePracticeGame(new PracticeGameConfig {
                    GameMap = GameMaps.SummonersRift,
                    GameMode = "CLASSIC",
                    AllowSpectators = "NONE",
                    GameTypeConfig = 2,
                    MaxNumPlayers = 10,
                    GamePassword = "secret",
                    GameName = "master mor's game",
                });
                if (game?.Name == null) {
                    throw new Exception("Invalid name");
                }

                loop.Setup(PlayLoopType.STANDARD, 0);
                OnGameDTO(game);
            } catch (InvocationException x) when (x.RootCause is PlayerAlreadyInGameException) {
                await loop.Quit();
                await CreateLobby();
            }
        }

        [Endpoint("/switchTeams")]
        public async Task SwitchTeams() {
            await this.session.GameService.SwitchTeams(lastGameDto.Id);
        }

        [Endpoint("/start")]
        public async Task Start() {
            await this.session.GameService.StartChampionSelection(lastGameDto.Id, lastGameDto.OptimisticLock);
        }

        #endregion

        internal void Reset() {
            if (state != null && state.Chatroom != Guid.Empty)
                rooms.LeaveRoom(state.Chatroom);

            state = new CustomState();
            OnStateChanged();
        }

        internal int CurrentConfig => lastGameDto?.GameTypeConfigId ?? -1;

        internal async Task Join(Invitation invite) {
            var peek = session.Peek<GameDTO>();
            await this.session.GameInvitationService.Accept(invite.Id);
            loop.Setup(PlayLoopType.STANDARD, invite.MetaData.QueueId);
            OnGameDTO(await peek);
        }

        private void OnStateChanged() {
            State?.Invoke(this, state);
        }

        private void OnAdvancedToChampSelect() {
            Reset();
            AdvancedToChampSelect?.Invoke(this, new EventArgs());
        }
    }
}
