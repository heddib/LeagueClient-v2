using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MFroehlich.Parsing;

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

        public WADFile GetFile(string path) {
            var bytes = xxHash.ComputeHash(Encoding.UTF8.GetBytes(path));
            var hash = BitConverter.ToUInt64(bytes, 0);
            return AllFiles[hash];
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
    }
}
