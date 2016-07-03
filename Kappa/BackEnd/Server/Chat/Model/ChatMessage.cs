using System;
using agsXMPP.protocol.client;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Chat.Model {
    [JSONSerializable]
    public class ChatMessage {
        public string User { get; }
        public string Body { get; }

        public bool Received { get; }
        public DateTime Date { get; }
        public bool Archived { get; }

        internal ChatMessage(string user, Message msg, bool archived) {
            User = user;
            Received = msg.From.User == user;
            Body = msg.Body;
            Archived = archived;

            Date = msg.HasAttribute("stamp") ? DateTime.Parse(msg.GetAttribute("stamp")) : DateTime.Now;
        }

        internal ChatMessage(string user, string body) {
            User = user;
            Received = false;
            Body = body;
            Date = DateTime.Now;
        }
    }
}