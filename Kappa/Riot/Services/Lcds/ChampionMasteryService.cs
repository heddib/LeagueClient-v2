using MFroehlich.Parsing.JSON;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services.Lcds {
    internal class ChampionMasteryService : LcdsService {
        protected override string Destination { get; } = LcdsServiceNames.ChampionMastery;

        public ChampionMasteryService(Session session, LcdsProxyService proxy) : base(session, proxy) { }

        /// <summary>
        /// Fetches total champion mastery points
        /// </summary>
        /// <param name="summonerId">The summoner to fetch score for</param>
        /// <returns></returns>
        public Task<LcdsServiceObject> GetChampionMasteryScore(long summonerId) {
            var json = new JSONArray { summonerId };
            return Invoke("getChampionMasteryScore", json);
        }

        /// <summary>
        /// Fetches champion mastery scores
        /// </summary>
        /// <param name="summonerId">The summoner to fetch masteries for</param>
        /// <returns></returns>
        public Task<LcdsServiceObject> GetAllChampionMasteries(long summonerId) {
            var json = new JSONArray { summonerId };
            return Invoke("getAllChampionMasteries", json);
        }

        /// <summary>
        /// Fetches champion mastery scores
        /// </summary>
        /// <returns></returns>
        public Task<LcdsServiceObject> GetChampionMasteryScoreAndChest() {
            var json = new JSONArray();
            return Invoke("getChampionMasteryScoreAndChest", json);
        }
    }
}
