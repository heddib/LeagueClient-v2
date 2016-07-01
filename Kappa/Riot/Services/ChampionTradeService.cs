using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class ChampionTradeService : Service {
        protected override string Destination => "lcdsChampionTradeService";

        public ChampionTradeService(Session session) : base(session) { }

        /// <summary>
        /// Gets the allowed traders in the current game
        /// </summary>
        /// <returns>Returns a list of traders</returns>
        public Task<PotentialTradersDTO> GetPotentialTraders() {
            return InvokeAsync<PotentialTradersDTO>("getPotentialTraders");
        }

        /// <summary>
        /// Attempts to trade with a player
        /// </summary>
        /// <param name="summonerInternalName">The internal name of a summoner</param>
        /// <param name="championId">The champion id requested</param>
        public Task<object> AttemptTrade(string summonerInternalName, int championId) {
            return InvokeAsync<object>("attemptTrade", summonerInternalName, championId, false);
        }

        /// <summary>
        /// Decline the current trade
        /// </summary>
        public Task<object> DeclineTrade() {
            return InvokeAsync<object>("dismissTrade");
        }

        /// <summary>
        /// Accepts the current trade
        /// </summary>
        /// <param name="summonerInternalName">The internal name of a summoner</param>
        /// <param name="championId">The champion id requested</param>
        public Task<object> AcceptTrade(string summonerInternalName, int championId) {
            return InvokeAsync<object>("attemptTrade", summonerInternalName, championId, true);
        }
    }
}
