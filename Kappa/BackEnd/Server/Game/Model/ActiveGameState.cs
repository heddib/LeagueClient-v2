using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class ActiveGameState : JSONSerializable {
        [JSONField("ingame")]
        public bool InGame { get; set; }

        [JSONField("launched")]
        public bool Launched { get; set; }

        [JSONField("stats")]
        public EndOfGameStats Stats { get; set; }

        [JSONField("championmastery")]
        public EndOfGameChampionMastery ChampionMastery { get; set; }
    }
}
