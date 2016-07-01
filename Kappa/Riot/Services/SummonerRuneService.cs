using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class SummonerRuneService : Service {
    protected override string Destination => "summonerRuneService";

    public SummonerRuneService(Session session) : base(session) { }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="summonerId">The summoner ID for the user</param>
    /// <returns>Returns the inventory for the user</returns>
    public Task<SummonerRuneInventory> GetSummonerRunes(long summonerId) {
      return InvokeAsync<SummonerRuneInventory>("getSummonerRunes", summonerId);
    }

    /// <summary>
    /// Get the runes the user owns.
    /// </summary>
    /// <param name="summonerId">The summoner ID for the user</param>
    /// <returns>Returns the mastery books for the user</returns>
    public Task<SummonerRuneInventory> GetSummonerRuneInventory(long summonerId) {
      return InvokeAsync<SummonerRuneInventory>("getSummonerRuneInventory", summonerId);
    }
  }
}
