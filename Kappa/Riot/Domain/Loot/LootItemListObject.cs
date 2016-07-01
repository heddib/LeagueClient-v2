using System.Collections.Generic;
using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.Loot {
    [LcdsService(LcdsServiceNames.Loot, "getAllLootItems")]
    public class LootItemListObject : LcdsServiceObject {
        public LootItemListDTO Content { get; }

        public LootItemListObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<LootItemListDTO>(payload);
        }
    }

    public enum LootItemRarity {
        DEFAULT = 0,
        EPIC = 1,
        LEGENDARY = 2,
        MYTHIC = 3,
        ULTIMATE = 4
    }

    public enum LootItemType {
        NONE = 14,
        CHAMPION = 0,
        CHAMPION_RENTAL = 1,
        SKIN = 2,
        SKIN_RENTAL = 3,
        WARDSKIN = 4,
        WARDSKIN_RENTAL = 5,
        SUMMONERICON = 6,
        CHEST = 7,
        CHROMA = 8,
        CHROMA_RENTAL = 9,
        CURRENCY = 10,
        MATERIAL = 11,
        CHAMPION_TOKEN = 12,
        BUNDLE = 13
    }

    public class LootItemListDTO : JSONSerializable {
        [JSONField("lootItems")]
        public List<LootItemDTO> LootItems { get; set; }

        [JSONField("lastUpdate")]
        public long LastUpdate { get; set; }
    }

    public class LootItemDTO : JSONSerializable {
        [JSONField("lootName")]
        public string LootName { get; set; }

        [JSONField("type")]
        public LootItemType Type { get; set; }

        [JSONField("rarity")]
        public LootItemRarity Rarity { get; set; }

        [JSONField("value")]
        public int Value { get; set; }

        [JSONField("storeItemId")]
        public int StoreItemId { get; set; }

        [JSONField("upgradeLootName")]
        public string UpgradeLootName { get; set; }

        [JSONField("expiryTime")]
        public long ExpiryTime { get; set; }

        [JSONField("rentalSeconds")]
        public long RentalSeconds { get; set; }

        [JSONField("rentalGames")]
        public int RentalGames { get; set; }

        [JSONField("tags")]
        public string Tags { get; set; }

        [JSONField("displayCategories")]
        public string DisplayCategories { get; set; }

        [JSONField("asset")]
        public string Asset { get; set; }
    }
}
