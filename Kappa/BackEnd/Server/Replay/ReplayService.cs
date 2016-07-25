using System.Collections.Generic;
using System.IO;
using Kappa.Riot.Domain;

namespace Kappa.BackEnd.Server.Replay {
    [Docs("group", "Replay")]
    public class ReplayService : JSONService {
        private List<long> saved = new List<long>();
        private string directory;

        private Session session;
        private SpectatorService spectator;

        public ReplayService(Session session) : base("/replays") {
            this.session = session;
            directory = GetStorage("replays");

            Directory.CreateDirectory(directory);
            foreach (var file in Directory.EnumerateFiles(directory, "*.lol")) {
                long gameid;
                if (!long.TryParse(Path.GetFileNameWithoutExtension(file), out gameid)) continue;
                saved.Add(gameid);
            }

            var messages = new MessageConsumer(session);
            messages.Consume<PlayerCredentialsDto>(OnPlayerCredentials);

            BackEndServer.AddService(spectator = new SpectatorService(session, LoadReplay));
        }

        private Replay LoadReplay(long gameid) {
            var file = Path.Combine(directory, gameid + ".lol");
            return new Replay(file);
        }

        private bool OnPlayerCredentials(PlayerCredentialsDto creds) {
            return false;
        }

        [Endpoint("/saved")]
        public List<long> GetSaved() {
            return saved;
        }

        [Endpoint("/replay")]
        public void Replay(long id) {
            if (!saved.Contains(id)) throw new FileNotFoundException($"Replay of game {id} not found");

            spectator.Replay(id);
        }
    }
}
