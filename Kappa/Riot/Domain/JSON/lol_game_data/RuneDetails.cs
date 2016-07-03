using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class RuneDetails : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("description")]
        public string Description { get; set; }

        [JSONField("image")]
        public string Image { get; set; }

        [JSONField("stats")]
        public Dictionary<string, double> Stats { get; set; }

        [JSONField("rune")]
        public RuneInfo Rune { get; set; }
    }

    public class RuneInfo : JSONSerializable {
        [JSONField("tier")]
        public int Tier { get; set; }

        [JSONField("type")]
        public string Type { get; set; }
    }

    public class RuneSlots : JSONSerializable {
        [JSONField("slots")]
        public Dictionary<string, RuneSlot> Slots { get; set; }
    }

    public class RuneSlot : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("type")]
        public string Type { get; set; }

        [JSONField("unlockLevel")]
        public int UnlockLevel { get; set; }
    }
}
