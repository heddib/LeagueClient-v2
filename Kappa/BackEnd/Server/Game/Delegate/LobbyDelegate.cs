using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP.Xml.Xpnet;
using Kappa.BackEnd.Server.Chat;
using Kappa.BackEnd.Server.Game.Model;

namespace Kappa.BackEnd.Server.Game.Delegate {
    internal abstract class LobbyDelegate {
        public event EventHandler<LobbyState> StateChanged;
        public event EventHandler AdvancedToMatchmaking;

        protected LobbyState state { get; private set; }
        protected ChatRoomService Rooms { get; }
        protected MessageConsumer Messages { get; }

        protected LobbyDelegate(Session session, ChatRoomService rooms) {
            Rooms = rooms;
            Messages = new MessageConsumer(session);
        }

        public abstract Task StartQueue();

        public void Reset() {
            if (state != null && state.Chatroom != Guid.Empty)
                Rooms.LeaveRoom(state.Chatroom);

            state = new LobbyState();
            OnStateChanged();
        }

        protected void OnStateChanged() {
            StateChanged?.Invoke(this, state);
        }

        protected void OnAdvancedToMatchmaking() {
            AdvancedToMatchmaking?.Invoke(this, EventArgs.Empty);
        }
    }
}
