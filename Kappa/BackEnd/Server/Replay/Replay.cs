using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Kappa.Util;
using MFroehlich.Parsing;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Replay {
    public class Replay {
        private const byte Version = 1;
        private static readonly byte[] Magic = "lolreplay".GetBytes();

        public GameMetaData MetaData { get; }
        public byte[] RawMetaData { get; }
        public Position[] Chunks { get; }
        public Position[] Frames { get; }

        private string file;

        public Replay(string file) {
            this.file = file;
            using (var stream = File.OpenRead(file)) {
                var magic = new byte[Magic.Length];
                stream.ReadFully(magic, 0, magic.Length);
                if (!magic.SequenceEqual(Magic)) throw new Exception("Magic does not match");

                var version = stream.ReadByte();
                if (version != Version) throw new Exception("Version does not match");

                int metadataLength = stream.ReadStruct<int>();
                Chunks = new Position[stream.ReadStruct<int>()];
                Frames = new Position[stream.ReadStruct<int>()];

                RawMetaData = new byte[metadataLength];
                stream.ReadFully(RawMetaData, 0, RawMetaData.Length);

                for (var i = 0; i < Chunks.Length; i++)
                    Chunks[i] = stream.ReadStruct<Position>();

                for (var i = 0; i < Frames.Length; i++)
                    Frames[i] = stream.ReadStruct<Position>();
            }

            MetaData = JSONDeserializer.Deserialize<GameMetaData>(JSONParser.ParseObject(RawMetaData));
        }

        public Stream Open() => File.OpenRead(file);


        // Version: byte
        // MetaDataLength: int
        // ChunkCount: int
        // FrameCount: int
        // MetaData: UTF8 x MetaDataLength
        // ChunkHeaders: Position x ChunkCount
        // FrameHeaders: Position x FrameCount
        // Chunks: byte[] x ChunkCount
        // Frames: byte[] x FrameCount

        private static readonly Uri BaseUri = QuickHttp.Uri(Region.Current.SpectatorServer, "/observer-mode/rest/consumer/");

        public static Replay Record(long gameId, string encryptKey, string file) {
            var path = Path.GetTempFileName();
            using (var tmp = File.Create(path)) {
                var gameOver = false;
                var timer = new Stopwatch();

                var chunks = new List<Position>();
                var frames = new List<Position>();
                while (true) {
                    var info = GetChunkInfo(gameId);
                    timer.Restart();

                    if (!gameOver && info.EndGameChunkId > 0) {
                        Debug.WriteLine("Last chunk: " + info.EndGameChunkId, "Recording");
                        gameOver = true;
                    }


                    for (var i = chunks.Count + 1; i <= info.ChunkId; i++) {
                        try {
                            using (var chunk = GetChunk(gameId, i)) {
                                var pos = new Position();
                                pos.Offset = tmp.Position;

                                chunk.CopyTo(tmp);

                                pos.Length = tmp.Position - pos.Offset;
                                chunks.Add(pos);
                            }
                        } catch (WebException x) {
                            var code = ((HttpWebResponse) x.Response).StatusCode;
                            if (code == HttpStatusCode.NotFound) throw new Exception("It is too late to record this game");

                            Debug.WriteLine($"Error fetching chunk {i}: {code}");
                        }
                    }

                    for (var i = frames.Count + 1; i <= info.KeyFrameId; i++) {
                        try {
                            using (var frame = GetKeyframe(gameId, i)) {
                                var pos = new Position();
                                pos.Offset = tmp.Position;

                                frame.CopyTo(tmp);

                                pos.Length = tmp.Position - pos.Offset;
                                frames.Add(pos);
                            }
                        } catch (WebException x) {
                            var code = ((HttpWebResponse) x.Response).StatusCode;
                            if (code == HttpStatusCode.NotFound) throw new Exception("It is too late to record this game");

                            Debug.WriteLine($"Error fetching frame {i}: {code}");
                        }
                    }

                    if (info.EndGameChunkId == info.ChunkId && info.ChunkId != 0) {
                        break;
                    }

                    int wait = info.NextAvailableChunk - (int) timer.ElapsedMilliseconds;
                    if (wait > 0) Thread.Sleep(wait);
                }

                var meta = GetMetaData(gameId);
                meta["encryptionKey"] = encryptKey;
                var metaRaw = meta.ToJSON().GetBytes();

                Directory.CreateDirectory(Path.GetDirectoryName(file));
                using (var output = File.Create(file)) {
                    output.Write(Magic, 0, Magic.Length);
                    output.WriteByte(Version);
                    output.WriteStruct(metaRaw.Length);
                    output.WriteStruct(chunks.Count);
                    output.WriteStruct(frames.Count);

                    output.Write(metaRaw, 0, metaRaw.Length);

                    long offset = output.Position;
                    offset += chunks.Count * Marshal.SizeOf<Position>();
                    offset += frames.Count * Marshal.SizeOf<Position>();

                    foreach (var chunk in chunks) {
                        output.WriteStruct(new Position {
                            Offset = offset,
                            Length = chunk.Length
                        });
                        offset += chunk.Length;
                    }

                    foreach (var frame in frames) {
                        output.WriteStruct(new Position {
                            Offset = offset,
                            Length = frame.Length
                        });
                        offset += frame.Length;
                    }

                    foreach (var chunk in chunks) {
                        tmp.Seek(chunk.Offset, SeekOrigin.Begin);
                        tmp.CopyToLength(output, chunk.Length);
                    }

                    foreach (var frame in frames) {
                        tmp.Seek(frame.Offset, SeekOrigin.Begin);
                        tmp.CopyToLength(output, frame.Length);
                    }
                }
            }

            File.Delete(path);
            return new Replay(file);
        }

        private static JSONObject GetMetaData(long gameId) {
            var uri = new Uri(BaseUri, $"getGameMetaData/{Region.Current.Platform}/{gameId}/1/token");
            return QuickHttp.Request("GET", uri.AbsoluteUri).JSONObject().Result;
        }

        private static ChunkInfo GetChunkInfo(long gameId) {
            var uri = new Uri(BaseUri, $"getLastChunkInfo/{Region.Current.Platform}/{gameId}/1/token");
            return JSONDeserializer.Deserialize<ChunkInfo>(QuickHttp.Request("GET", uri.AbsoluteUri).JSONObject().Result);
        }

        private static Stream GetChunk(long gameId, int id) {
            var uri = new Uri(BaseUri, $"getGameDataChunk/{Region.Current.Platform}/{gameId}/{id}/token");
            var req = WebRequest.CreateHttp(uri);
            return req.GetResponse().GetResponseStream();
        }

        private static Stream GetKeyframe(long gameId, int id) {
            var uri = new Uri(BaseUri, $"getKeyFrame/{Region.Current.Platform}/{gameId}/{id}/token");
            var req = WebRequest.CreateHttp(uri);
            return req.GetResponse().GetResponseStream();
        }

        // ReSharper disable All
        //Not the full meta data, just the needed declarations//
        public class ChunkInfo : JSONSerializable {
            [JSONField("nextChunkId")]
            public int NextChunkId { get; set; }

            [JSONField("endStartupChunkId")]
            public int EndStartupChunkId { get; set; }

            [JSONField("keyFrameId")]
            public int KeyFrameId { get; set; }

            [JSONField("startGameChunkId")]
            public int StartGameChunkId { get; set; }

            [JSONField("endGameChunkId")]
            public int EndGameChunkId { get; set; }

            [JSONField("nextAvailableChunk")]
            public int NextAvailableChunk { get; set; }

            [JSONField("duration")]
            public int Duration { get; set; }

            [JSONField("availableSince")]
            public int AvailableSince { get; set; }

            [JSONField("chunkId")]
            public int ChunkId { get; set; }
        }

        //Not the full meta data, just the needed declarations//
        public class GameMetaData : JSONSerializable {
            [JSONField("startGameChunkId")]
            public int StartGameChunkId { get; set; }

            [JSONField("endGameKeyFrameId")]
            public int EndGameKeyFrameId { get; set; }

            [JSONField("endGameChunkId")]
            public int EndGameChunkId { get; set; }

            [JSONField("endStartupChunkId")]
            public int EndStartupChunkId { get; set; }

            [JSONField("encryptionKey")]
            public string EncryptionKey { get; set; }

            [JSONField("gameKey")]
            public GameMetaDataKey GameKey { get; set; }

            public class GameMetaDataKey : JSONSerializable {
                [JSONField("gameId")]
                public long GameId { get; set; }
            }
        }
        // ReSharper restore All

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Position {
            public long Offset;
            public long Length;
        }
    }
}
