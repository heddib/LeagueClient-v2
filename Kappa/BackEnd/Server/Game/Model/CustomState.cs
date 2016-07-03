using System;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class CustomState {
        public List<LobbyMember> BlueTeam { get; set; } = new List<LobbyMember>();
        public List<LobbyMember> RedTeam { get; set; } = new List<LobbyMember>();

        public LobbyMember Owner { get; set; }
        public LobbyMember Me { get; set; }

        public Guid Chatroom { get; set; }
    }
}
