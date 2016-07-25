using System.Diagnostics;

namespace Kappa.BackEnd.Server.Meta {
    [Docs("group", "Meta")]
    public class MetaService : JSONService {
        private Session session;
        public MetaService(Session session) : base("/meta") {
            this.session = session;
        }

        [Endpoint("/close")]
        public void Close() {
            session.Exit();
        }

        [Endpoint("/link")]
        public void OpenLink(string link) {
            Process.Start(link);
        }
    }
}
