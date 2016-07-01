using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    public class Inventory : JSONSerializable {
        [JSONField("pickableChamps")]
        public List<int> PickableChampions { get; set; }

        [JSONField("bannableChamps")]
        public List<int> BannableChampions { get; set; }

        [JSONField("availableSpells")]
        public List<int> AvailableSpells { get; set; }
    }
}
