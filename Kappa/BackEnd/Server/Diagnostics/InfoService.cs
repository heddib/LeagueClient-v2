using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Diagnostics.Model;
using MFroehlich.League.Assets;

namespace Kappa.BackEnd.Server.Diagnostics {
    public class InfoService : JSONService {
        private Session session;

        public InfoService(Session session) : base("/info") {
            this.session = session;
        }

        [Endpoint("/versions")]
        public Versions Versions() {
            return new Versions {
                Game = Session.Latest.SolutionVersion.ToString(),
                Patch = DataDragon.CurrentVersion
            };
        }
    }
}
