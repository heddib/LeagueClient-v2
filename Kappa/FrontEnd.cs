using System;
using Kappa.BackEnd.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Kappa.BackEnd;

namespace Kappa {
    public class FrontEnd : HttpService {
        private static readonly List<string> ContentFileTypes = new List<string> { ".css", ".html", ".js" };

        private static readonly string FrontEndExecutable = "client.exe";

        private FilePack content;
        private FilePack assets;

        public FrontEnd(string content, string assets) : base("/ui") {
            this.content = new FilePack(content);
            this.assets = new FilePack(assets);
        }

        public void Start(string host) {
            string index = $"http://{host}/ui/client.html";

            string args = $"--backend=\"{index}\"";
#if BUILD_UI
            args += " --remote-debugging-port=1337 --disable-web-security";
            //Process.Start("http://localhost:1337/");
#endif
            Process.Start(FrontEndExecutable, args);

            content.Load();
            assets.Load();
        }

        public override bool Handle(HttpListenerContext context) {
            string path = context.Request.Url.LocalPath.Substring(BaseUrl.Length);

#if BUILD_UI
            if (content.TryHandle(path, context, false)) return true;
#else
            if (content.TryHandle(path, context, true)) return true;
#endif
            if (assets.TryHandle(path, context, true)) return true;

            return false;
        }

        [Conditional("BUILD_UI")]
        public void Build(string contentRoot, string assetsRoot) {
            var loose = from ext in ContentFileTypes
                        from file in Directory.EnumerateFiles(assetsRoot, "*" + ext, SearchOption.AllDirectories)
                        select file;
            foreach (string file in loose) Console.WriteLine(file);

            string src = Path.Combine(contentRoot, "client.less");
            string dst = Path.Combine(Path.GetDirectoryName(src), "bin", "compiled.css");

            const string npm = @"C:\Users\Max\AppData\Roaming\npm";
            Func<string, string, Process> start = (file, args) => Process.Start(new ProcessStartInfo {
                FileName = Path.Combine(npm, file + ".cmd"),
                WorkingDirectory = contentRoot,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = args,
            });

            var lessFiles = Directory.EnumerateFiles(contentRoot, "*.less", SearchOption.AllDirectories);
            var client = File.ReadAllText(src);
            foreach (string file in lessFiles) {
                if (client.Contains(Path.GetFileName(file))) continue;
                if (file.Contains("client.less")) continue;

                var path = file.Substring(contentRoot.Length + 1).Replace('\\', '/');
                Debug.WriteLine($"    @import \"{path}\";");
            }

            var less = start("lessc", $"-sm=\"on\" \"{Path.GetFullPath(src)}\" \"{Path.GetFullPath(dst)}\"");
            var tsc = start("tsc", "-p .");
            tsc.WaitForExit();

            var rollup = start("rollup", @"-o bin\compiled.js -f iife bin\client.js");
            rollup.WaitForExit();

            less.WaitForExit();

            if (less.ExitCode != 0) {
                Debug.WriteLine(less.StandardError.ReadToEnd());
                Debug.WriteLine(less.StandardOutput.ReadToEnd());
            }

            if (tsc.ExitCode != 0) {
                Debug.WriteLine(tsc.StandardError.ReadToEnd());
                Debug.WriteLine(tsc.StandardOutput.ReadToEnd());
            }

            if (rollup.ExitCode != 0) {
                Debug.WriteLine(rollup.StandardError.ReadToEnd());
                Debug.WriteLine(rollup.StandardOutput.ReadToEnd());
            }

            var contentFiles = Files(contentRoot, ContentFileTypes);
            var assetFiles = Files(assetsRoot);

            this.content.Create(contentFiles.ToList());
            this.assets.Create(assetFiles.ToList());
        }

        private static IEnumerable<FileAlias> Files(string root, IEnumerable<string> exts = null) {
            return from ext in exts ?? new[] { "" }
                   from file in Directory.EnumerateFiles(root, "*" + ext, SearchOption.AllDirectories)
                   let alias = file.Contains(@"\bin\") ? file.Substring(Path.Combine(root, "bin").Length) : file.Substring(root.Length)
                   select new FileAlias(file, alias.Replace("\\", "/"));
        }

        private class FileAlias {
            public FileInfo File { get; }
            public string Alias { get; }
            public FileAlias(string original, string name) {
                File = new FileInfo(original);
                Alias = name;
            }
        }

        private class FilePack {
            private FileInfo pack;
            private IReadOnlyDictionary<string, FileHeader> headers;

            public FilePack(string pack) {
                this.pack = new FileInfo(pack);
            }

            public bool TryHandle(string path, HttpListenerContext context, bool caching) {
                FileHeader header;

                if (headers.TryGetValue(path, out header))
                    context.Response.ContentType = GetMimeType(path);
                else if (headers.TryGetValue(path + "/index.html", out header))
                    context.Response.ContentType = "index.html";
                else
                    return false;

                if (header.Compressed)
                    context.Response.Headers.Add("Content-Encoding", "gzip");

                if (caching)
                    context.Response.Headers.Add("Cache-Control", "max-age=315360000, public");

                using (var file = pack.OpenRead())
                    HandleStream(context, file, header.Offset, header.Length);

                return true;
            }

            public void Load() {
                var dict = new Dictionary<string, FileHeader>();
                using (var file = pack.OpenRead()) {
                    var binary = new BinaryReader(file);
                    int count = binary.ReadInt32();
                    for (int i = 0; i < count; i++) {
                        string path = binary.ReadString();
                        bool zipped = binary.ReadBoolean();
                        long offset = binary.ReadInt64();
                        long length = binary.ReadInt64();
                        dict.Add(path, new FileHeader(offset, length, zipped));
                    }
                }
                headers = dict;
            }

            public void Create(List<FileAlias> src) {
                const int headerSize = 18;
                long offset = 4 + src.Sum(f => f.Alias.Length + headerSize);

                using (var save = new BinaryWriter(pack.Create())) {
                    save.Write(src.Count);
                    foreach (var alias in src) {
                        GetMimeType(alias.File.FullName);
                        long head = save.BaseStream.Position;
                        save.BaseStream.Seek(offset, SeekOrigin.Begin);

                        long size;
                        string mime = GetMimeType(alias.Alias);
                        bool compress = !(mime.Contains("audio") || mime.Contains("video") || mime.Contains("image"));

                        if (compress) {
                            using (var read = alias.File.OpenRead())
                            using (var gzip = new GZipStream(save.BaseStream, CompressionMode.Compress, true))
                                read.CopyTo(gzip);
                            size = save.BaseStream.Position - offset;
                        }
                        else {
                            using (var read = alias.File.OpenRead())
                                read.CopyTo(save.BaseStream);
                            size = alias.File.Length;
                        }

                        save.BaseStream.Seek(head, SeekOrigin.Begin);
                        long tmp = save.BaseStream.Position;
                        save.Write(alias.Alias);
                        save.Write(compress);
                        save.Write(offset);
                        save.Write(size);
                        if (save.BaseStream.Position - tmp - alias.Alias.Length != headerSize) throw new Exception("Headersize missmatch");

                        offset += size;
                    }
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct FileHeader {
                public long Offset;
                public long Length;
                public bool Compressed;

                public FileHeader(long o, long s, bool c) {
                    Offset = o;
                    Length = s;
                    Compressed = c;
                }
            }
        }
    }
}
