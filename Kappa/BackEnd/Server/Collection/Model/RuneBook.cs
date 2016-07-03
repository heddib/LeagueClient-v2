using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    [JSONSerializable]
    public class RuneBook {
        public long Selected { get; set; }
        public List<RunePage> Pages { get; set; } = new List<RunePage>();
    }

    [JSONSerializable]
    public class RunePage {
        public long Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, int> Runes { get; set; } = new Dictionary<string, int>();
    }
}
