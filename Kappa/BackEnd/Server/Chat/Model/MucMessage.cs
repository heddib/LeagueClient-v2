using agsXMPP.protocol.client;
using MFroehlich.Parsing.JSON;
using System;

namespace Kappa.BackEnd.Server.Chat.Model {
    public class MucMessage : JSONSerializable {
        [JSONField("room")]
        public Guid Room { get; }
        [JSONField("from")]
        public string From { get; }
        [JSONField("body")]
        public string Content { get; }

        internal MucMessage(Guid room, Message msg) {
            Room = room;
            From = msg.From.Resource;
            Content = msg.Body;
        }
    }
}
