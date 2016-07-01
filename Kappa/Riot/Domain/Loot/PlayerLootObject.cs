using System.Collections.Generic;
using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.Loot {
    [LcdsService(LcdsServiceNames.Loot, "getAllPlayerLoot")]
    public class PlayerLootObject : LcdsServiceObject {
        public List<PlayerLootDTO> Content { get; }

        public PlayerLootObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<List<PlayerLootDTO>>(payload);
        }
    }

    public class PlayerLootDTO : JSONSerializable {
        [JSONField("playerId")]
        public long PlayerId { get; set; }

        [JSONField("lootName")]
        public string LootName { get; set; }

        [JSONField("count")]
        public int Count { get; set; }

        [JSONField("expiryTime")]
        public long ExpiryTime { get; set; }

        [JSONField("refId")]
        public string RefId { get; set; }
    }
}
