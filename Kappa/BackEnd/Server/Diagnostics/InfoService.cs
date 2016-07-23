using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Diagnostics.Model;
using Kappa.BackEnd.Server.Patcher;
using MFroehlich.League.Assets;

namespace Kappa.BackEnd.Server.Diagnostics {
    public class InfoService : JSONService {
        private PatcherService patcher;
        private Session session;

        public InfoService(Session session, PatcherService patcher) : base("/info") {
            this.session = session;
            this.patcher = patcher;

        }

        [Endpoint("/versions")]
        public Versions Versions() {
            return new Versions {
                WAD = patcher.WADVersion,
                WADs = patcher.WADs,

                Game = Session.Latest.SolutionVersion.ToString(),
                Patch = DataDragon.CurrentVersion,
            };
        }
    }
}
