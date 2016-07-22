using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Chat;
using Kappa.BackEnd.Server.Game.Model;

namespace Kappa.BackEnd.Server.Game.Delegate {
    internal abstract class MatchmakingDelegate {
        public event EventHandler<MatchmakingState> StateChanged;
        public event EventHandler AdvancedToMatchmaking;

        protected MatchmakingState State { get; private set; }
        protected ChatRoomService Rooms { get; }
        protected MessageConsumer Messages { get; }

        protected MatchmakingDelegate(Session session, ChatRoomService rooms) {
            Rooms = rooms;
            Messages = new MessageConsumer(session);
        }

        public abstract Task CancelQueue();
        public abstract Task AcceptAfkCheck(bool accept);

        public void Reset() {
            if (State != null && State.Chatroom != Guid.Empty)
                Rooms.LeaveRoom(State.Chatroom);

            State = new MatchmakingState();
            OnStateChanged();
        }

        protected void OnStateChanged() {
            StateChanged?.Invoke(this, State);
        }

        protected void OnAdvancedToMatchmaking() {
            AdvancedToMatchmaking?.Invoke(this, EventArgs.Empty);
        }
    }
}
