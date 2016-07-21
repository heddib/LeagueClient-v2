using MFroehlich.Parsing.JSON;
using System.Threading.Tasks;
using Kappa.BackEnd;
using Kappa.Riot.Domain.TeambuilderDraft;
using LeagueSharp;

namespace Kappa.Riot.Services.Lcds {
    internal class TeambuilderDraftService : LcdsService {
        protected override string Destination { get; } = LcdsServiceNames.TeambuilderDraft;

        public TeambuilderDraftService(Session session, LcdsProxyService proxy) : base(session, proxy) { }

        public Task<LcdsServiceObject> AbandonLeaverBusterLowPriorityQueue(string accessTokenStr = null) {
            var json = new JSONObject {
                ["accessTokenStr"] = accessTokenStr,
            };
            return Invoke("abandonLeaverBusterLowPriorityQueueV1", json);
        }

        public Task<LcdsServiceObject> CreateDraftPremade(int queueId) {
            var json = new JSONObject {
                ["queueId"] = queueId,
            };
            return Invoke("createDraftPremadeV1", json);
        }

        public Task<LcdsServiceObject> IndicateAfkReadiness(bool afkReady) {
            var json = new JSONObject {
                ["afkReady"] = afkReady,
            };
            return Invoke("indicateAfkReadinessV1", json);
        }

        public Task<LcdsServiceObject> JoinDraftPremade(string draftPremadeId) {
            var json = new JSONObject {
                ["draftPremadeId"] = draftPremadeId,
            };
            return Invoke("joinDraftPremadeV1", json);
        }

        public Task<LcdsServiceObject> KickPlayer(int slotId) {
            var json = new JSONObject {
                ["slotId"] = slotId,
            };
            return Invoke("kickPlayerV1", json);
        }

        public Task<LcdsServiceObject> LockChampionBan() {
            var json = new JSONObject();
            return Invoke("lockChampionBanV1", json);
        }

        public Task<LcdsServiceObject> LockChampionPick() {
            var json = new JSONObject();
            return Invoke("lockChampionPickV1", json);
        }

        public Task<LcdsServiceObject> LockChampionFromPickIntent() {
            return Invoke("lockChampionFromPickIntentV1", new JSONObject());
        }

        public Task<LcdsServiceObject> PromoteToCaptain(int slotId) {
            var json = new JSONObject {
                ["slotId"] = slotId,
            };
            return Invoke("promoteToCaptainV1", json);
        }

        public Task<LcdsServiceObject> Quit() {
            return Invoke("quitV2", new JSONObject());
        }

        public Task<LcdsServiceObject> RetrieveFeatureToggles() {
            var json = new JSONObject();
            return Invoke("retrieveFeatureToggles", json);
        }

        public Task<LcdsServiceObject> SelectChampionBan(int championId) {
            var json = new JSONObject {
                ["championId"] = championId,
            };
            return Invoke("selectChampionBanV1", json);
        }

        public Task<LcdsServiceObject> SelectChampionPick(int championId) {
            var json = new JSONObject {
                ["championId"] = championId,
            };
            return Invoke("selectChampionPickV1", json);
        }

        public Task<LcdsServiceObject> SignalChampionPickIntent(int championId) {
            var json = new JSONObject {
                ["championId"] = championId,
            };
            return Invoke("signalChampionPickIntentV1", json);
        }

        public Task<LcdsServiceObject> SpecifyDraftPositionPreferences(Position firstPreference, Position secondPreference) {
            var json = new JSONObject {
                ["firstPreference"] = firstPreference,
                ["secondPreference"] = secondPreference,
            };
            return Invoke("specifyDraftPositionPreferencesV1", json);
        }

        public Task<LcdsServiceObject> StartMatchmaking(string accessTokenStr = null) {
            var json = new JSONObject {
                ["accessTokenStr"] = accessTokenStr,
            };
            return Invoke("startMatchmakingV1", json);
        }

        public Task<LcdsServiceObject> LeaveMatchmaking() {
            var json = new JSONObject();
            return Invoke("leaveMatchmakingV1", json);
        }

        public Task<LcdsServiceObject> AcceptTrade(int tradeId) {
            var json = new JSONObject {
                ["tradeId"] = tradeId,
            };
            return Invoke("acceptTradeV1", json);
        }

        public Task<LcdsServiceObject> DeclineTrade(int tradeId) {
            var json = new JSONObject {
                ["tradeId"] = tradeId,
            };
            return Invoke("declineTradeV1", json);
        }

        public Task<LcdsServiceObject> GetWallet(long accountId) {
            var json = new JSONObject {
                ["accountId"] = accountId,
            };
            return Invoke("getWallet", json);
        }

        public Task<LcdsServiceObject> GetItems(int championId, string region, string language) {
            var json = new JSONObject {
                ["tag"] = "champions_" + championId,
                ["inventoryTypes"] = new JSONArray { "CHAMPION_SKIN" },
                ["region"] = Region.Current.Platform,
                ["language"] = new JSONArray { Region.Locale },
            };
            return Invoke("getItems", json);
        }

        public Task<LcdsServiceObject> PickSkin(int skinId, bool isNewlyPurchasedSkin) {
            var json = new JSONObject {
                ["skinId"] = skinId,
                ["isNewlyPurchasedSkin"] = isNewlyPurchasedSkin,
            };
            return Invoke("pickSkinV1", json);
        }

        public Task<LcdsServiceObject> PurchaseItem(long accountId, int itemId, string itemType, string currencyType, int cost) {
            var json = new JSONObject {
                ["accountId"] = accountId,
                ["itemId"] = itemId,
                ["itemType"] = itemType,
                ["currencyType"] = currencyType,
                ["cost"] = cost,
                ["quantity"] = 1,
            };
            return Invoke("purchaseItem", json);
        }

        public Task<LcdsServiceObject> RetrieveLatestTbdGameDto() {
            var json = new JSONObject();
            return Invoke("retrieveLatestTbdGameDtoV1", json);
        }

        public Task<LcdsServiceObject> PickSpells(int spell1Id, int spell2Id) {
            var json = new JSONObject {
                ["spell1Id"] = spell1Id,
                ["spell2Id"] = spell2Id,
            };
            return Invoke("pickSpellsV1", json);
        }
    }
}
