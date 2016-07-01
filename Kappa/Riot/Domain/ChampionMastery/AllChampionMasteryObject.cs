using System.Collections.Generic;
using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.ChampionMastery {
    [LcdsService(LcdsServiceNames.ChampionMastery, "getAllChampionMasteries")]
    public class AllChampionMasteryObject : LcdsServiceObject {
        public List<ChampionMasteryDTO> Content { get; }

        public AllChampionMasteryObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<List<ChampionMasteryDTO>>(payload);
        }
    }

    public class ChampionMasteryDTO : JSONSerializable {
        [JSONField("highestGrade")]
        public string HighestGrade { get; set; }

        [JSONField("playerId")]
        public long PlayerId { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("championLevel")]
        public int ChampionLevel { get; set; }

        [JSONField("championPoints")]
        public int ChampionPoints { get; set; }

        [JSONField("lastPlayTime")]
        public long LastPlayTime { get; set; }

        [JSONField("championPointsSinceLastLevel")]
        public int ChampionPointsSinceLastLevel { get; set; }

        [JSONField("championPointsUntilNextLevel")]
        public int ChampionPointsUntilNextLevel { get; set; }

        [JSONField("ChestGranted")]
        public bool ChestGranted { get; set; }
    }
}
