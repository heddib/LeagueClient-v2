﻿using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using Kappa.BackEnd.Server.Assets;
using MFroehlich.Parsing;
using MFroehlich.Parsing.JSON;

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
            return AllFiles[Hash(path)];
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
    }
}
