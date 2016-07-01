using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using FileType = Kappa.BackEnd.Server.Patcher.ReleaseManifest.FileType;
using ManifestFile = Kappa.BackEnd.Server.Patcher.ReleaseManifest.ManifestFile;

namespace Kappa.BackEnd.Server.Patcher {
    public class ProjectPatcher {
        public ReleaseManifest Manifest { get; }
        public long TotalBytes { get; private set; }
        public long DoneBytes => patchingDone;

        private Region region;
        private string project, version;
        private string root;
        private string target;

        private Dictionary<string, RAFArchive> archives = new Dictionary<string, RAFArchive>();

        private ReleaseManifest old;

        private long patchingDone;

        public ProjectPatcher(Region region, string rads, string project, string version) {
            this.region = region;
            this.project = project;
            this.version = version;

            this.root = Path.Combine(rads, "projects", project);

            var releases = Path.Combine(this.root, "releases");
            target = Path.Combine(releases, version);

            if (Directory.Exists(releases)) {
                var newest = (from dir in Directory.EnumerateDirectories(releases)
                              let ver = Version.Parse(Path.GetFileName(dir))
                              orderby ver descending
                              select dir).FirstOrDefault();
                if (Directory.Exists(newest)) {
                    if (newest != target)
                        Directory.Move(newest, target);
                    var oldFile = Path.Combine(newest, "releasemanifest");
                    if (File.Exists(Path.Combine(newest, "S_OK")) && File.Exists(oldFile)) {
                        old = new ReleaseManifest(oldFile);
                    }
                }
            }

            Manifest = ReleaseManifest.Download(region, project, version, target);
        }

        public async Task Patch() {
            var files = Differences().ToList();

            if (!files.Any()) return;

            var manifest = DownloadPackageManifest();

            TotalBytes = files.Sum(f => (long) f.CompletedSize);

            var tasks = (from file in files
                         group file by file.MetaData.Release into bin
                         select DownloadGroup(bin, manifest)).ToList();

            await Task.WhenAll(tasks);

            foreach (var archive in archives.Values) {
                archive.Save();
            }

            using (File.Create(Path.Combine(target, "S_OK"))) { }
        }

        public Stream GetStream(ManifestFile man, bool write) {
            Stream stream;
            if (man.MetaData.Type == FileType.COMPRESSED_ARCHIVE || man.MetaData.Type == FileType.UNCOMPRESSED_ARCHIVE)
                stream = GetArchive(man.MetaData.Version).Write(man);
            else {
                string dst;
                if (man.MetaData.Type == FileType.MANAGEDFILE)
                    dst = Path.Combine(this.root, "managedfiles", man.MetaData.Version, man.FullName);
                else
                    dst = Path.Combine(this.root, "releases", version, "deploy", man.FullName);

                Directory.CreateDirectory(Path.GetDirectoryName(dst));

                stream = write ? File.OpenWrite(dst) : File.OpenRead(dst);
            }

            return stream;
        }

        private Dictionary<string, PackageFile> DownloadPackageManifest() {
            var dict = new Dictionary<string, PackageFile>();
            var req = WebRequest.CreateHttp(region.PackageManifest(project, version));
            using (var res = req.GetResponse())
            using (var reader = new StreamReader(res.GetResponseStream())) {
                var header = reader.ReadLine();
                if (header != "PKG1") Debug.Write("UNEXPECTED HEADER: " + header);
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var file = new PackageFile(line);
                    dict.Add(file.Path, file);
                }
            }
            return dict;
        }

        private async Task DownloadGroup(IEnumerable<ManifestFile> group, Dictionary<string, PackageFile> manifest) {
            foreach (var mFile in group) {
                var ext = mFile.MetaData.Type > FileType.NORMAL_FILE ? ".compressed" : "";
                var url = $"/projects/{project}/releases/{mFile.MetaData.Version}/files/{mFile.FullName.Replace('\\', '/')}{ext}";

                PackageFile pFile;
                if (!manifest.TryGetValue(url, out pFile)) throw new Exception("404");

                var binUrl = $"projects/{project}/releases/{version}/packages/files/{pFile.BinName}";
                while (true) {
                    var req = WebRequest.CreateHttp(new Uri(region.UpdateBase, binUrl));
                    req.AddRange(pFile.Offset, pFile.Offset + pFile.Length - 1);
                    try {
                        using (var res = await req.GetResponseAsync())
                        using (var src = res.GetResponseStream()) {
                            using (var save = this.GetStream(mFile, true)) {
                                long start = save.Position;

                                if (mFile.MetaData.Type > FileType.NORMAL_FILE && mFile.MetaData.Type != FileType.COMPRESSED_ARCHIVE) {
                                    using (var wrap = new InflaterInputStream(src))
                                        await wrap.CopyToAsync(save);
                                }
                                else await src.CopyToAsync(save);

                                if (save.Position - start != mFile.CompletedSize) {
                                    throw new Exception("File not properly downloaded: " + mFile.FullName);
                                }
                            }
                        }
                        break;
                    } catch (ZipException) {
                    } catch (IOException) { }
                }

                Interlocked.Add(ref patchingDone, mFile.CompletedSize);
            }
        }

        private IEnumerable<ManifestFile> Differences() {
            foreach (var pair in Manifest.AllFiles) {
                if (old == null) {
                    yield return pair.Value;
                }
                else if (pair.Value.MetaData.Type == FileType.COMPRESSED_ARCHIVE || pair.Value.MetaData.Type == FileType.UNCOMPRESSED_ARCHIVE) {
                    var raf = GetArchive(pair.Value.MetaData.Version);

                    if (!raf.HasFile(pair.Key))
                        yield return pair.Value;
                }
                else {
                    var oldFile = old.AllFiles[pair.Key];
                    if (oldFile == null || !oldFile.ChecksumEquals(pair.Value)) {
                        yield return pair.Value;
                    }
                }
            }
        }

        private RAFArchive GetArchive(string v) {
            RAFArchive archive;
            if (archives.TryGetValue(v, out archive)) return archive;

            var folder = Path.Combine(this.root, "filearchives", v);
            Directory.CreateDirectory(folder);
            var rafs = Directory.EnumerateFiles(folder, "*.raf").ToList();

            if (rafs.Any())
                archive = new RAFArchive(rafs.First());
            else
                archive = new RAFArchive(Path.Combine(folder, "Archive_1.raf"));

            archives.Add(v, archive);
            return archive;
        }

        private class PackageFile {
            public string Path { get; }
            public string BinName { get; }
            public long Offset { get; }
            public int Length { get; }

            public PackageFile(string line) {
                var split = line.Split(',');
                Path = split[0];
                BinName = split[1];
                Offset = long.Parse(split[2]);
                Length = int.Parse(split[3]);
                if (int.Parse(split[4]) != 0)
                    Debug.WriteLine(line);
            }
        }
    }
}
