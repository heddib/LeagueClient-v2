using System;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    public class LobbyState : JSONSerializable {
        [JSONField("isCaptain")]
        public bool IsCaptain { get; set; }

        [JSONField("canInvite")]
        public bool CanInvite { get; set; }

        [JSONField("canMatch")]
        public bool CanMatch { get; set; }

        [JSONField("members")]
        public List<LobbyMember> Members { get; set; }

        [JSONField("me")]
        public LobbyMember Me { get; set; }

        [JSONField("chatroom")]
        public Guid Chatroom { get; set; }
    }
}
