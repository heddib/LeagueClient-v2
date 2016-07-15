using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Kappa.BackEnd.Server.Patcher {
    public class ReleaseManifest {
        private string[] strings;

        public Dictionary<string, ManifestFile> AllFiles { get; }

        public static ReleaseManifest Download(Region region, string name, string version, string target) {
            var dst = Path.Combine(target, "releasemanifest");
            var tmp = Path.Combine(target, "releasemanifest.tmp");

            while (true) {
                Directory.CreateDirectory(target);

                var req = WebRequest.CreateHttp(region.ReleaseManifest(name, version));
                using (var res = req.GetResponse())
                using (var file = File.Create(tmp)) {
                    res.GetResponseStream().CopyTo(file);
                }

                try {
                    var manifest = new ReleaseManifest(tmp);

                    File.Delete(dst);
                    File.Move(tmp, dst);

                    return manifest;
                } catch {
                    Debug.WriteLine("Error downloading manifest, trying again...");
                    // Error while downloading //
                }
            }
        }

        public ReleaseManifest(string file) {
            using (var stream = File.OpenRead(file)) {
                stream.ReadStruct<ReleaseManifestHeader>();

                var directories = new DirectoryMetaData[stream.ReadStruct<int>()];
                for (var i = 0; i < directories.Length; i++) directories[i] = stream.ReadStruct<DirectoryMetaData>();

                var files = new FileMetaData[stream.ReadStruct<int>()];
                for (var i = 0; i < files.Length; i++) files[i] = stream.ReadStruct<FileMetaData>();

                strings = new string[stream.ReadStruct<int>()];
                stream.ReadStruct<int>();

                var build = new StringBuilder();
                for (int i = 0; i < strings.Length; i++) {
                    int c;
                    while ((c = stream.ReadByte()) > 0)
                        build.Append((char) c);
                    strings[i] = build.ToString();
                    build.Clear();
                }

                AllFiles = new Dictionary<string, ManifestFile>(files.Length);

                var manDirs = directories.Select(d => new ManifestDirectory(d, strings[d.NameIndex])).ToArray();
                var manFiles = files.Select(d => new ManifestFile(d, strings[d.NameIndex])).ToArray();

                for (var i = 0; i < directories.Length; i++) {
                    int start = directories[i].FirstSubdirIndex;
                    if (start == i) start++;

                    for (var j = 0; j < directories[i].SubdirCount; j++) {
                        manDirs[start + j].LinkToParent(manDirs[i]);
                    }
                }

                for (var i = 0; i < directories.Length; i++) {
                    int start = directories[i].FirstFileIndex;

                    for (var j = 0; j < directories[i].FileCount; j++) {
                        manFiles[start + j].LinkToParent(manDirs[i]);
                        AllFiles[manFiles[start + j].FullName] = manFiles[start + j];
                    }
                }
            }
        }

        public class ManifestDirectory {
            public List<ManifestDirectory> Subdirectories { get; } = new List<ManifestDirectory>();
            public List<ManifestFile> Files { get; } = new List<ManifestFile>();
            public string FullName { get; private set; }

            internal DirectoryMetaData MetaData { get; }

            internal ManifestDirectory(DirectoryMetaData meta, string fullName) {
                FullName = fullName;
                MetaData = meta;
            }

            internal void LinkToParent(ManifestDirectory parent) {
                parent.Subdirectories.Add(this);
                FullName = Path.Combine(parent.FullName, FullName);
            }
        }

        public class ManifestFile {
            public string FullName { get; private set; }

            internal FileMetaData MetaData { get; }

            internal ManifestFile(FileMetaData meta, string name) {
                MetaData = meta;
                FullName = name;
            }

            internal void LinkToParent(ManifestDirectory parent) {
                parent.Files.Add(this);
                FullName = Path.Combine(parent.FullName, FullName);
            }

            public bool ChecksumEquals(ManifestFile value) {
                return MetaData.Checksum.ToByteArray().SequenceEqual(value.MetaData.Checksum.ToByteArray());
            }

            public int CompletedSize => MetaData.Type == FileType.COMPRESSED_ARCHIVE ? MetaData.SizeCompressed : MetaData.Size;
        }

        // ReSharper disable InconsistentNaming
        public static class FileType {
            public const int
                NORMAL_FILE = 0,
                COMPRESSED = 2,
                COPY_TO_SLN = 4,
                MANAGEDFILE = 5,
                UNCOMPRESSED_ARCHIVE = 6,
                COMPRESSED_ARCHIVE = 22;
        }
        // ReSharper restore InconsistentNaming

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct ReleaseManifestHeader {
            public int MagicHeaderInt;
            public int Type;
            public int ItemCount;
            public int Release;

            public string Version => string.Join(".", BitConverter.GetBytes(Release).Reverse());
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct DirectoryMetaData {
            public int NameIndex;
            public int FirstSubdirIndex;
            public int SubdirCount;
            public int FirstFileIndex;
            public int FileCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct FileMetaData {
            public int NameIndex;
            public int Release;

            public Checksum16 Checksum;

            public int Type;
            public int Size;
            public int SizeCompressed;
            public long unknown;

            public string Version => string.Join(".", BitConverter.GetBytes(Release).Reverse());
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        internal struct Checksum16 { }
    }
}
