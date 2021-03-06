﻿using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    [JSONSerializable]
    public class MasteryBook {
        public long Selected { get; set; }
        public List<MasteryPage> Pages { get; set; } = new List<MasteryPage>();
    }

    [JSONSerializable]
    public class MasteryPage {
        public long Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, int> Masteries { get; set; } = new Dictionary<string, int>();
    }
}
