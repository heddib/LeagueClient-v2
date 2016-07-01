using agsXMPP.protocol.iq.roster;
using System;
using System.Collections.Generic;
using agsXMPP.protocol.client;
using MFroehlich.Parsing.JSON;
using static MFroehlich.League.RiotAPI.CurrentGameAPI;
using agsXMPP;

namespace Kappa.BackEnd.Server.Chat.Model {
    public class Friend : IComparable<Friend>, JSONValuable {
        public Jid Jid { get; }
        public string User { get; }

        public string Name { get; }
        public Status Status { get; private set; }
        public CurrentGameInfo CurrentGame { get; private set; }

        public List<ChatMessage> Messages { get; } = new List<ChatMessage>();

        public bool IsOnline => Status != null;
        public bool IsMobile => IsOnline && Status.Show == Status.StatusType.MOBILE;
        public bool IsInGame => IsOnline && Status.GameStatus == GameStatus.inGame;

        internal Friend(RosterItem item) {
            Jid = item.Jid;
            User = Jid.User;
            Name = item.Attribute("name");
        }

        public int CompareTo(Friend other) {
            if (IsOnline && other.IsOnline) {
                if (Status.Show != other.Status.Show)
                    return Status.Show.CompareTo(other.Status.Show);

                if (Status.GameStatus != other.Status.GameStatus)
                    if (Status.GameStatus == null)
                        return 1;
                    else if (other.Status.GameStatus == null)
                        return -1;
                    else
                        return Status.GameStatus.CompareTo(other.Status.GameStatus);

                if (IsInGame && other.IsInGame)
                    return (CurrentGame?.gameStartTime ?? Status.TimeStamp).CompareTo
                           (other.CurrentGame?.gameStartTime ?? other.Status.TimeStamp);

                if (IsMobile && other.IsMobile && Status.LastOnline != null && other.Status.LastOnline != null)
                    return -Status.LastOnline.Value.CompareTo(other.Status.LastOnline.Value);
            } else if (IsOnline != other.IsOnline) {
                return IsOnline.CompareTo(other.IsOnline);
            }

            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        JSONValue JSONValuable.ToJSON() {
            var json = new JSONObject() {
                ["user"] = User,
                ["name"] = Name,
                ["id"] = ChatUtils.GetSummonerId(User)
            };

            if (IsOnline) {
                json["show"] = Status.Show;
                json["message"] = Status.Message;
                if (Status.GameStatus != null)
                    json["status"] = new JSONObject {
                        ["display"] = Status.GameStatus.Display,
                        ["id"] = Status.GameStatus.Key
                    };
            }

            if (IsMobile && Status.LastOnline.HasValue) {
                json["lastonline"] = Status.LastOnline.Value;
            }

            if (IsInGame) {
                json["game"] = new JSONObject {
                    ["start"] = CurrentGame?.gameStartTime ?? Status.TimeStamp,
                    ["exact"] = CurrentGame != null,
                    ["type"] = CurrentGame?.gameQueueConfigId ?? -1
                };
            }

            return json;
        }

        internal void OnPresence(Presence pres) {
            Status = pres.Type == PresenceType.unavailable ? null : new Status(pres);
        }

        internal void OnGame(CurrentGameInfo game) {
            this.CurrentGame = game;
        }
    }
}
