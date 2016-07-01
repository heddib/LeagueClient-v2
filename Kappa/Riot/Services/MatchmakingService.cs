using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class MatchmakingService : Service {
        protected override string Destination => "matchmakerService";

        public MatchmakingService(Session session) : base(session) { }

        public Task<bool> IsMatchmakingEnabled() {
            return InvokeAsync<bool>("isMatchmakingEnabled");
        }

        /// <summary>
        /// Attemps to leave a queue
        /// </summary>
        /// <param name="queueId">The queue to cancel from</param>
        /// <returns>If successfully cancelled returns true, otherwise champion select about to start</returns>
        public Task<bool> CancelFromQueueIfPossible(int queueId) {
            return InvokeAsync<bool>("cancelFromQueueIfPossible", queueId);
        }

        /// <summary>
        /// Gets the queue information for a selected queue
        /// </summary>
        /// <param name="queueId">The queue id</param>
        /// <returns>Returns the queue information</returns>
        public Task<QueueInfo> GetQueueInformation(double queueId) {
            return InvokeAsync<QueueInfo>("getQueueInfo", queueId);
        }

        public Task<object> PurgeFromQueues() {
            return InvokeAsync<object>("purgeFromQueues");
        }

        /// <summary>
        /// Attaches to a queue
        /// </summary>
        /// <param name="matchMakerParams">The parameters for the queue</param>
        /// <returns>Returns a notification to tell you if it was successful</returns>
        public Task<SearchingForMatchNotification> AttachToQueue(MatchMakerParams matchMakerParams) {
            return InvokeAsync<SearchingForMatchNotification>("attachToQueue", matchMakerParams);
        }

        /// <summary>
        /// Attaches a premade team to a queue
        /// </summary>
        /// <param name="matchMakerParams">The parameters for the queue</param>
        /// <returns>Returns a notification to tell you if it was successful</returns>
        public Task<SearchingForMatchNotification> AttachTeamToQueue(MatchMakerParams matchMakerParams) {
            return InvokeAsync<SearchingForMatchNotification>("attachTeamToQueue", matchMakerParams);
        }

        /// <summary>
        /// Get the queues that are currently enabled.
        /// </summary>
        /// <returns>Returns an array of queues that are enabled</returns>
        public Task<GameQueueConfig[]> GetAvailableQueues() {
            return InvokeAsync<GameQueueConfig[]>("getAvailableQueues");
        }
    }
}
