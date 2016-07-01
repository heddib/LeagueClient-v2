using Kappa.Riot.Domain;
using Kappa.Riot.Domain.TeambuilderDraft;
using MFroehlich.League.Assets;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public enum TradeState {
        INVALID,
        POSSIBLE,
        BUSY,
        SENT,
        RECEIVED,
        CANCELLED,
    }

    public class GameMember : JSONSerializable {
        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("champion")]
        public int Champion { get; set; }

        [JSONField("spell1")]
        public int Spell1 { get; set; }

        [JSONField("spell2")]
        public int Spell2 { get; set; }

        [JSONField("id")]
        public object Id { get; set; }

        [JSONField("active")]
        public bool Active { get; set; }

        [JSONField("trade")]
        public TradeState TradeState { get; set; }

        [JSONField("reroll")]
        public RerollState Reroll { get; set; }

        [JSONField("intent")]
        public bool Intent { get; set; }

        [JSONField("role")]
        public Position Role { get; set; }

        private GameMember(object id) {
            Id = id;
        }

        public GameMember(Cell cell, ChampSelectAction current, Trade trade) : this(cell.CellId) {
            Name = cell.Name;
            Role = cell.AssignedPosition;

            if (current?.ActorCellId == cell.CellId && (current.Type == ChampSelectActionType.BAN || current.ChampionId != 0))
                Champion = current.ChampionId;
            else if (cell.ChampionId != 0)
                Champion = cell.ChampionId;
            else {
                Intent = true;
                Champion = cell.ChampionPickIntent;
            }

            Spell1 = cell.Spell1Id;
            Spell2 = cell.Spell2Id;

            Active = cell.CellId == current?.ActorCellId;

            switch (trade?.State) {
            case TBDTradeState.INVALID:
                TradeState = TradeState.INVALID;
                break;
            case TBDTradeState.BUSY:
                TradeState = TradeState.BUSY;
                break;
            case TBDTradeState.AVAILABLE:
                TradeState = TradeState.POSSIBLE;
                break;
            case TBDTradeState.SENT:
                TradeState = TradeState.SENT;
                break;
            case TBDTradeState.RECEIVED:
                TradeState = TradeState.RECEIVED;
                break;
            }
        }

        public GameMember(PlayerParticipant player, PlayerChampionSelectionDTO selection, TradeContractDTO trade, bool canTrade, int pickTurn) : this(player.SummonerInternalName) {
            Name = player.SummonerName;
            Champion = selection?.ChampionId ?? 0;
            Spell1 = selection?.Spell1Id ?? 0;
            Spell2 = selection?.Spell2Id ?? 0;
            Active = player.PickTurn == pickTurn;

            switch (trade?.State) {
            case null:
                TradeState = canTrade ? TradeState.POSSIBLE : TradeState.INVALID;
                break;
            case "PENDING":
                TradeState = trade.RequesterInternalSummonerName == player.SummonerInternalName ? TradeState.RECEIVED : TradeState.SENT;
                break;
            case "CANCELED":
                TradeState = TradeState.CANCELLED;
                break;
            case "DECLINED":
                TradeState = TradeState.POSSIBLE;
                break;
            default:
                TradeState = TradeState.INVALID;
                break;
            }
        }

        public GameMember(BotParticipant bot, int pickTurn) : this(bot.SummonerInternalName) {
            Name = bot.SummonerName;
            var key = bot.SummonerInternalName.Split('_')[1];
            Champion = DataDragon.ChampData.Value.data[key].key;
            Spell1 = Spell2 = 0;
            Active = bot.PickTurn == pickTurn;
        }

        public GameMember(ObfuscatedParticipant obf, int pickTurn) : this(obf.GameUniqueId) {
            Name = obf.SummonerName;
            Active = obf.PickTurn == pickTurn;
        }
    }
}
