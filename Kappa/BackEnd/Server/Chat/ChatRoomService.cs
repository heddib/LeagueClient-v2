using agsXMPP;
using agsXMPP.protocol.client;
using Kappa.BackEnd.Server.Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.x.muc;
using Kappa.Riot.Domain.TeambuilderDraft;

namespace Kappa.BackEnd.Server.Chat {
    [Docs("group", "Chat")]
    public class ChatRoomService : JSONService {
        private ChatService chat;
        private MucManager muc;
        private Session session;

        private Dictionary<Guid, Jid> rooms = new Dictionary<Guid, Jid>();
        private Dictionary<string, Guid> roomsReverse = new Dictionary<string, Guid>();
        private Dictionary<Guid, HashSet<string>> users = new Dictionary<Guid, HashSet<string>>();

        [Async("/memberJoin")]
        public event EventHandler<MucFriend> MemberJoin;

        [Async("/memberLeave")]
        public event EventHandler<MucFriend> MemberLeave;

        [Async("/message")]
        public event EventHandler<MucMessage> Message;

        public ChatRoomService(Session session, ChatService chat) : base("/chat/rooms") {
            this.session = session;
            this.chat = chat;
            this.muc = new MucManager(chat.XMPP);

            chat.MessageRaw += Chat_MessageRaw;
            chat.PresenceRaw += Chat_PresenceRaw;
        }

        internal void LeaveRoom(Guid roomId) {
            Jid jid;
            if (!rooms.TryGetValue(roomId, out jid)) return;

            muc.LeaveRoom(jid, session.Me.Name);
            rooms.Remove(roomId);
            users.Remove(roomId);
            roomsReverse.Remove(jid.User);
        }

        internal Guid JoinStandard(Riot.Domain.LobbyStatus status) {
            var jid = ChatUtils.GetChatroomJID(status.InvitationId, "ag", false, status.ChatKey);
            return JoinRoom(jid);
        }

        internal Guid JoinDraft(PremadeState state) {
            var jid = ChatUtils.GetChatroomJID(state.PremadeId, "cp", false);
            return JoinRoom(jid);
        }

        internal Guid JoinDraft(ChampSelectState state) {
            var jid = new Jid(state.TeamId + "@" + "sec.pvp.net");
            return JoinRoom(jid);
        }

        internal Guid JoinCustom(Riot.Domain.GameDTO game) {
            var jid = ChatUtils.GetChatroomJID(game.Name + game.Id, "ap", false, game.RoomPassword);
            return JoinRoom(jid);
        }

        internal Guid JoinChampSelect(Riot.Domain.GameDTO game) {
            Jid jid;
            if (string.IsNullOrEmpty(game.RoomName)) {
                bool blue = game.TeamOne.Any(p => p.SummonerInternalName == session.Me.InternalName);
                var prefix = blue ? "c1" : "c2";
                jid = ChatUtils.GetChatroomJID(game.Name + "_" + prefix, prefix, false, game.RoomPassword);
            } else {
                jid = new Jid(game.RoomName.ToLower() + ".pvp.net");
            }
            return JoinRoom(jid);
        }

        private Guid JoinRoom(Jid jid) {
            muc.AcceptDefaultConfiguration(jid);
            muc.JoinRoom(jid, session.Me.Name, false);
            var id = Guid.NewGuid();
            roomsReverse[jid.User] = id;
            rooms[id] = jid;
            users[id] = new HashSet<string>();
            return id;
        }

        [Endpoint("/message")]
        public void SendMessage(Guid room, string body) {
            Jid jid;
            if (rooms.TryGetValue(room, out jid)) {
                var msg = new Message(jid, MessageType.groupchat, body);
                chat.SendRaw(msg);
            } else throw new KeyNotFoundException("Room not found");
        }

        private void Chat_PresenceRaw(object sender, Presence e) {
            Guid roomId;
            if (!roomsReverse.TryGetValue(e.From.User, out roomId)) return;

            var jid = e.MucUser.Item.Jid;
            var list = users[roomId];
            var friend = new MucFriend(roomId, jid, e.From.Resource);

            bool offline = e.Type == PresenceType.unavailable;

            if (!offline && !list.Contains(jid.User)) {
                list.Add(jid.User);
                MemberJoin?.Invoke(this, friend);
            }

            if (offline && list.Contains(jid.User)) {
                list.Remove(jid.User);
                MemberLeave?.Invoke(this, friend);
            }
        }

        private void Chat_MessageRaw(object sender, Message raw) {
            Guid roomId;
            if (!roomsReverse.TryGetValue(raw.From.User, out roomId)) return;

            var message = new MucMessage(roomId, raw);
            Message?.Invoke(this, message);
        }
    }
}
