using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MFroehlich.Parsing;

namespace Kappa.BackEnd.Server.Patcher {
    public class RAFArchive {
        public Dictionary<string, RAFFile> AllFiles { get; }

        private string raf;
        private string dat;

        public RAFArchive(string raf) {
            this.raf = raf;
            this.dat = raf + ".dat";

            if (!File.Exists(raf)) {
                AllFiles = new Dictionary<string, RAFFile>();
                return;
            }

            using (var stream = File.OpenRead(raf)) {
                var archiveheader = stream.ReadStruct<RAFHeader>();

                stream.Seek(archiveheader.NamesOffset, SeekOrigin.Begin);
                var stringsHeader = stream.ReadStruct<RAFStringsHeader>();

                var offsets = new RAFString[stringsHeader.Count];
                for (var i = 0; i < offsets.Length; i++) offsets[i] = stream.ReadStruct<RAFString>();

                var strings = new List<string>(offsets.Length);
                foreach (var x in offsets) {
                    stream.Seek(archiveheader.NamesOffset + x.Offset, SeekOrigin.Begin);

                    byte[] bytes = new byte[x.Length];
                    stream.ReadFully(bytes, 0, bytes.Length);

                    var value = Encoding.ASCII.GetString(bytes);
                    value = value.TrimEnd('\0');
                    strings.Add(value);
                }

                stream.Seek(archiveheader.FilesOffset, SeekOrigin.Begin);

                var fileCount = stream.ReadStruct<int>();
                AllFiles = new Dictionary<string, RAFFile>(fileCount);
                for (var i = 0; i < fileCount; i++) {
                    var file = stream.ReadStruct<RAFFile>();
                    var name = strings[file.NameIndex];
                    //file.Hash = new RAFFile(name, 0, 0).Hash;
                    AllFiles.Add(name, file);
                }
            }
        }

        public Stream Write(ReleaseManifest.ManifestFile man) {
            var info = new FileInfo(dat);
            var file = new RAFFile(man.FullName, info.Exists ? (int) info.Length : 0, man.CompletedSize);
            AllFiles[man.FullName] = file;
            return File.Open(dat, FileMode.Append);
        }

        public bool HasFile(string key) {

            return AllFiles.ContainsKey(key) || AllFiles.ContainsKey(key.Replace('\\', '/')) || AllFiles.ContainsKey(key.Replace('/', '\\'));
        }

        public void Save() {
            using (var stream = File.OpenWrite(this.raf)) {
                var header = new RAFHeader(this.AllFiles.Count);
                stream.WriteStruct(header);

                var ordered = this.AllFiles.OrderBy(pair => pair.Value.Hash).ThenBy(pair => pair.Key.ToLower()).ToList();

                stream.WriteStruct(ordered.Count);
                for (var i = 0; i < ordered.Count; i++) {
                    var file = ordered[i].Value;
                    file.NameIndex = i;
                    stream.WriteStruct(file);
                }

                var pathListSize = ordered.Sum(pair => pair.Key.Length + 1);//NULL byte

                stream.WriteStruct(new RAFStringsHeader(pathListSize, ordered.Count));

                int pathOffset = 8 + ordered.Count * Marshal.SizeOf<RAFString>();
                foreach (var pair in ordered) {
                    int l = pair.Key.Length + 1;
                    stream.WriteStruct(new RAFString(pathOffset, l));
                    pathOffset += l;
                }

                foreach (var pair in ordered) {
                    var bytes = Encoding.ASCII.GetBytes(pair.Key.Replace('\\', '/'));
                    stream.Write(bytes, 0, bytes.Length);
                    stream.WriteByte(0);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct RAFHeader {
            public int Magic;
            public int Version;
            public int ManagerIndex;
            public int FilesOffset;
            public int NamesOffset;

            public RAFHeader(int fileCount) : this() {
                Magic = 0x18be0ef0;
                Version = 1;
                ManagerIndex = 0;
                FilesOffset = Marshal.SizeOf(this);
                NamesOffset = FilesOffset + sizeof(int) + fileCount * Marshal.SizeOf<RAFFile>();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct RAFStringsHeader {
            // Size, in bytes, of the PathList
            public int SizeInBytes;
            // Number of path strings contained in the path list
            public int Count;

            public RAFStringsHeader(int sizeInBytes, int count) {
                SizeInBytes = sizeInBytes;
                Count = count;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct RAFString {
            // offset from the START OF THE STRING TABLE
            public int Offset;
            // length of the string, in bytes
            public int Length;

            public RAFString(int offset, int length) {
                Offset = offset;
                Length = length;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAFFile {
            public int Hash;
            public int DataOffset;
            public int DataLength;
            public int NameIndex;

            public RAFFile(string path, int offset, int length) {
                DataOffset = offset;
                DataLength = length;

                long hash = 0;
                foreach (byte b in Encoding.ASCII.GetBytes(path.Replace('\\', '/').ToLowerInvariant())) {
                    hash = (hash << 4) + b;
                    var temp = hash & 0xF0000000;
                    hash = hash ^ (temp >> 24);
                    hash = hash ^ temp;
                }
                Hash = (int) hash;

                NameIndex = -1;
            }
        }
    }
}
