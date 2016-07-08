using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Collection.Model;
using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON;
using Kappa.Riot.Domain.Loot;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class HextechService : JSONService {
        private Session session;

        private static Dictionary<string, LootItemDTO> itemDefinitions;

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

        private HextechInventory inventory = new HextechInventory();

        [Async("/update")]
        public event EventHandler<HextechInventory> Update;

        public HextechService(Session session) : base("/collection/hextech") {
            this.session = session;

            var messages = new MessageConsumer(session);
            messages.Consume<SimpleDialogMessage>(OnSimpleDialogMessage);
        }

        private bool OnSimpleDialogMessage(SimpleDialogMessage msg) {
            switch (msg.TitleCode) {
            case "championMasteryLootGrant":
                var arg = JSONParser.ParseObject((string) msg.Params[0]);
                var rawLoot = JSONDeserializer.Deserialize<ChampionMasteryLootGrant>(arg);

                Add(inventory, true, rawLoot.LootName, 1, null);
                OnUpdate();
                break;
            }

            return true;
        }

        [Endpoint("/inventory")]
        public async Task<HextechInventory> GetInventory() {
            var all = await this.session.LootService.GetAllPlayerLootAndDefinitions(0, 0) as PlayerLootAndDefinitionsObject;
            itemDefinitions = all.Content.LootItemList.LootItems.ToDictionary(item => item.LootName);

            inventory = new HextechInventory();

            foreach (var loot in all.Content.PlayerLoot) {
                Add(inventory, true, loot.LootName, loot.Count, loot.RefId);
            }

            return inventory;
        }

        internal static void Add(HextechInventory inventory, bool add, string lootName, int count, string refId) {
            var item = itemDefinitions[lootName];

            if (!add) count *= -1;

            switch (item.Type) {
            case LootItemType.CHAMPION:
                Add(inventory.Champs, item.StoreItemId, count);
                break;

            case LootItemType.CHAMPION_RENTAL:
                Add(inventory.ChampShards, item.StoreItemId, count);
                break;

            case LootItemType.SKIN:
                Add(inventory.Skins, item.StoreItemId, count);
                break;

            case LootItemType.SKIN_RENTAL:
                Add(inventory.SkinShards, item.StoreItemId, count);
                break;

            case LootItemType.WARDSKIN:
                Add(inventory.WardSkins, item.StoreItemId, count);
                break;

            case LootItemType.WARDSKIN_RENTAL:
                Add(inventory.WardSkinShards, item.StoreItemId, count);
                break;

            case LootItemType.CHAMPION_TOKEN:
                int champ = int.Parse(refId);
                switch (item.LootName) {
                case LootNames.MASTERY_TOKEN_6:
                    Add(inventory.Mastery6Tokens, champ, count);
                    break;
                case LootNames.MASTERY_TOKEN_7:
                    Add(inventory.Mastery7Tokens, champ, count);
                    break;
                default:
                    Session.Log("Unknown champion token: " + item.LootName);
                    break;
                }
                break;

            case LootItemType.CHEST:
                inventory.Chests += count;
                break;

            case LootItemType.CURRENCY:
                switch (item.LootName) {
                case LootNames.BLUE_ESSENCE:
                    inventory.BlueEssence = count;
                    break;
                case LootNames.ORANGE_ESSENCE:
                    inventory.OrangeEssence = count;
                    break;
                default:
                    Session.Log("Unknown currency: " + item.LootName);
                    break;
                }
                break;

            case LootItemType.MATERIAL:
                switch (item.LootName) {
                case LootNames.KEY:
                    inventory.Keys = count;
                    break;
                case LootNames.KEY_FRAGMENT:
                    inventory.KeyFragments = count;
                    break;
                default:
                    Session.Log("Unknown material: " + item.LootName);
                    break;
                }
                break;

            default:
                Session.Log(item.Type);
                break;
            }
        }

        private static void Add<TKey>(IDictionary<TKey, int> dict, TKey key, int value) {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
            else
                dict[key] += value;
        }

        private void OnUpdate() {
            Update?.Invoke(this, inventory);
        }
    }
}
