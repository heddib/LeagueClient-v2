using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class GameStatistics : JSONSerializable {

        public GameStatistics(EndOfGameStats stats, MatchDetails result) {
            EarnedIP = stats.IpEarned;
            TotalIP = stats.IpTotal;
            TimeUntilFirstWin = stats.TimeUntilNextFirstWinBonus;
        }

        [JSONField("ipTotal")]
        public int TotalIP { get; set; }

        [JSONField("ipEarned")]
        public int EarnedIP { get; set; }

        [JSONField("timeUntilFirstWin")]
        public long TimeUntilFirstWin { get; set; }

        [JSONField("room")]
        public Guid Room { get; set; }

        [JSONField("blueTeam")]
        public List<PlayerStatistics> BlueTeam { get; set; }
    }

    public class PlayerStatistics : JSONSerializable {
        //public 
    }
}
