using agsXMPP.protocol.client;
using MFroehlich.Parsing.JSON;
using System;

namespace Kappa.BackEnd.Server.Chat.Model {
    [JSONSerializable]
    public class MucMessage {
        public Guid Room { get; }
        public string From { get; }
        public string Body { get; }

        internal MucMessage(Guid room, Message msg) {
            Room = room;
            From = msg.From.Resource;
            Body = msg.Body;
        }
    }
}
