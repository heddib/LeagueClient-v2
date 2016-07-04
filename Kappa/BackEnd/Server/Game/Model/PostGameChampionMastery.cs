using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class PostGameChampionMastery {
        public ChampionMasteryState Before { get; set; }
        public ChampionMasteryState After { get; set; }

        public string Grade { get; set; }
        public int Champion { get; set; }
    }

    [JSONSerializable]
    public class ChampionMasteryState {
        public int Level { get; set; }

        public int TotalPoints { get; set; }
        public int PointsInLevel { get; set; }
        public int PointsSinceLevel { get; set; }
    }
}
