using System;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class LobbyState {
        public bool IsCaptain { get; set; }

        public bool CanInvite { get; set; }
        public bool CanMatch { get; set; }

        public List<LobbyMember> Members { get; set; }
        public LobbyMember Me { get; set; }

        public Guid Chatroom { get; set; }
    }
}
