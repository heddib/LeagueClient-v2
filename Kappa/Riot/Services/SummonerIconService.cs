using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class SummonerIconService : Service {
        protected override string Destination => "summonerIconService";

        public SummonerIconService(Session session) : base(session) { }

        /// <summary>
        /// Gets the summoner icons for a user
        /// </summary>
        /// <param name="summonerId">The summoner id</param>
        /// <returns>Returns the summoner icons</returns>
        public Task<SummonerIconInventoryDTO> GetSummonerIconInventory(long summonerId) {
            return InvokeAsync<SummonerIconInventoryDTO>("getSummonerIconInventory", summonerId);
        }
    }
}
