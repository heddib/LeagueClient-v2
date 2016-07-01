using System;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    public class CustomState : JSONSerializable {
        [JSONField("blueTeam")]
        public List<LobbyMember> BlueTeam { get; set; } = new List<LobbyMember>();

        [JSONField("redTeam")]
        public List<LobbyMember> RedTeam { get; set; } = new List<LobbyMember>();

        [JSONField("owner")]
        public LobbyMember Owner { get; set; }

        [JSONField("me")]
        public LobbyMember Me { get; set; }

        [JSONField("chatroom")]
        public Guid Chatroom { get; set; }
    }
}
