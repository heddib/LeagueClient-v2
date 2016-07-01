using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class RerollState : JSONSerializable {
        [JSONField("cost")]
        public int Cost { get; set; }

        [JSONField("points")]
        public int Points { get; set; }

        [JSONField("maxPoints")]
        public int MaxPoints { get; set; }

        public RerollState(PointSummary points) {
            Cost = points.PointsCostToRoll;
            Points = points.CurrentPoints;
            MaxPoints = points.MaxRolls * Cost;
        }
    }
}