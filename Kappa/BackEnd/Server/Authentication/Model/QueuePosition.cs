using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication.Model {
    [JSONSerializable]
    public class QueuePosition {
        public int Position { get; set; }
    }
}
