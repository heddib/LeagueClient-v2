using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.BackEnd.Server.Game.Delegate {
    internal abstract class PlayLoopDelegate {
        public void Quit() {
            Abandon();
        }
        public abstract Task Abandon();
    }
}
