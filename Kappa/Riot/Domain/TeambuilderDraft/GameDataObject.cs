using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.Riot.Domain.TeambuilderDraft {
    [LcdsService(LcdsServiceNames.TeambuilderDraft, "tbdGameDtoV1")]
    public class GameDataObject : LcdsServiceObject {
        public GameData Content { get; }

        public GameDataObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<GameData>(payload);
        }
    }

    public enum Position {
        UNSELECTED,
        UTILITY,
        JUNGLE,
        MIDDLE,
        BOTTOM,
        TOP,
        FILL
    }

    public enum Phase {
        ERROR,
        DRAFT_PREMADE,
        MATCHMAKING,
        AFK_CHECK,
        CHAMPION_SELECT
    }

    public enum ChampSelectActionType {
        BAN,
        PICK
    }

    public enum ChampSelectPhase {
        PLANNING,
        BAN_PICK,
        FINALIZATION,
        GAME_STARTING
    }

    public enum TBDTradeState {
        AVAILABLE,
        INVALID,
        SENT,
        RECEIVED,
        BUSY
    }

    public class GameData : JSONSerializable {
        [JSONField("counter")]
        public int Counter { get; set; }

        [JSONField("phaseName")]
        public Phase CurrentPhase { get; set; }

        [JSONField("premadeState")]
        public PremadeState PremadeState { get; set; }

        [JSONField("championSelectState")]
        public ChampSelectState ChampSelectState { get; set; }

        [JSONField("matchmakingState")]
        public MatchmakingState MatchmakingState { get; set; }

        [JSONField("afkCheckState")]
        public AfkCheckState AfkCheckState { get; set; }
    }

    public class ChampSelectState : JSONSerializable {
        [JSONField("teamId")]
        public string TeamId { get; set; }

        [JSONField("teamChatRoomId")]
        public string TeamChatRoomId { get; set; }

        [JSONField("subphase")]
        public ChampSelectPhase Subphase { get; set; }

        [JSONField("actionSetList")]
        public List<ChampSelectAction[]> Actions { get; set; }

        [JSONField("currentActionSetIndex")]
        public int CurrentActionIndex { get; set; }

        [JSONField("cells")]
        public Cells Cells { get; set; }

        [JSONField("localPlayerCellId")]
        public int MyCellId { get; set; }

        [JSONField("bans")]
        public Bans Bans { get; set; }

        [JSONField("currentTotalTimeMillis")]
        public int CurrentTotalMillis { get; set; }

        [JSONField("currentTimeRemainingMillis")]
        public int CurrentRemainingMillis { get; set; }

        [JSONField("trades")]
        public List<Trade> Trades { get; set; }
    }

    public class Bans : JSONSerializable {
        [JSONField("alliedBans")]
        public List<int> AlliedBans { get; set; }

        [JSONField("enemyBans")]
        public List<int> EnemyBans { get; set; }
    }

    public class Cells : JSONSerializable {
        [JSONField("alliedTeam")]
        public Cell[] AlliedTeam { get; set; }

        [JSONField("enemyTeam")]
        public Cell[] EnemyTeam { get; set; }
    }

    public class Cell : JSONSerializable {
        [JSONField("teamId")]
        public int TeamId { get; set; }

        [JSONField("cellId")]
        public int CellId { get; set; }

        [JSONField("summonerName")]
        public string Name { get; set; }

        [JSONField("championPickIntent")]
        public int ChampionPickIntent { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("assignedPosition")]
        public Position AssignedPosition { get; set; }

        [JSONField("spell1Id")]
        public int Spell1Id { get; set; }

        [JSONField("spell2Id")]
        public int Spell2Id { get; set; }
    }

    public class ChampSelectAction : JSONSerializable {
        [JSONField("actionId")]
        public int ActionId { get; set; }

        [JSONField("actorCellId")]
        public int ActorCellId { get; set; }

        [JSONField("type")]
        public ChampSelectActionType Type { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("completed")]
        public bool Completed { get; set; }
    }

    public class Trade : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("cellId")]
        public int CellId { get; set; }

        [JSONField("state")]
        public TBDTradeState State { get; set; }
    }

    public class PremadeState : JSONSerializable {
        [JSONField("timer")]
        public int Timer { get; set; }

        [JSONField("draftPremadeId")]
        public string PremadeId { get; set; }

        [JSONField("premadeChatRoomId")]
        public string ChatRoomId { get; set; }

        [JSONField("captainSlotId")]
        public int CaptainSlot { get; set; }

        [JSONField("readyToMatchmake")]
        public bool ReadyToMatchmake { get; set; }

        [JSONField("draftSlots")]
        public List<SlotData> Slots { get; set; }

        [JSONField("playableDraftPositions")]
        public List<Position> PlayablePositions { get; set; }

        [JSONField("localPlayerSlotId")]
        public int MySlot { get; set; }
    }

    public class SlotData : JSONSerializable {
        [JSONField("slotId")]
        public int SlotId { get; set; }

        [JSONField("summonerName")]
        public string SummonerName { get; set; }

        [JSONField("draftPositionPreferences")]
        public List<Position> Roles { get; set; }
    }

    public class MatchmakingState : JSONSerializable {
        [JSONField("estimatedMatchmakingTimeMillis")]
        public long EstimatedMatchmakingTime { get; set; }

        [JSONField("timeInMatchmakingMillis")]
        public long TimeInMatchmaking { get; set; }

        [JSONField("backwardsTransitionReason")]
        public string BackwardsTransitionReason { get; set; }
    }

    public class AfkCheckState : JSONSerializable {
        [JSONField("maxAfkMillis")]
        public long MaxDuration { get; set; }

        [JSONField("remainingAfkMillis")]
        public long RemainingDuration { get; set; }

        [JSONField("afkReady")]
        public bool IsReady { get; set; }

        [JSONField("inventoryDraft")]
        public Inventory Inventory { get; set; }
    }

    public class Inventory : JSONSerializable {
        [JSONField("lastSelectedSkinIdByChampionId")]
        public Dictionary<string, int> LastSelectedSkins { get; set; }

        [JSONField("skinIds")]
        public List<int> SkinIds { get; set; }

        [JSONField("spellIds")]
        public List<int> SpellIds { get; set; }

        [JSONField("initialSpellIds")]
        public List<int> InitialSpells { get; set; }

        [JSONField("allChampionIds")]
        public List<int> AllChampionIds { get; set; }

        [JSONField("disabledChampionIds")]
        public List<int> DisabledChampionIds { get; set; }
    }
}
