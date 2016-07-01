using Kappa.Riot.Domain;
using Kappa.BackEnd.Server.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class SkinsService : JSONService {
        private Session session;

        private ChampionDTO[] champs;

        public SkinsService(Session session) : base("/collection/skins") {
            this.session = session;
            session.Authed += Session_Authed;
        }

        private async void Session_Authed(object sender, EventArgs e) {
            champs = await this.session.InventoryService.GetAvailableChampions();
        }

        [Endpoint("/owned")]
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
