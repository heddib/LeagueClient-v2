using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner.Model {
    public class SummonerKudos : JSONSerializable {
        [JSONField("friendlies")]
        public int Friendlies { get; set; }

        [JSONField("helpfuls")]
        public int Helpfuls { get; set; }

        [JSONField("teamworks")]
        public int Teamworks { get; set; }

        [JSONField("honorables")]
        public int Honorables { get; set; }
    }
}
