using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON.MatchHistory;

namespace Kappa.BackEnd.Server.Profile {
    [Docs("group", "Profile")]
    public class MatchHistoryService : JSONService {
        private Session session;

        [Endpoint("/new")]
        public event EventHandler<MatchDetails> NewMatch;

        public MatchHistoryService(Session session) : base("/profile/matches") {
            this.session = session;

            var messages = new MessageConsumer(session);
            messages.Consume<EndOfGameStats>(OnEndOfGameStats);
        }

        private bool OnEndOfGameStats(EndOfGameStats stats) {
            WaitForStats(stats);

            return true;
        }

        private async void WaitForStats(EndOfGameStats stats) {
            while (true) {
                try {
                    var details = await session.MatchHistoryService.GetMatchDetails(stats.ReportGameId);
                    Debug.WriteLine("Fetched stats: " + details.GameMode);
                    OnNewMatch(details);
                    return;
                } catch (Exception x) {
                    Debug.WriteLine(x);
                    await Task.Delay(500);
                }
            }
        }

        [Endpoint("/history")]
        public async Task<PlayerHistory> GetHistory(long account) {
            var thing = await session.MatchHistoryService.GetMatchHistory(account);
            return thing;
        }

        [Endpoint("/deltas")]
        public async Task<PlayerDeltas> GetDeltas() {
            var thing = await session.MatchHistoryService.GetDeltas();
            return thing;
        }

        [Endpoint("/details")]
        public async Task<MatchDetails> GetDetails(long matchId) {
            var thing = await session.MatchHistoryService.GetMatchDetails(matchId);
            return thing;
        }

        private void OnNewMatch(MatchDetails match) {
            NewMatch?.Invoke(this, match);
        }
    }
}
