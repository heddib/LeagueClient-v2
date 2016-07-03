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

    [JSONSerializable]
    public class GameMember {
        public string Name { get; set; }

        public int Champion { get; set; }

        public int Spell1 { get; set; }
        public int Spell2 { get; set; }

        public object Id { get; set; }

        public bool Active { get; set; }

        public TradeState Trade { get; set; }
        public RerollState Reroll { get; set; }

        public bool Intent { get; set; }
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
                Trade = TradeState.INVALID;
                break;
            case TBDTradeState.BUSY:
                Trade = TradeState.BUSY;
                break;
            case TBDTradeState.AVAILABLE:
                Trade = TradeState.POSSIBLE;
                break;
            case TBDTradeState.SENT:
                Trade = TradeState.SENT;
                break;
            case TBDTradeState.RECEIVED:
                Trade = TradeState.RECEIVED;
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
                Trade = canTrade ? TradeState.POSSIBLE : TradeState.INVALID;
                break;
            case "PENDING":
                Trade = trade.RequesterInternalSummonerName == player.SummonerInternalName ? TradeState.RECEIVED : TradeState.SENT;
                break;
            case "CANCELED":
                Trade = TradeState.CANCELLED;
                break;
            case "DECLINED":
                Trade = TradeState.POSSIBLE;
                break;
            default:
                Trade = TradeState.INVALID;
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
