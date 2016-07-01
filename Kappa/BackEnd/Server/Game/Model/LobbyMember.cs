using Kappa.Riot.Domain;
using Kappa.Riot.Domain.TeambuilderDraft;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class LobbyMember : JSONSerializable {
        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("id")]
        public object Id { get; set; }

        [JSONField("champ")]
        public int Champion { get; set; }
        [JSONField("role1")]
        public Position? Role1 { get; set; }

        [JSONField("role2")]
        public Position? Role2 { get; set; }

        private LobbyMember(object id) {
            Id = id;
        }

        public LobbyMember(SlotData slot) : this(slot.SlotId) {
            Name = slot.SummonerName;
            Role1 = slot.Roles[0];
            Role2 = slot.Roles[1];
        }

        public LobbyMember(Riot.Domain.Member m) : this(m.SummonerId) {
            Name = m.SummonerName;
        }

        public LobbyMember(GameParticipant m) : this(m.SummonerInternalName) {
            Name = m.SummonerName;
            var bot = m as BotParticipant;
            if (bot != null)
                Champion = bot.Champion.ChampionId;
        }
    }
}
