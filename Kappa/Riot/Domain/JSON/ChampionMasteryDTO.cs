using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON {
    public class EogChampionMasteryDTO : JSONSerializable {
        [JSONField("shardId")]
        public string ShardId { get; set; }

        [JSONField("gameId")]
        public long GameId { get; set; }

        [JSONField("playerId")]
        public long SummonerId { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("championLevel")]
        public int ChampionLevel { get; set; }

        [JSONField("championPointsBeforeGame")]
        public int ChampionPointsBeforeGame { get; set; }

        [JSONField("championPointsGained")]
        public int ChampionPointsGained { get; set; }

        [JSONField("championPointsGainedIndividualContribution")]
        public int ChampionPointsGainedIndividualContribution { get; set; }

        [JSONField("bonusChampionPointsGained")]
        public int BonusChampionPointsGained { get; set; }

        [JSONField("playerGrade")]
        public string PlayerGrade { get; set; }

        [JSONField("championPointsSinceLastLevelBeforeGame")]
        public int ChampionPointsSinceLastLevelBeforeGame { get; set; }

        [JSONField("championPointsUntilNextLevelBeforeGame")]
        public int ChampionPointsUntilNextLevelBeforeGame { get; set; }

        [JSONField("championPointsUntilNextLevelAfterGame")]
        public int ChampionPointsUntilNextLevelAfterGame { get; set; }

        [JSONField("championLevelUp")]
        public bool ChampionLevelUp { get; set; }

        [JSONField("score")]
        public int Score { get; set; }

        [JSONField("levelUpList")]
        public List<LevelUp> LevelUpList { get; set; }

        [JSONField("memberGrades")]
        public List<OtherPlayerGrade> MemberGrades { get; set; }
    }

    public class LevelUp {
        [JSONField("playerId")]
        public long SummonerId { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("championLevel")]
        public int ChampionLevel { get; set; }
    }

    public class OtherPlayerGrade {
        [JSONField("playerId")]
        public long SummonerId { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("grade")]
        public string Grade { get; set; }
    }

    public class ChampionMasteryLootGrant {
        [JSONField("shardId")]
        public string ShardId { get; set; }

        [JSONField("gameId")]
        public long GameId { get; set; }

        [JSONField("playerId")]
        public long SummonerId { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("playerGrade")]
        public string PlayerGrade { get; set; }

        [JSONField("lootName")]
        public string LootName { get; set; }

        [JSONField("messageKey")]
        public object MessageKey { get; set; }
    }
}
