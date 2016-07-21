using agsXMPP;
using agsXMPP.protocol.client;
using Kappa.BackEnd.Server.Authentication.Model;
using Kappa.BackEnd.Server.Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using agsXMPP.Xml.Dom;
using LeagueSharp;
using static MFroehlich.League.RiotAPI.CurrentGameAPI;
using RosterItem = agsXMPP.protocol.iq.roster.RosterItem;

namespace Kappa.BackEnd.Server.Chat {
    [Docs("group", "Chat")]
    public class ChatService : JSONService {
        private Dictionary<string, Friend> friends = new Dictionary<string, Friend>();
        private HashSet<string> fetchedHistories = new HashSet<string>();

        private XmppClientConnection xmpp;

        private Status status;
        private Session session;

        [Async("/message")]
        public event EventHandler<ChatMessage> Message;

        [Async("/status")]
        public event EventHandler<Friend> Status;

        internal IEnumerable<Friend> Friends => friends.Values;

        internal event EventHandler<Presence> PresenceRaw;
        internal event EventHandler<Message> MessageRaw;
        internal XmppClientConnection XMPP => xmpp;

        public ChatService(Session session, GameStatus status = null, string msg = null) : base("/chat") {
            this.session = session;
            this.status = new Status(msg ?? "", status ?? GameStatus.outOfGame);

            xmpp = new XmppClientConnection {
                Server = "pvp.net",
                Port = 5223,
                ConnectServer = Region.Current.ChatServer,
                AutoResolveConnectServer = false,
                Resource = "xiff",
                UseSSL = true,
                KeepAliveInterval = 10,
                KeepAlive = true,
                UseCompression = true,
                AutoPresence = true,
                Status = this.status.ToXML(),
                Show = ShowType.chat,
                Priority = 0,
            };

            xmpp.ClientSocket.ConnectTimeout = 1000;
            xmpp.OnMessage += Xmpp_OnMessage;
            xmpp.OnError += (o, e) => Session.Log(e);

            xmpp.OnRosterItem += Xmpp_OnRosterItem;
            xmpp.OnPresence += Xmpp_OnPresence;
        }

        public void Connect(AuthResult auth) {
            xmpp.OnSocketError += (s, e) => xmpp.Open(auth.Username, "AIR_" + auth.Password);
            xmpp.Open(auth.Username, "AIR_" + auth.Password);
        }

        [Endpoint("/message")]
        public void SendMessage(string to, string body) {
            var friend = friends[to];
            xmpp.Send(new Message(friend.Jid, MessageType.chat, body));
            OnMessage(new ChatMessage(to, body));
        }

        internal void SendRaw(Message msg) {
            xmpp.Send(msg);
        }

        internal void SendPresence() {
            xmpp.Send(new Presence(ShowType.chat, status.ToXML()) { Type = PresenceType.available });
        }

        private void Xmpp_OnRosterItem(object sender, RosterItem item) {
            var user = item.Jid.User;
            var friend = new Friend(item);
            friends.Add(user, friend);
        }

        private void Xmpp_OnPresence(object sender, Presence pres) {
            var user = pres.From.User;
            Friend friend;

            //Wack to fix chatroom presences
            if (pres.Status == null && pres.MucUser != null && ChatUtils.GetSummonerId(pres.MucUser.Item.Jid.User) == session.Me.SummonerId) {
                SendPresence();
            }
            else if (friends.TryGetValue(user, out friend)) {
                friend.OnPresence(pres);
                OnStatus(friend);

                if (friend.Status?.GameStatus == GameStatus.inGame) {
                    GetInGameData(friend);
                }

                if (!fetchedHistories.Contains(user)) {
                    fetchedHistories.Add(user);
                    var iq = new IQ(IqType.get, xmpp.MyJID, null) { Id = "arch_" + Guid.NewGuid() };
                    var query = new Element("query") { Namespace = "jabber:iq:riotgames:archive" };
                    query.AddChild(new Element("with", pres.From.Bare));
                    iq.AddChild(query);

                    xmpp.IqGrabber.SendIq(iq, async (s, result, d) => {
                        await Task.Delay(5000);
                        //TODO Fix this disgusting hack
                        var history = result.SelectElements<Message>();
                        foreach (var msg in history.OrderBy(m => DateTime.Parse(m.GetAttribute("stamp")))) {
                            OnMessage(new ChatMessage(friend.User, msg, true));
                        }
                    });
                }
            }
            else {
                PresenceRaw?.Invoke(this, pres);
            }
        }

        private void Xmpp_OnMessage(object sender, Message msg) {
            Friend friend;
            if (friends.TryGetValue(msg.From.User, out friend))
                OnMessage(new ChatMessage(msg.From.User, msg, false));
            else {
                MessageRaw?.Invoke(this, msg);
            }
        }

        private void OnStatus(Friend friend) {
            Status?.Invoke(this, friend);
        }

        private void OnMessage(ChatMessage msg) {
            Message?.Invoke(this, msg);
        }

        private async void GetInGameData(Friend friend) {
            long summonerId = ChatUtils.GetSummonerId(friend.User);
            CurrentGameInfo game = null;
            while (friend.Status?.GameStatus == GameStatus.inGame) {
                try {
                    game = await Session.RiotAPI.CurrentGameAPI.BySummoner(summonerId);
                } catch {
                    // ignored
                }

                if (game?.gameMode != null) {
                    friend.OnGame(game);
                    OnStatus(friend);
                }

                if (game != null && game.gameStartTime > 0)
                    break;

                await Task.Delay(20 * 1000);
            }
        }
    }
}
