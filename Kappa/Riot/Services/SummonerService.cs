using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class SummonerService : Service {
        protected override string Destination => "summonerService";

        public SummonerService(Session session) : base(session) { }

        /// <summary>
        /// Gets summoner data by account id
        /// </summary>
        /// <param name="accountId">The account id</param>
        /// <returns>Returns all the summoner data for an account</returns>
        public Task<AllSummonerData> GetAllSummonerDataByAccount(long accountId) {
            return InvokeAsync<AllSummonerData>("getAllSummonerDataByAccount");
        }

        public Task<object> GetSummonerCatalog() {
            return InvokeAsync<object>("getSummonerCatalog");
        }

        /// <summary>
        /// Gets summoner by name
        /// </summary>
        /// <param name="summonerName">The name of the summoner</param>
        /// <returns>Returns the summoner</returns>
        public Task<PublicSummoner> GetSummonerByName(string summonerName) {
            return InvokeAsync<PublicSummoner>("getSummonerByName", summonerName);
        }

        /// <summary>
        /// Gets the public summoner data by account id
        /// </summary>
        /// <param name="accountId">The account id</param>
        /// <returns>Returns all the public summoner data for an account</returns>
        public Task<AllPublicSummonerDataDTO> GetAllPublicSummonerDataByAccount(long accountId) {
            return InvokeAsync<AllPublicSummonerDataDTO>("getAllPublicSummonerDataByAccount", accountId);
        }

        /// <summary>
        /// Gets the summoner internal name of a summoner
        /// </summary>
        /// <param name="summonerName">The summoner name</param>
        /// <returns>Returns a summoners internal name</returns>
        public Task<string> GetSummonerInternalNameByName(string summonerName) {
            return InvokeAsync<string>("getSummonerInternalNameByName", summonerName);
        }

        /// <summary>
        /// Updates the profile icon for the user
        /// </summary>
        /// <param name="iconId">The icon id</param>
        public Task<object> UpdateProfileIconId(int iconId) {
            return InvokeAsync<object>("updateProfileIconId", iconId);
        }

        /// <summary>
        /// Get the summoner names for an array of Summoner IDs.
        /// </summary>
        /// <param name="summonerIds">Array of Summoner IDs</param>
        /// <returns>Returns an array of Summoner Names</returns>
        public Task<string[]> GetSummonerNames(long[] summonerIds) {
            return InvokeAsync<string[]>("getSummonerNames", summonerIds);
        }

        /// <summary>
        /// Get the summoner icons for an array of Summoner IDs.
        /// </summary>
        /// <param name="summonerIds">Array of Summoner IDs</param>
        /// <returns>Returns an array of Summoner Names</returns>
        public Task<string> GetSummonerIcons(long[] summonerIds) {
            return InvokeAsync<string>("getSummonerIcons", summonerIds);
        }

        /// <summary>
        /// Sends a players display name when logging in.
        /// </summary>
        /// <param name="playerName">Display name for the summoner</param>
        /// <returns></returns>
        public Task<AllSummonerData> CreateDefaultSummoner(string playerName) {
            return InvokeAsync<AllSummonerData>("createDefaultSummoner", playerName);
        }
    }
}
