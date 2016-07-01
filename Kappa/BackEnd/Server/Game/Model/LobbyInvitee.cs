using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.Interop.Game.Model {
    public class LobbyInvitee : RecordType, JSONValuable, IEquatable<LobbyInvitee> {
        public Invitee Base { get; }

        public string State => Base.InviteeState;
        public string Name => Base.SummonerName;
        public long SummonerId => Base.SummonerId;

        internal LobbyInvitee(Invitee invitee) {
            this.Base = invitee;
            CreateRecord();
        }

        public bool Equals(LobbyInvitee other) => base.Equals(other);

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["state"] = State,
                ["name"] = Name,
                ["id"] = SummonerId
            };
        }
    }
}
