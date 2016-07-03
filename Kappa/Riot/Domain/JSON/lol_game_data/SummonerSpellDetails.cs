using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class SummonerSpellDetails : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("description")]
        public string Description { get; set; }

        [JSONField("summonerLevel")]
        public int SummonerLevel { get; set; }

        [JSONField("gameModes")]
        public List<string> GameModes { get; set; }
    }
}
