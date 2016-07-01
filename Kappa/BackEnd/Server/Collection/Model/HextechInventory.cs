using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    public class HextechInventory : JSONSerializable {
        [JSONField("champShards")]
        public Dictionary<int, int> ChampionShards { get; set; } = new Dictionary<int, int>();

        [JSONField("champs")]
        public Dictionary<int, int> Champions { get; set; } = new Dictionary<int, int>();

        [JSONField("skinShards")]
        public Dictionary<int, int> SkinShards { get; set; } = new Dictionary<int, int>();

        [JSONField("skins")]
        public Dictionary<int, int> Skins { get; set; } = new Dictionary<int, int>();

        [JSONField("wardSkinShards")]
        public Dictionary<int, int> WardSkinShards { get; set; } = new Dictionary<int, int>();

        [JSONField("wardSkins")]
        public Dictionary<int, int> WardSkins { get; set; } = new Dictionary<int, int>();

        [JSONField("mastery6Tokens")]
        public Dictionary<int, int> Mastery6Tokens { get; set; } = new Dictionary<int, int>();

        [JSONField("mastery7Tokens")]
        public Dictionary<int, int> MasteryyTokens { get; set; } = new Dictionary<int, int>();

        [JSONField("chests")]
        public int Chests { get; set; }

        [JSONField("keys")]
        public int Keys { get; set; }

        [JSONField("keyFragments")]
        public int KeyFragments { get; set; }

        [JSONField("blueEssence")]
        public int BlueEssence { get; set; }

        [JSONField("orangeEssence")]
        public int OrangeEssence { get; set; }
    }
}
