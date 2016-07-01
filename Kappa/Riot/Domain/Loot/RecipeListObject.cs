using System.Collections.Generic;
using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.Loot {
    [LcdsService(LcdsServiceNames.Loot, "getAllRecipes")]
    public class RecipeListObject : LcdsServiceObject {
        public RecipeListDTO Content { get; }

        public RecipeListObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<RecipeListDTO>(payload);
        }
    }

    public enum RecipeType {
        NONE,
        OPEN,
        REROLL,
        DISENCHANT,
        UPGRADE,
        FORGE,
        TOKENESSENCEUPGRADE,
        TOKENSHARDUPGRADE
    }

    public class RecipeListDTO : JSONSerializable {
        [JSONField("recipes")]
        public List<RecipeDTO> Recipes { get; set; }

        [JSONField("lastUpdate")]
        public long LastUpdate { get; set; }
    }

    public class RecipeDTO : JSONSerializable {
        [JSONField("recipeName")]
        public string RecipeName { get; set; }

        [JSONField("type")]
        public RecipeType Type { get; set; }

        [JSONField("displayCategories")]
        public string DisplayCategories { get; set; }

        [JSONField("slots")]
        public List<RecipeSlotDTO> Slots { get; set; }

        [JSONField("outputs")]
        public List<RecipeOutputDTO> Outputs { get; set; }
    }

    public class RecipeSlotDTO : JSONSerializable {
        [JSONField("slotNumber")]
        public int SlotNumber { get; set; }

        [JSONField("query")]
        public string Query { get; set; }

        [JSONField("quantityExpression")]
        public string QuantityExpression { get; set; }
    }

    public class RecipeOutputDTO : JSONSerializable {
        [JSONField("lootName")]
        public string LootName { get; set; }

        [JSONField("quantityExpression")]
        public string QuantityExpression { get; set; }

        [JSONField("probability")]
        public double Probability { get; set; }

        [JSONField("allowDuplicates")]
        public bool AllowDuplicates { get; set; }
    }
}
