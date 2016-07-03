using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class MasteriesInfo : JSONSerializable {
        [JSONField("type")]
        public string Type;

        [JSONField("tree")]
        public MasteryTree Tree;

        [JSONField("data")]
        public Dictionary<string, Mastery> Data;
    }

    public class MasteryTree : JSONSerializable {
        [JSONField("groups")]
        public List<MasteryGroup> Groups;
    }

    public class MasteryGroup : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("version")]
        public int Version { get; set; }

        [JSONField("rows")]
        public List<MasteryRow> Rows { get; set; }
    }

    public class MasteryRow : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("pointsToActivate")]
        public int PointsToActivate { get; set; }

        [JSONField("maxPointsInRow")]
        public int MaxPointsInRow { get; set; }

        [JSONField("masteries")]
        public List<int> Masteries { get; set; }
    }

    public class Mastery : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("column")]
        public string Column { get; set; }

        [JSONField("maxRank")]
        public int MaxRank { get; set; }

        [JSONField("minLevel")]
        public int MinLevel { get; set; }

        [JSONField("minTier")]
        public int MinTier { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("description")]
        public string[] Description { get; set; }

        [JSONField("image")]
        public MasteryImageInfo Image { get; set; }
    }

    public class MasteryImageInfo : JSONSerializable {
        [JSONField("icon")]
        public string Icon { get; set; }
    }
}
