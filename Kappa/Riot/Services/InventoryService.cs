using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class InventoryService : Service {
    protected override string Destination => "inventoryService";

    public InventoryService(Session session) : base(session) { }

    /// <summary>
    /// Get the current IP & EXP Boosts for the user.
    /// </summary>
    /// <returns>Returns the active boosts for the user</returns>
    public Task<SummonerActiveBoostsDTO> GetSumonerActiveBoosts() {
      return InvokeAsync<SummonerActiveBoostsDTO>("getSumonerActiveBoosts");
    }

    /// <summary>
    /// Get the current champions for the user.
    /// </summary>
    /// <returns>Returns an array of champions</returns>
    public Task<ChampionDTO[]> GetAvailableChampions() {
      return InvokeAsync<ChampionDTO[]>("getAvailableChampions");
    }
  }
}
