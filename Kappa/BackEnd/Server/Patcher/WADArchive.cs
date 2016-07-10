using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using MFroehlich.Parsing;
using System.Linq;

namespace Kappa.BackEnd.Server.Patcher {
    public class WADArchive {
        public Dictionary<ulong, WADFile> AllFiles { get; } = new Dictionary<ulong, WADFile>();
        public FileInfo File;

        // ReSharper disable once InconsistentNaming
        private static readonly xxHash xxHash = new xxHash(64);

        public WADArchive(string wad) {
            File = new FileInfo(wad);

            var padding = new byte[96];

            using (var stream = File.OpenRead()) {
                stream.ReadStruct<WADHeader>();

                stream.ReadFully(padding, 0, padding.Length);
                var count = stream.ReadStruct<uint>();

                for (var i = 0; i < count; i++) {
                    var file = stream.ReadStruct<WADFile>();
                    AllFiles.Add(file.PathHash, file);
                }
            }
        }

        public bool TryGetFile(string path, out WADFile file) {
            return AllFiles.TryGetValue(Hash(path), out file);
        }

        public string Extract(WADFile file) {
            if (file.Zipped == 0) {
                using (var stream = File.OpenRead())
                using (var mem = new MemoryStream()) {
                    stream.Seek(file.Offset, SeekOrigin.Begin);
                    stream.CopyToLength(mem, file.Size);
                    return Encoding.UTF8.GetString(mem.ToArray());
                }
            }

            using (var stream = File.OpenRead())
            using (var zip = new GZipStream(stream, CompressionMode.Decompress))
            using (var mem = new MemoryStream()) {
                stream.Seek(file.Offset, SeekOrigin.Begin);
                zip.CopyToLength(mem, file.Size);
                return Encoding.UTF8.GetString(mem.ToArray());
            }
        }

        public void ExtractFile(WADFile file, string dst) {
            using (var stream = File.OpenRead()) {
                Directory.CreateDirectory(Path.GetDirectoryName(dst));
                stream.Seek(file.Offset, SeekOrigin.Begin);

                if (file.Zipped == 0) {
                    var peek = new byte[8];
                    stream.ReadFully(peek, 0, peek.Length);
                    dst += GetExtension(peek);

                    using (var extract = System.IO.File.OpenWrite(dst)) {
                        extract.Write(peek, 0, peek.Length);

                        stream.CopyToLength(extract, file.Size - peek.Length);
                    }
                }
                else {
                    using (var zip = new GZipStream(stream, CompressionMode.Decompress)) {
                        var peek = new byte[8];
                        zip.ReadFully(peek, 0, peek.Length);
                        dst += GetExtension(peek);

                        using (var extract = System.IO.File.OpenWrite(dst)) {
                            extract.Write(peek, 0, peek.Length);

                            zip.CopyToLength(extract, file.Size - peek.Length);
                        }
                    }
                }
            }
        }

        public static ulong Hash(string path) {
            var bytes = xxHash.ComputeHash(Encoding.UTF8.GetBytes(path));
            var hash = BitConverter.ToUInt64(bytes, 0);
            return hash;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct WADHeader {
            public ushort Magic;
            public byte Major;
            public byte Minor;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WADFile {
            public ulong PathHash;
            public uint Offset;
            public uint ZipSize;
            public uint Size;
            public uint Zipped;
            public ulong SHA256;

            public override string ToString() {
                return $"{Offset}: {ZipSize} -> {Size}";
            }
        }

        private static string GetExtension(byte[] peek) {
            Func<int, int, string> sub = (s, c) => Encoding.UTF8.GetString(peek, s, c);

            if (sub(1, 3) == "PNG")
                return ".png";

            if (sub(0, 3) == "Ogg")
                return ".ogg";

            if (sub(0, 4) == "OTTO")
                return ".ttf";

            if (sub(0, 8) == "\"use str")
                return ".js";

            if (sub(0, 5) == "!func")
                return ".js";

            if (sub(0, 7) == "webpack" || sub(0, 8) == "/******/")
                return ".js";

            byte[] mp4 = { 26, 69, 223, 163, 1, 0, 0, 0 };
            if (peek.Take(8).SequenceEqual(mp4))
                return ".webm";

            byte[] jpg = { 255, 216 };
            if (peek.Take(2).SequenceEqual(jpg))
                return ".jpg";

            if (sub(0, 1) == "<")
                return ".html";

            if (sub(0, 1) == "{" || sub(3, 1) == "{" || sub(0, 1) == "[")
                return ".json";

            return "";
        }
    }
}
