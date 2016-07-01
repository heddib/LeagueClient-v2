using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.Riot.Domain.JSON;

namespace Kappa.BackEnd.Server.Profile {
    [Docs("group", "Profile")]
    public class MatchHistoryService : JSONService {
        private Session session;

        public MatchHistoryService(Session session) : base("/profile/matches") {
            this.session = session;
        }

        [Endpoint("/history")]
        public async Task<PlayerHistory> GetHistory() {
            var thing = await session.MatchHistoryService.GetMatchHistory(session.Me.AccountId);
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
    }
}
