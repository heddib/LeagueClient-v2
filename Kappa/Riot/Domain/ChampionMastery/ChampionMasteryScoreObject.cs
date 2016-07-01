using Kappa.Riot.Services.Lcds;

namespace Kappa.Riot.Domain.ChampionMastery {
    [LcdsService(LcdsServiceNames.ChampionMastery, "getChampionMasteryScore")]
    public class ChampionMasteryScoreObject : LcdsServiceObject {
        public int Score { get; }
        public ChampionMasteryScoreObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Score = (int) payload;
        }
    }
}
