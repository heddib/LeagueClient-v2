using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using agsXMPP.protocol.client;
using Kappa.BackEnd.Server.Summoner.Model;

namespace Kappa.BackEnd.Server.Chat {
    public class Status {
        public enum StatusType {
            CHAT,
            DND,
            AWAY,
            MOBILE
        }

        public int ProfileIcon { get; private set; }
        public long TimeStamp { get; private set; }
        public string Message { get; private set; }
        public string Champion { get; private set; }

        public string Raw { get; private set; }

        public GameStatus GameStatus { get; private set; }
        public StatusType Show { get; private set; }

        public DateTime? LastOnline { get; private set; }

        public Status(string message, GameStatus ingame) {
            Message = message;
            GameStatus = ingame;
        }

        public Status(Presence xml) {
            var pres = new XmlDocument();
            pres.LoadXml(xml.ToString());
            foreach (XmlElement child in pres.DocumentElement?.ChildNodes) {
                switch (child.Name) {
                case "status":
                    ParseStatus(child.InnerText, this);
                    Raw = child.InnerText;
                    break;
                case "show":
                    StatusType type;
                    if (!Enum.TryParse(child.InnerText, true, out type))
                        throw new FormatException("Show: " + child.InnerText);
                    Show = type;
                    break;
                case "last_online":
                    DateTime time;
                    if (!DateTime.TryParse(child.InnerText, out time))
                        throw new FormatException("LastOnline: " + child.InnerText);
                    LastOnline = time;
                    break;
                }
            }
        }

        public string ToXML(Me me = null) {
            //var unranked = Session.Current.Account.LoginPacket.PlayerStatSummaries.PlayerStatSummarySet.FirstOrDefault(x => x.PlayerStatSummaryTypeString.Equals("Unranked"));
            //var ranked = Session.Current.Account.LoginPacket.PlayerStatSummaries.PlayerStatSummarySet.FirstOrDefault(x => x.PlayerStatSummaryTypeString.Equals("RankedSolo5x5"));
            //var league = Session.Current.Leagues.SummonerLeagues.FirstOrDefault(l => l.Queue.Equals(QueueType.RANKED_SOLO_5x5.Key));

            var dict = new Dictionary<string, object>();
            if (me != null) {
                dict["profileIcon"] = me.ProfileIcon;
                dict["level"] = me.Level;
            }
            else {
                dict["profileIcon"] = 2;
                dict["level"] = 30;
            }
            dict["gameStatus"] = GameStatus.Key;

            if (!string.IsNullOrEmpty(Message)) dict["statusMsg"] = Message;

            //if (ranked != null && league != null) {
            //  dict["rankedLeagueName"] = league.DivisionName;
            //  dict["rankedLeagueDivision"] = league.Rank;
            //  dict["rankedLeagueTier"] = league.Tier;
            //  dict["rankedLeagueQueue"] = league.Queue;
            //  dict["rankedWins"] = ranked.Wins;
            //}

            var xml = new XElement("body", dict.Select(pair => new XElement(pair.Key, pair.Value)));
            return xml.ToString();
        }

        private static void ParseStatus(string inner, Status status) {
            var doc = new XmlDocument();
            try {
                doc.LoadXml(inner);
            } catch (XmlException) {
                inner = HttpUtility.HtmlDecode(inner);
                doc.LoadXml(inner);
            }

            foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
                switch (node.Name) {
                case "profileIcon":
                    status.ProfileIcon = int.Parse(node.InnerText);
                    break;
                case "statusMsg":
                    status.Message = node.InnerText;
                    break;
                case "gameStatus":
                    status.GameStatus = GameStatus.Values[node.InnerText];
                    break;
                case "timeStamp":
                    status.TimeStamp = long.Parse(node.InnerText);
                    break;
                case "skinname":
                    status.Champion = node.InnerText;
                    break;
                }
            }
        }
    }
}
