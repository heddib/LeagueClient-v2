using MFroehlich.Parsing.JSON;
using System;
using agsXMPP;

namespace Kappa.BackEnd.Server.Chat.Model {
    public class MucFriend : JSONSerializable {
        [JSONField("room")]
        public Guid Room { get; }

        [JSONField("user")]
        public string User { get; }

        [JSONField("name")]
        public string Name { get; }

        public MucFriend(Guid room, Jid jid, string name) {
            Room = room;
            Name = name;
            User = jid.User;
        }
    }
}
