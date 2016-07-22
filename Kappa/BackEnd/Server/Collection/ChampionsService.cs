using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Collection.Model;
using Kappa.Riot.Domain;
using Kappa.Riot.Domain.ChampionMastery;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class ChampionsService : JSONService {
        private Session session;
        private ChampionDTO[] champs;

        public ChampionsService(Session session) : base("/collection/champions") {
            this.session = session;
            session.Authed += Session_Authed;
        }

        private async void Session_Authed(object sender, EventArgs e) {
            session.Authed += Session_Authed;
            champs = await this.session.InventoryService.GetAvailableChampions();
        }

        [Endpoint("/inventory")]
        public async Task<ChampionInventory> GetChampions() {
            return new ChampionInventory(await this.session.InventoryService.GetAvailableChampions());
        }

        [Endpoint("/mastery")]
        public async Task<List<ChampionMasteryDTO>> GetChampionMastery(long summoner) {
            var all = await session.ChampionMasteryService.GetAllChampionMasteries(summoner);
            var expected = all as AllChampionMasteryObject;
            return expected?.Content;
        }

        [Endpoint("/skins")]
        public Dictionary<int, List<Skin>> GetOwnedSkins() {
            var map = new Dictionary<int, List<Skin>>();
            foreach (var champ in champs) {
                map[champ.ChampionId] = (from s in champ.ChampionSkins
                                         where s.Owned
                                         select new Skin(s)).ToList();
            }
            return map;
        }
    }
}
