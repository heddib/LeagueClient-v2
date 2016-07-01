using System;
using agsXMPP.protocol.client;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Chat.Model {
    public class ChatMessage : JSONSerializable {
        [JSONField("user")]
        public string User { get; }

        [JSONField("received")]
        public bool Received { get; }

        [JSONField("body")]
        public string Content { get; }

        [JSONField("date")]
        public DateTime Date { get; }

        [JSONField("archived")]
        public bool Archived { get; }

        internal ChatMessage(string user, Message msg, bool archived) {
            User = user;
            Received = msg.From.User == user;
            Content = msg.Body;
            Archived = archived;

            Date = msg.HasAttribute("stamp") ? DateTime.Parse(msg.GetAttribute("stamp")) : DateTime.Now;
        }

        internal ChatMessage(string user, string body) {
            User = user;
            Received = false;
            Content = body;
            Date = DateTime.Now;
        }
    }
}