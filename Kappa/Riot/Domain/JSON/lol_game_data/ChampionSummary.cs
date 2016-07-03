using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class ChampionSummary : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("alias")]
        public string Alias { get; set; }

        [JSONField("roles")]
        public string[] Roles { get; set; }
    }
}
