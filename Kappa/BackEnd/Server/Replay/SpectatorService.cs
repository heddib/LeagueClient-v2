using System.Collections.Generic;
using System.IO;
using System.Net;
using MFroehlich.Parsing;

namespace Kappa.BackEnd.Server.Replay {
    public class SpectatorService : HttpService {
        public delegate Replay ReplayLoader(long gameId);

        private Dictionary<long, Replay> replays = new Dictionary<long, Replay>();
        private ReplayLoader loader;
        private Session session;

        public SpectatorService(Session session, ReplayLoader loader) : base("/observer-mode/rest/consumer") {
            this.session = session;
            this.loader = loader;

            //Replay(2232035041);
        }

        private Replay GetReplay(long id) {
            Replay replay;
            if (!replays.TryGetValue(id, out replay))
                replays.Add(id, replay = loader(id));
            return replay;
        }

        internal void Replay(long id) {
            var replay = GetReplay(id);
            session.Maestro.ReplayGame("localhost", BackEndServer.HttpPort, replay.MetaData.EncryptionKey, replay.MetaData.GameKey.GameId);
        }

        public override bool Handle(HttpListenerContext context) {
            using (var stream = context.Response.OutputStream) {
                var target = context.Request.Url.AbsolutePath;

                context.Response.ContentType = "text/plain";
                var path = target.Split('/');

                Replay replay = null;
                long gameId;
                if (path.Length > 6 && long.TryParse(path[6], out gameId)) {
                    replay = GetReplay(gameId);
                }

                switch (path[4]) {
                case "version":
                    stream.Write("1.82.81".GetBytes());
                    return true;
                case "getGameMetaData":
                    stream.Write(replay.RawMetaData);
                    return true;
                case "getLastChunkInfo":
                    var info = new Replay.ChunkInfo {
                        Duration = 30000,
                        AvailableSince = 30000,
                        NextAvailableChunk = 0,
                        EndStartupChunkId = replay.MetaData.EndStartupChunkId,
                        StartGameChunkId = replay.MetaData.StartGameChunkId,
                        EndGameChunkId = replay.MetaData.EndGameChunkId,
                        NextChunkId = replay.MetaData.EndGameChunkId,
                        ChunkId = replay.MetaData.EndGameChunkId,
                        KeyFrameId = replay.MetaData.EndGameKeyFrameId
                    };

                    stream.Write(info.Serialize().ToJSON().GetBytes());
                    return true;
                case "getGameDataChunk":
                    var chunk = replay.Chunks[int.Parse(path[7]) - 1];
                    using (var input = replay.Open()) {
                        input.Seek(chunk.Offset, SeekOrigin.Begin);
                        input.CopyToLength(stream, chunk.Length);
                    }
                    return true;
                case "getKeyFrame":
                    var frame = replay.Chunks[int.Parse(path[7]) - 1];
                    using (var input = replay.Open()) {
                        input.Seek(frame.Offset, SeekOrigin.Begin);
                        input.CopyToLength(stream, frame.Length);
                    }
                    return true;
                //case "getLastKeyFrameInfo":
                //case "endOfGameStats":
                default:
                    return false;
                }
            }
        }
    }
}
