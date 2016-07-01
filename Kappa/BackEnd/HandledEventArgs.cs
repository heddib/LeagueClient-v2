using System;

namespace Kappa.BackEnd {
    public class HandledEventArgs<T> : EventArgs {
        public T Args { get; }
        public bool Handled { get; private set; }

        public HandledEventArgs(T t) {
            Args = t;
            Handled = false;
        }

        public void Handle() {
            Handled = true;
        }
    }
}
