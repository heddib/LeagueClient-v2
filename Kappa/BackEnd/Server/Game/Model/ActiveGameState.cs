using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class ActiveGameState {
        public bool InGame { get; set; }
        public bool Launched { get; set; }
    }
}
