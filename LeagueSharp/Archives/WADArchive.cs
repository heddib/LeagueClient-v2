using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MFroehlich.Parsing;

namespace LeagueSharp.Archives {
    public class WADArchive {
        public Dictionary<ulong, FileInfo> AllFiles { get; } = new Dictionary<ulong, FileInfo>();
        public System.IO.FileInfo File;
        public byte[] Unknown1 { get; } = new byte[84];
        public ulong Unknown2 { get; }
        public ushort FileInfosOffset { get; }
        public ushort FileInfoSize { get; }

        // ReSharper disable once InconsistentNaming
        private static readonly xxHash xxHash = new xxHash(64);

        public WADArchive(string wad) {
            File = new System.IO.FileInfo(wad);

            using (var stream = File.OpenRead()) {
                var header = stream.ReadStruct<Header>();

                stream.ReadFully(Unknown1, 0, Unknown1.Length);
                Unknown2 = stream.ReadStruct<ulong>();
                FileInfosOffset = stream.ReadStruct<ushort>();
                FileInfoSize = stream.ReadStruct<ushort>();
                var count = stream.ReadStruct<uint>();

                for (var i = 0; i < count; i++) {
                    var file = stream.ReadStruct<FileInfo>();
                    AllFiles.Add(file.PathHash, file);
                }
            }
        }

        public bool TryGetFile(string path, out FileInfo fileInfo) {
            return AllFiles.TryGetValue(Hash(path), out fileInfo);
        }

        public byte[] ExtractTest(FileInfo fileInfo, out byte[] thing) {
            using (var tmp = File.OpenRead()) {
                tmp.Seek(fileInfo.Offset, SeekOrigin.Begin);
                thing = new byte[fileInfo.ZipSize];
                tmp.ReadFully(thing, 0, thing.Length);
            }
            using (var stream = GetStream(fileInfo))
            using (var mem = new MemoryStream()) {
                stream.CopyToLength(mem, fileInfo.Size);
                return mem.ToArray();
            }
        }

        public byte[] Extract(FileInfo fileInfo) {
            using (var stream = GetStream(fileInfo))
            using (var mem = new MemoryStream()) {
                stream.CopyToLength(mem, fileInfo.Size);
                return mem.ToArray();
            }
        }

        public void ExtractFile(FileInfo fileInfo, string dst, bool extend = true) {
            using (var stream = GetStream(fileInfo)) {
                Directory.CreateDirectory(Path.GetDirectoryName(dst));

                var peek = new byte[8];
                stream.ReadFully(peek, 0, peek.Length);
                var ext = GetExtension(peek);
                if (!dst.EndsWith(ext))
                    dst += GetExtension(peek);

                using (var extract = System.IO.File.OpenWrite(dst)) {
                    extract.Write(peek, 0, peek.Length);

                    stream.CopyToLength(extract, fileInfo.Size - peek.Length);
                }
            }
        }

        private Stream GetStream(FileInfo fileInfo) {
            var stream = File.OpenRead();
            stream.Seek(fileInfo.Offset, SeekOrigin.Begin);
            if (fileInfo.Zipped != 0)
                return new GZipStream(stream, CompressionMode.Decompress);
            return stream;
        }

        public static ulong Hash(string path) {
            var bytes = xxHash.ComputeHash(Encoding.UTF8.GetBytes(path.ToLower()));
            var hash = BitConverter.ToUInt64(bytes, 0);
            return hash;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct Header {
            public ushort Magic;
            public byte Major;
            public byte Minor;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FileInfo {
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
