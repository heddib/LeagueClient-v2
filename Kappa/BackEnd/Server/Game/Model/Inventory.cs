using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class Inventory {
        public List<int> PickableChamps { get; set; }
        public List<int> BannableChamps { get; set; }
        public List<int> AvailableSpells { get; set; }
    }
}
