using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner.Model {
    [JSONSerializable]
    public class SummonerSummary {
        public string InternalName { get; set; }
        public long SummonerId { get; set; }
        public long AccountId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
        public int Icon { get; set; }

        public SummonerSummary(PublicSummoner pub) {
            InternalName = pub.InternalName;
            SummonerId = pub.SummonerId;
            AccountId = pub.AcctId;

            Name = pub.Name;

            Level = pub.SummonerLevel;
            Icon = pub.ProfileIconId;
        }
    }
}
