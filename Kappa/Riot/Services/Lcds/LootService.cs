using System.Collections.Generic;
using MFroehlich.Parsing.JSON;
using System.Threading.Tasks;
using Kappa.BackEnd;
using System.Linq;

namespace Kappa.Riot.Services.Lcds {
    internal class LootService : LcdsService {
        protected override string Destination { get; } = LcdsServiceNames.Loot;

        public LootService(Session session, LcdsProxyService proxy) : base(session, proxy) { }

        public Task<LcdsServiceObject> GetAllPlayerLootAndDefinitions(int lastLootItemUpdate, int lastRecipeUpdate) {
            var json = new JSONArray { lastLootItemUpdate, lastRecipeUpdate };
            return Invoke("getAllPlayerLootAndAllDefinitions", json);
        }

        public Task<LcdsServiceObject> GetAllRecipes(int lastUpdate) {
            var json = new JSONArray { lastUpdate };
            return Invoke("getAllRecipes", json);
        }

        public Task<LcdsServiceObject> GetAllQueries(int lastUpdate) {
            var json = new JSONArray { lastUpdate };
            return Invoke("getAllQueries", json);
        }

        public Task<LcdsServiceObject> GetAllLootItems(int lastUpdate) {
            var json = new JSONArray { lastUpdate };
            return Invoke("getAllLootItems", json);
        }

        public Task<LcdsServiceObject> GetAllPlayerLoot() {
            var json = new JSONArray();
            return Invoke("getAllPlayerLoot", json);
        }

        public Task<LcdsServiceObject> RedeemPlayerLoot(string lootName) {
            var json = new JSONArray {
                "atnlasnflast",
                new RedeemLootTransactionDTO { LootName =  lootName }
            };
            return Invoke("redeemPlayerLoot", json);
        }

        public Task<LcdsServiceObject> CraftPlayerLoot(string recipe, string[] loot) {
            var json = new JSONArray {
                "123nlansfa",
                new CraftLootTransactionDTO { LootNames =  loot.ToList(), RecipeName =  recipe}
            };
            return Invoke("craftPlayerLoot", json);
        }

        public Task<LcdsServiceObject> CraftPlayerLootWithRefId(string recipe, LootNameRefId[] loot) {
            var json = new JSONArray {
                "123nlansfa",
                new CraftLootRefTransactionDTO { LootNames =  loot.ToList(), RecipeName =  recipe}
            };
            return Invoke("craftPlayerLootWithRefId", json);
        }

        public Task<LcdsServiceObject> CheckAndGrantNewPlayerChest() {
            var json = new JSONArray();
            return Invoke("checkAndGrantNewPlayerChest", json);
        }

        private class CraftLootTransactionDTO : JSONSerializable {
            [JSONField("lootNames")]
            public List<string> LootNames { get; set; }
            [JSONField("recipeName")]
            public string RecipeName { get; set; }
        }

        private class CraftLootRefTransactionDTO : JSONSerializable {
            [JSONField("lootNameRefIds")]
            public List<LootNameRefId> LootNames { get; set; }
            [JSONField("recipeName")]
            public string RecipeName { get; set; }
        }

        public class LootNameRefId : JSONSerializable {
            [JSONField("lootName")]
            public string LootNames { get; set; }
            [JSONField("refId")]
            public string RecipeName { get; set; }
        }

        private class RedeemLootTransactionDTO : JSONSerializable {
            [JSONField("lootName")]
            public string LootName { get; set; }
        }
    }
}
