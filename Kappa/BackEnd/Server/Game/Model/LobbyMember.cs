using Kappa.Riot.Domain;
using Kappa.Riot.Domain.TeambuilderDraft;
using MFroehlich.League.Assets;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class LobbyMember {
        public string Name { get; set; }

        public object Id { get; set; }

        public int Champ { get; set; }

        public Position? Role1 { get; set; }
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
            if (m is BotParticipant) {
                var split = m.SummonerInternalName.Split('_');
                var name = split[1];
                Champ = DataDragon.ChampData.Value.data[name].key;
            }
        }
    }
}
