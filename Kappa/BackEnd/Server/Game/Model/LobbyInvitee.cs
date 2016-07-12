using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class LobbyInvitee {
        [JSONSkip]
        public Invitee Base { get; }

        public string State => Base.InviteeState;
        public string Name => Base.SummonerName;
        public long SummonerId => Base.SummonerId;

        internal LobbyInvitee(Invitee invitee) {
            this.Base = invitee;
        }
    }
}
