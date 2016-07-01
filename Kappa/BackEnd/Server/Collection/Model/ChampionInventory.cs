using System.Collections.Generic;
using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    public class ChampionInventory : JSONSerializable {
        [JSONField("owned")]
        public List<int> Owned { get; set; } = new List<int>();

        [JSONField("free")]
        public List<int> Free { get; set; } = new List<int>();

        public ChampionInventory(IEnumerable<ChampionDTO> src) {
            foreach (var champ in src) {
                if (champ.Owned) Owned.Add(champ.ChampionId);
                if (champ.FreeToPlay) Free.Add(champ.ChampionId);
            }
        }
    }
}
