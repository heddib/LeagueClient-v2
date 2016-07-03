using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner.Model {
    [JSONSerializable]
    public class SummonerKudos  {
        public int Friendlies { get; set; }
        public int Helpfuls { get; set; }
        public int Teamworks { get; set; }
        public int Honorables { get; set; }
    }
}
