using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Diagnostics.Model {
    [JSONSerializable]
    public class Versions {
        public string Game { get; set; }
        public string Patch { get; set; }
    }
}
