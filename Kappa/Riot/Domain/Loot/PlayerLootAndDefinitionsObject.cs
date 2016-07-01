using System.Collections.Generic;
using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.Loot {
    [LcdsService(LcdsServiceNames.Loot, "getAllPlayerLootAndAllDefinitions")]
    public class PlayerLootAndDefinitionsObject : LcdsServiceObject {
        public PlayerLootDefinitionsDTO Content { get; }

        public PlayerLootAndDefinitionsObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<PlayerLootDefinitionsDTO>(payload);
        }
    }

    public class PlayerLootDefinitionsDTO : JSONSerializable {
        [JSONField("lootItemList")]
        public LootItemListDTO LootItemList { get; set; }

        [JSONField("recipeList")]
        public RecipeListDTO RecipeList { get; set; }

        [JSONField("queryResult")]
        public QueryListDTO QueryList { get; set; }

        [JSONField("playerLoot")]
        public List<PlayerLootDTO> PlayerLoot { get; set; }
    }
}
