using System;
using System.Collections.Generic;
using System.Linq;

namespace Kappa.BackEnd {
    public class MessageConsumer : IDisposable {
        private Session kappa;
        private readonly Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        public MessageConsumer(Session kappa) {
            this.kappa = kappa;
            this.kappa.InternalMessageReceived += OnMessage;
        }

        public void Consume<T>(Func<T, bool> handler) {
            handlers.Add(typeof(T), handler);
        }

        public bool HandleMessage(object message) {
            Delegate handler;
            var arg = message;

            if (!handlers.TryGetValue(message.GetType(), out handler)) {
                handler = handlers.LastOrDefault(pair => pair.Key.IsInstanceOfType(message)).Value;
            }

            return handler != null && (bool) handler.DynamicInvoke(arg);
        }

        private void OnMessage(object sender, HandledEventArgs<object> args) {
            if (HandleMessage(args.Args))
                args.Handle();
        }

        #region IDisposable Support

        private bool isDisposed;
        protected virtual void Dispose(bool disposing) {
            if (isDisposed) return;

            if (disposing) {
                kappa.InternalMessageReceived -= OnMessage;
            }

            isDisposed = true;
        }

        public void Dispose() {
            Dispose(true);
        }

        #endregion
    }
}
