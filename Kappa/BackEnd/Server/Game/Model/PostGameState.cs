using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Collection.Model;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class PostGameState {
        public Guid Chatroom { get; set; }
        public int IpEarned { get; set; }
        public int IpTotal { get; set; }
        public int IpLifetime { get; set; }

        public HextechInventory Hextech { get; set; }

        public PostGameChampionMastery ChampionMastery { get; set; }
    }
}
