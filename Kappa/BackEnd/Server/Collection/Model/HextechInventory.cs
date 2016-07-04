using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    [JSONSerializable]
    public class HextechInventory {
        public Dictionary<int, int> ChampShards { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, int> Champs { get; set; } = new Dictionary<int, int>();

        public Dictionary<int, int> SkinShards { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, int> Skins { get; set; } = new Dictionary<int, int>();

        public Dictionary<int, int> WardSkinShards { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, int> WardSkins { get; set; } = new Dictionary<int, int>();

        public Dictionary<int, int> Mastery6Tokens { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, int> Mastery7Tokens { get; set; } = new Dictionary<int, int>();

        public int Chests { get; set; }
        public int Keys { get; set; }
        public int KeyFragments { get; set; }

        public int BlueEssence { get; set; }
        public int OrangeEssence { get; set; }
    }
}
