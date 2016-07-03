using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class RerollState  {
        public int Cost { get; set; }
        public int Points { get; set; }
        public int MaxPoints { get; set; }

        public RerollState(PointSummary points) {
            Cost = points.PointsCostToRoll;
            Points = points.CurrentPoints;
            MaxPoints = points.MaxRolls * Cost;
        }
    }
}