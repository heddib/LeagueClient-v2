using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;

namespace LeagueSharp.Archives {
    public class WADBuilder : IDisposable {
        private const ushort Magic = 22354;
        private const byte Major = 2, Minor = 0;

        private static readonly SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();

        private Stream output;
        private int count;
        private long headersOffset;
        private long contentOffset;
        private object _lock = new object();


        public WADBuilder(string file, int count, byte[] unk, ulong unk2) {
            this.output = File.Create(file);

            output.WriteStruct(new WADArchive.Header {
                Magic = Magic,
                Major = Major,
                Minor = Minor
            });

            output.Write(unk, 0, unk.Length);

            headersOffset = output.Position + 16;

            output.WriteStruct(unk2);
            output.WriteStruct((ushort) headersOffset);
            output.WriteStruct((ushort) Marshal.SizeOf<WADArchive.FileInfo>());
            output.WriteStruct(count);

            contentOffset = headersOffset + count * Marshal.SizeOf<WADArchive.FileInfo>();
        }

        /// <summary>
        /// Creates an entry in the archive and writes to it without compression
        /// </summary>
        /// <param name="hash">The path hash to identify the entry</param>
        /// <param name="content">The content of the entry</param>
        public void Write(ulong hash, byte[] content) {
            Write(hash, content, content.Length, false);
        }

        /// <summary>
        /// Creates an entry in the archive and writes to it with compression
        /// </summary>
        /// <param name="hash">The path hash to identify the entry</param>
        /// <param name="raw">The content to compress and store in the entry</param>
        public void Compress(ulong hash, byte[] raw) {
            byte[] content;
            using (var mem = new MemoryStream()) {
                using (var gzip = new GZipOutputStream(mem)) {
                    gzip.Write(raw, 0, raw.Length);
                }
                content = mem.ToArray();
            }

            Write(hash, content, raw.Length, true);
        }

        private static byte[] Decompress(byte[] comp) {
            using (var src = new MemoryStream(comp))
            using (var dst = new MemoryStream())
            using (var gzip = new GZipInputStream(src)) {
                gzip.CopyTo(dst);
                return dst.ToArray();
            }
        }

        /// <summary>
        /// Creates an entry in the archive and writes to int without compression
        /// </summary>
        /// <param name="hash">The path hash to identify the entry</param>
        /// <param name="content">The content of the entry</param>
        /// <param name="length">The uncompresses length of the content, if it is compressed</param>
        /// <param name="compressed">True if the contents is compressed</param>
        public void Write(ulong hash, byte[] content, int length, bool compressed) {
            if (!compressed && length != content.Length) throw new ArgumentException("Uncompressed data size missmatch");

            lock (_lock) {
                output.Seek(headersOffset, SeekOrigin.Begin);
                output.WriteStruct(new WADArchive.FileInfo {
                    PathHash = hash,
                    Offset = (uint) contentOffset,
                    ZipSize = (uint) content.Length,
                    Size = (uint) length,
                    Zipped = (uint) (compressed ? 1 : 0),
                    SHA256 = BitConverter.ToUInt64(SHA256.ComputeHash(content), 0)//First 8 bytes of SHA256
                });
                headersOffset = output.Position;

                output.Seek(contentOffset, SeekOrigin.Begin);
                output.Write(content, 0, content.Length);
                contentOffset = output.Position;

                this.count++;
            }
        }

        public void Dispose() {
            output.Dispose();
        }
    }
}
