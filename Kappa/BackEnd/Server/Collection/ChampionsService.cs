using System.Collections.Generic;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Collection.Model;
using Kappa.Riot.Domain.ChampionMastery;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class ChampionsService : JSONService {
        private Session session;

        public ChampionsService(Session session) : base("/collection/champions") {
            this.session = session;
        }

        [Endpoint("/inventory")]
        public async Task<ChampionInventory> GetChampions() {
            return new ChampionInventory(await session.InventoryService.GetAvailableChampions());
        }

        [Endpoint("/mastery")]
        public async Task<List<ChampionMasteryDTO>> GetChampionMastery(long summoner) {
            var all = await session.ChampionMasteryService.GetAllChampionMasteries(summoner);
            var expected = all as AllChampionMasteryObject;
            return expected?.Content;
        }
    }
}
