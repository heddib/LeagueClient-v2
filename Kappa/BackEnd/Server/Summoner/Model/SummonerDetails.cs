using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner.Model {
    [JSONSerializable]
    public class SummonerDetails : SummonerSummary {
        public int Xp { get; set; }
        public int XpToLevel { get; set; }

        public SummonerDetails(AllPublicSummonerDataDTO all) : base(all.Summoner) {
            Level = all.SummonerLevel.Level;
        }
    }
}
