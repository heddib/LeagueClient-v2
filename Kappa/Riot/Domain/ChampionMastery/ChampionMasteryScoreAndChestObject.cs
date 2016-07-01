using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.ChampionMastery {
    [LcdsService(LcdsServiceNames.ChampionMastery, "getChampionMasteryScoreAndChest")]
    public class ChampionMasteryScoreAndChestObject : LcdsServiceObject {
        public ChampionMasteryScoreAndChest Content { get; }

        public ChampionMasteryScoreAndChestObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<ChampionMasteryScoreAndChest>(payload);
        }
    }

    public class ChampionMasteryScoreAndChest : JSONSerializable {
        [JSONField("score")]
        public int Score { get; set; }

        [JSONField("earnableChests")]
        public int EarnableChests { get; set; }

        [JSONField("maximumChests")]
        public int MaximumChests { get; set; }

        [JSONField("nextChestRechargeTime")]
        public long NextChestRechargeTime { get; set; }
    }
}
