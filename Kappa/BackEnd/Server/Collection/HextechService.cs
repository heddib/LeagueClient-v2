using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Collection.Model;
using Kappa.Riot.Domain.Loot;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class HextechService : JSONService {
        private Session session;

        // ReSharper disable InconsistentNaming
        private static class LootNames {
            public const string
                MASTERY_TOKEN_6 = "CHAMPION_TOKEN_6",
                MASTERY_TOKEN_7 = "CHAMPION_TOKEN_7",
                ORANGE_ESSENCE = "CURRENCY_cosmetic",
                BLUE_ESSENCE = "CURRENCY_champion",
                KEY_FRAGMENT = "MATERIAL_key_fragment",
                KEY = "MATERIAL_key";

        }
        // ReSharper restore InconsistentNaming

        public HextechService(Session session) : base("/collection/hextech") {
            this.session = session;
        }

        [Endpoint("/inventory")]
        public async Task<HextechInventory> GetInventory() {
            var inventory = new HextechInventory();

            var all = await this.session.LootService.GetAllPlayerLootAndDefinitions(0, 0) as PlayerLootAndDefinitionsObject;
            var items = all.Content.LootItemList.LootItems.ToDictionary(item => item.LootName);
            foreach (var loot in all.Content.PlayerLoot) {
                var item = items[loot.LootName];
                switch (item.Type) {
                case LootItemType.CHAMPION:
                    inventory.Champs.Add(item.StoreItemId, loot.Count);
                    break;

                case LootItemType.CHAMPION_RENTAL:
                    inventory.ChampShards.Add(item.StoreItemId, loot.Count);
                    break;

                case LootItemType.SKIN:
                    inventory.Skins.Add(item.StoreItemId, loot.Count);
                    break;

                case LootItemType.SKIN_RENTAL:
                    inventory.SkinShards.Add(item.StoreItemId, loot.Count);
                    break;

                case LootItemType.WARDSKIN:
                    inventory.WardSkins.Add(item.StoreItemId, loot.Count);
                    break;

                case LootItemType.WARDSKIN_RENTAL:
                    inventory.WardSkinShards.Add(item.StoreItemId, loot.Count);
                    break;

                case LootItemType.CHAMPION_TOKEN:
                    int champ = int.Parse(loot.RefId);
                    switch (loot.LootName) {
                    case LootNames.MASTERY_TOKEN_6:
                        inventory.Mastery6Tokens.Add(champ, loot.Count);
                        break;
                    case LootNames.MASTERY_TOKEN_7:
                        inventory.MasteryyTokens.Add(champ, loot.Count);
                        break;
                    default:
                        Debug.WriteLine("Unknown champion token: " + loot.LootName);
                        break;
                    }
                    break;

                case LootItemType.CHEST:
                    inventory.Chests += loot.Count;
                    break;

                case LootItemType.CURRENCY:
                    switch (item.LootName) {
                    case LootNames.BLUE_ESSENCE:
                        inventory.BlueEssence = loot.Count;
                        break;
                    case LootNames.ORANGE_ESSENCE:
                        inventory.OrangeEssence = loot.Count;
                        break;
                    default:
                        Debug.WriteLine("Unknown currency: " + loot.LootName);
                        break;
                    }
                    break;

                case LootItemType.MATERIAL:
                    switch (item.LootName) {
                    case LootNames.KEY:
                        inventory.Keys = loot.Count;
                        break;
                    case LootNames.KEY_FRAGMENT:
                        inventory.KeyFragments = loot.Count;
                        break;
                    default:
                        Debug.WriteLine("Unknown material: " + loot.LootName);
                        break;
                    }
                    break;

                default:
                    Debug.WriteLine(item.Type);
                    break;
                }
            }

            return inventory;
        }
    }
}
