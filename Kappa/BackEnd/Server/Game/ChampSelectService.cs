using Kappa.Riot.Domain;
using Kappa.Riot.Domain.TeambuilderDraft;
using Kappa.BackEnd.Server.Game.Model;
using Kappa.Riot.Services.Lcds;
using MFroehlich.League.Assets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;
using static Kappa.BackEnd.Server.Game.PlayLoopService;

namespace Kappa.BackEnd.Server.Game {
    [Docs("group", "Champ Select")]
    public class ChampSelectService : JSONService {
        private Session session;
        private MessageConsumer messages;

        private Model.ChampSelectState state;

        private ChatRoomService rooms;
        private PlayLoopService loop;

        [Async("/state")]
        public event EventHandler<Model.ChampSelectState> State;

        [Async("/start")]
        public event EventHandler GameStarted;

        [Async("/lobby")]
        public event EventHandler AdvancedToLobby;

        [Async("/custom")]
        public event EventHandler AdvancedToCustom;

        [Async("/matchmaking")]
        public event EventHandler AdvancedToMatchmaking;

        public ChampSelectService(PlayLoopService loop, ChatRoomService rooms, Session session) : base("/playloop/champselect") {
            this.session = session;
            this.rooms = rooms;
            this.loop = loop;

            messages = new MessageConsumer(session);

            messages.Consume<GameDTO>(OnGameDTO);
            messages.Consume<TradeContractDTO>(OnTradeContract);

            messages.Consume<GameDataObject>(OnGameData);
            messages.Consume<RemovedFromService>(OnRemovedFromService);
        }

        #region Standard

        private Dictionary<object, GameParticipant> standardMembers;

        private DateTime timerUpdated;
        private Dictionary<string, PlayerChampionSelectionDTO> selections;
        private Dictionary<string, TradeContractDTO> trades = new Dictionary<string, TradeContractDTO>();
        private List<string> potentialTraders;
        private GameDTO lastGameDto;

        private bool OnGameDTO(GameDTO data) {
            var changed = lastGameDto?.GameState != data.GameState;
            lastGameDto = data;

            var config = session.Me.GameTypeConfigs.Single(c => c.Id == data.GameTypeConfigId);

            switch (data.GameState) {
            case GameState.TEAM_SELECT:
                OnAdvancedToCustom();
                break;

            case GameState.TERMINATED:
                OnAdvancedToMatchmaking();
                break;

            case GameState.START_REQUESTED:
                OnGameStarted();
                break;

            case GameState.PRE_CHAMP_SELECT:
                if (changed) {
                    state.Remaining = config.BanTimerDuration * 1000 + 1000;
                    timerUpdated = DateTime.Now;
                }
                state.Phase = Model.ChampSelectPhase.BANNING;
                #region BANNING
                if (changed) {
                    this.session.GameService.GetChampionsForBan().ContinueWith(t => {
                        var available = from c in t.Result
                                        where c.EnemyOwned
                                        select c.ChampionId;
                        state.Inventory.BannableChampions = available.ToList();
                        OnStateChanged();
                    });
                }

                var alliedBans = from b in data.BannedChampions
                                 where RiotUtils.TeamIdToIsBlue(b.TeamId) == state.IsBlue
                                 select b.ChampionId;
                var enemyBans = from b in data.BannedChampions
                                where RiotUtils.TeamIdToIsBlue(b.TeamId) != state.IsBlue
                                select b.ChampionId;

                state.AlliedBans = alliedBans.ToList();
                state.EnemyBans = enemyBans.ToList();
                #endregion
                goto MAIN_CHAMP_SELECT;

            case GameState.POST_CHAMP_SELECT:
                if (changed) {
                    state.Remaining = config.PostPickTimerDuration * 1000 + 1000;
                    timerUpdated = DateTime.Now;
                }
                state.Phase = Model.ChampSelectPhase.FINALIZING;
                #region FINALIZING
                this.session.ChampionTradeService.GetPotentialTraders().ContinueWith(t => {
                    potentialTraders = t.Result.PotentialTraders;
                    UpdateGameMembers();
                });
                #endregion
                goto MAIN_CHAMP_SELECT;

            case GameState.CHAMP_SELECT:
                if (changed) {
                    state.Remaining = config.MainPickTimerDuration * 1000 + 1000;
                    timerUpdated = DateTime.Now;
                }
                state.Phase = Model.ChampSelectPhase.PICKING;

            MAIN_CHAMP_SELECT:
                if (state.Chatroom == Guid.Empty) {
                    state.Chatroom = rooms.JoinChampSelect(data);
                }

                if (changed) {
                    this.session.GameService.SetClientReceivedGameMessage(data.Id, "CHAMP_SELECT_CLIENT");

                    state.Inventory = new Model.Inventory();
                    state.IsBlue = data.TeamOne.Any(p => p.SummonerInternalName == session.Me.InternalName);

                    var ids = from spell in DataDragon.SpellData.Value.data.Values
                              where spell.modes.Contains(data.GameMode)
                              select spell.key;
                    state.Inventory.AvailableSpells = ids.ToList();

                    this.session.InventoryService.GetAvailableChampions().ContinueWith(t => {
                        var available = from c in t.Result
                                        where c.Owned || c.FreeToPlay
                                        select c.ChampionId;
                        state.Inventory.PickableChampions = available.ToList();
                        OnStateChanged();
                    });
                }

                state.Remaining -= (long) (DateTime.Now - timerUpdated).TotalMilliseconds;
                this.session.GameService.GetCurrentTimerForGame().ContinueWith(t => {
                    if (t.Result == null) return;

                    state.Remaining = t.Result.RemainingTimeInMillis;
                    OnStateChanged();
                    timerUpdated = DateTime.Now;
                });

                state.Turn = data.PickTurn;
                selections = data.PlayerChampionSelections.ToDictionary(s => s.SummonerInternalName);

                var expired = from trade in trades.Values
                              where trade.RequesterInternalSummonerName == session.Me.InternalName
                              where trade.RequesterChampionId != selections[session.Me.InternalName].ChampionId
                              select trade.ResponderInternalSummonerName;
                foreach (var iName in expired.ToList())
                    trades.Remove(iName);

                UpdateGameMembers();

                break;
            }

            return true;
        }

        private bool OnTradeContract(TradeContractDTO trade) {
            var other = trade.RequesterInternalSummonerName;
            if (other == session.Me.InternalName) other = trade.ResponderInternalSummonerName;

            trades[other] = trade;

            UpdateGameMembers();
            return true;
        }

        private void UpdateGameMembers() {
            GameMember me = null;
            var allies = state.IsBlue ? lastGameDto.TeamOne : lastGameDto.TeamTwo;
            var enemies = state.IsBlue ? lastGameDto.TeamTwo : lastGameDto.TeamOne;
            standardMembers = new Dictionary<object, GameParticipant>();
            state.Allies = allies.Select(p => GetMember(p, ref me)).ToList();
            state.Enemies = enemies.Select(p => GetMember(p, ref me)).ToList();
            state.Me = me;
            OnStateChanged();
        }

        private GameMember GetMember(GameParticipant part, ref GameMember me) {
            GameMember member;
            var player = part as PlayerParticipant;
            if (player != null) {
                PlayerChampionSelectionDTO selection;
                TradeContractDTO trade;

                selections.TryGetValue(player.SummonerInternalName, out selection);
                trades.TryGetValue(player.SummonerInternalName, out trade);
                bool canTrade = potentialTraders?.Contains(player.SummonerInternalName) ?? false;

                member = new GameMember(player, selection, trade, canTrade, lastGameDto.PickTurn);

                var pojo = player as ARAMPlayerParticipant;
                if (pojo != null) {
                    member.Reroll = new RerollState(pojo.PointSummary);
                }

                if (player.SummonerId == session.Me.SummonerId) {
                    me = member;
                }
            } else if (part is BotParticipant) {
                member = new GameMember((BotParticipant) part, lastGameDto.PickTurn);
            } else if (part is ObfuscatedParticipant) {
                member = new GameMember((ObfuscatedParticipant) part, lastGameDto.PickTurn);
            } else {
                throw new NotImplementedException(part.GetType().FullName);
            }
            standardMembers[member.Id] = part;
            return member;
        }

        #endregion

        #region Draft

        private Dictionary<object, Cell> draftMembers;

        private GameData draftData;
        private Model.Inventory inventory;
        private bool OnGameData(GameDataObject lcds) {
            draftData = lcds.Content;

            if (draftData.AfkCheckState?.Inventory != null) {
                var src = draftData.AfkCheckState.Inventory;
                this.inventory = new Model.Inventory {
                    AvailableSpells = src.SpellIds,
                    PickableChampions = src.LastSelectedSkins.Keys.Select(int.Parse).ToList(),
                    BannableChampions = src.AllChampionIds
                };
            }

            switch (lcds.Content.CurrentPhase) {
            case Phase.DRAFT_PREMADE:
                OnAdvancedToLobby();
                break;
            case Phase.MATCHMAKING:
            case Phase.AFK_CHECK:
                OnAdvancedToMatchmaking();
                break;
            case Phase.CHAMPION_SELECT:
                if (state.Chatroom == Guid.Empty) {
                    state.Chatroom = rooms.JoinDraft(draftData.ChampSelectState);
                }

                state.Inventory = this.inventory;
                state.Remaining = draftData.ChampSelectState.CurrentRemainingMillis;
                state.Turn = draftData.ChampSelectState.CurrentActionIndex;

                state.AlliedBans = draftData.ChampSelectState.Bans.AlliedBans;
                state.EnemyBans = draftData.ChampSelectState.Bans.EnemyBans;

                state.Phase = GetPhase(draftData.ChampSelectState);

                var current = draftData.ChampSelectState.GetCurrentAction();
                var tradesByCell = draftData.ChampSelectState.Trades.ToDictionary(t => t.CellId);
                draftMembers = new Dictionary<object, Cell>();

                state.Allies = (from c in draftData.ChampSelectState.Cells.AlliedTeam
                                orderby c.CellId
                                select GetMember(c, current, tradesByCell)).ToList();

                state.Enemies = (from c in draftData.ChampSelectState.Cells.EnemyTeam
                                 orderby c.CellId
                                 select GetMember(c, current, tradesByCell)).ToList();

                OnStateChanged();
                break;
            }

            return true;
        }

        private bool OnRemovedFromService(RemovedFromService lcds) {
            Debug.WriteLine("REMOVED FROM SERVICE");
            //TODO Removed from lobby
            return true;
        }

        private GameMember GetMember(Cell cell, ChampSelectAction current, Dictionary<int, Trade> tradesByCell) {
            Trade trade;
            tradesByCell.TryGetValue(cell.CellId, out trade);
            var member = new GameMember(cell, current, trade);
            if (cell.CellId == draftData.ChampSelectState.MyCellId)
                state.Me = member;
            draftMembers[member.Id] = cell;
            return member;
        }

        private static Model.ChampSelectPhase GetPhase(Riot.Domain.TeambuilderDraft.ChampSelectState src) {
            switch (src.Subphase) {
            case Riot.Domain.TeambuilderDraft.ChampSelectPhase.PLANNING:
                return Model.ChampSelectPhase.PLANNING;
            case Riot.Domain.TeambuilderDraft.ChampSelectPhase.BAN_PICK:
                if (src.GetCurrentAction()?.Type == ChampSelectActionType.BAN)
                    return Model.ChampSelectPhase.BANNING;
                return Model.ChampSelectPhase.PICKING;
            case Riot.Domain.TeambuilderDraft.ChampSelectPhase.FINALIZATION:
                return Model.ChampSelectPhase.FINALIZING;
            }
            return Model.ChampSelectPhase.FINALIZING;
        }

        #endregion

        #region Endpoints

        [Endpoint("/selectChampion")]
        public async Task SelectChampion(int id) {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                if (!state.Me.Active) break;

                switch (lastGameDto.GameState) {
                case GameState.PRE_CHAMP_SELECT:
                    await this.session.GameService.BanChampion(id);
                    break;
                case GameState.CHAMP_SELECT:
                    await this.session.GameService.SelectChampion(id);
                    break;
                }
                break;

            case PlayLoopType.DRAFT:
                var action = draftData.ChampSelectState.GetCurrentAction();
                LcdsServiceObject result;

                if (action == null || action.ActorCellId != draftData.ChampSelectState.MyCellId) {
                    result = await this.session.TeambuilderDraftService.SignalChampionPickIntent(id);
                } else if (action.Type == ChampSelectActionType.BAN) {
                    result = await this.session.TeambuilderDraftService.SelectChampionBan(id);
                } else {
                    result = await this.session.TeambuilderDraftService.SelectChampionPick(id);
                }
                messages.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/selectSkin")]
        public async Task SelectSkin(int id) {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                await this.session.GameService.SelectChampionSkin(selections[session.Me.InternalName].ChampionId, id);
                break;

            case PlayLoopType.DRAFT:
                var result = await this.session.TeambuilderDraftService.PickSkin(id, false);
                messages.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/selectSpells")]
        public async Task SelectSpells(int one, int two) {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                await this.session.GameService.SelectSpells(one, two);
                break;
            case PlayLoopType.DRAFT:
                var result = await this.session.TeambuilderDraftService.PickSpells(one, two);
                session.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/lockIn")]
        public async Task LockIn() {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                await this.session.GameService.ChampionSelectCompleted();
                break;

            case PlayLoopType.DRAFT:
                var action = draftData.ChampSelectState.GetCurrentAction();
                if (action?.ActorCellId != draftData.ChampSelectState.MyCellId)
                    throw new InvalidOperationException("Not your turn");

                LcdsServiceObject result;
                if (action.Type == ChampSelectActionType.BAN)
                    result = await this.session.TeambuilderDraftService.LockChampionBan();
                else if (action.ChampionId == 0)
                    result = await this.session.TeambuilderDraftService.LockChampionFromPickIntent();
                else
                    result = await this.session.TeambuilderDraftService.LockChampionPick();
                messages.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/trade")]
        public async Task Trade(object id) {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                var member = state.Allies.Single(a => a.Id.Equals(id));
                var iName = standardMembers[id].SummonerInternalName;

                switch (member.TradeState) {
                case TradeState.RECEIVED:
                    trades.Remove(iName);
                    await this.session.ChampionTradeService.AcceptTrade(iName, selections[iName].ChampionId);
                    UpdateGameMembers();
                    break;
                case TradeState.CANCELLED:
                case TradeState.POSSIBLE:
                    await this.session.ChampionTradeService.AttemptTrade(iName, selections[iName].ChampionId);
                    trades[iName] = new TradeContractDTO {
                        RequesterChampionId = state.Me.Champion,
                        RequesterInternalSummonerName = session.Me.InternalName,
                        ResponderChampionId = member.Champion,
                        ResponderInternalSummonerName = iName,
                        State = "PENDING"
                    };
                    UpdateGameMembers();
                    break;
                default:
                    throw new InvalidOperationException("Attempted trade when trade state is " + member.TradeState);
                }
                break;

            case PlayLoopType.DRAFT:
                var cell = draftMembers[id];
                var trade = draftData.ChampSelectState.Trades.Single(t => t.CellId == cell.CellId);
                var result = await this.session.TeambuilderDraftService.AcceptTrade(trade.Id);
                session.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/decline")]
        public async Task DeclineTrade(object id) {
            switch (loop.CurrentType) {
            case PlayLoopType.STANDARD:
                var member = state.Allies.Single(a => a.Id.Equals(id));
                var iName = standardMembers[id].SummonerInternalName;

                switch (member.TradeState) {
                case TradeState.RECEIVED://Decline
                case TradeState.SENT://Cancel
                    trades.Remove(iName);
                    await this.session.ChampionTradeService.DeclineTrade();
                    UpdateGameMembers();
                    break;
                default:
                    throw new InvalidOperationException("Attempted decline when trade state is " + member.TradeState);
                }
                break;

            case PlayLoopType.DRAFT:
                var cell = draftMembers[id];
                var trade = draftData.ChampSelectState.Trades.Single(t => t.CellId == cell.CellId);
                var result = await this.session.TeambuilderDraftService.DeclineTrade(trade.Id);
                session.HandleMessage(result);
                break;
            }
        }

        [Endpoint("/reroll")]
        public async Task Reroll() {
            if (loop.CurrentType == PlayLoopType.STANDARD) {
                var data = await this.session.RerollService.Roll();
                //var data = new RollResult {
                //    ChampionId = 127,
                //    PointSummary = new PointSummary {
                //        PointsCostToRoll = 250,
                //        MaxRolls = 2,
                //        NumberOfRolls = 1,
                //        CurrentPoints = 250,
                //        PointsToNextRoll = 250
                //    }
                //};

                var selection = lastGameDto.PlayerChampionSelections.Single(s => s.SummonerInternalName == session.Me.InternalName);
                selection.ChampionId = data.ChampionId;

                var me = (state.IsBlue ? lastGameDto.TeamOne : lastGameDto.TeamTwo).Single(s => s.SummonerInternalName == session.Me.InternalName);
                ((ARAMPlayerParticipant) me).PointSummary = data.PointSummary;

                potentialTraders = (await this.session.ChampionTradeService.GetPotentialTraders()).PotentialTraders;
                UpdateGameMembers();
            }
        }

        #endregion

        internal void Reset() {
            if (state != null && state.Chatroom != Guid.Empty)
                rooms.LeaveRoom(state.Chatroom);

            state = new Model.ChampSelectState();
            OnStateChanged();

            timerUpdated = default(DateTime);
            potentialTraders = null;
            selections = null;
            trades.Clear();
        }

        private void OnStateChanged() {
            State?.Invoke(this, state);
        }

        private void OnGameStarted() {
            GameStarted?.Invoke(this, new EventArgs());
        }

        private void OnAdvancedToLobby() {
            AdvancedToLobby?.Invoke(this, new EventArgs());
        }

        private void OnAdvancedToCustom() {
            AdvancedToCustom?.Invoke(this, new EventArgs());
        }

        private void OnAdvancedToMatchmaking() {
            AdvancedToMatchmaking?.Invoke(this, new EventArgs());
        }
    }
}
