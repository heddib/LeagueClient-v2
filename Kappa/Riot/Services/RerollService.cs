using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class RerollService : Service {
        protected override string Destination => "lcdsRerollService";

        public RerollService(Session session) : base(session) { }

        /// <summary>
        /// Gets the player reroll balance
        /// </summary>
        /// <returns>Returns the reroll balance for the player</returns>
        public Task<PointSummary> GetPointsBalance() {
            return InvokeAsync<PointSummary>("getPointsBalance");
        }

        /// <summary>
        /// Attempts to reroll the champion. Only works in AllRandomPickStrategy
        /// </summary>
        /// <returns>Returns the amount of rolls left for the player</returns>
        public Task<RollResult> Roll() {
            return InvokeAsync<RollResult>("roll");
        }
    }
}
