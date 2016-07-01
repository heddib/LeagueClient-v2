using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class EndOfGameChampionMastery : JSONSerializable {
        [JSONField("before")]
        public ChampionMasteryState Before { get; set; }

        [JSONField("after")]
        public ChampionMasteryState After { get; set; }

        [JSONField("grade")]
        public string Grade { get; set; }

        [JSONField("champ")]
        public int Champion { get; set; }
    }

    public class ChampionMasteryState : JSONSerializable {
        [JSONField("level")]
        public int Level { get; set; }

        [JSONField("total")]
        public int TotalPoints { get; set; }

        [JSONField("pointsInLevel")]
        public int PointsInLevel { get; set; }

        [JSONField("pointsSinceLevel")]
        public int PointsSinceLevel { get; set; }
    }
}
