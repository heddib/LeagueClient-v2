using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Kappa.BackEnd.Server.Patcher.Model;
using Kappa.Settings;
using LeagueSharp;
using LeagueSharp.Archives;
using LeagueSharp.RADS;

namespace Kappa.BackEnd.Server.Patcher {
    public class PatcherService : JSONService {
        public List<string> WADs { get; } = new List<string>();
        public string WADVersion { get; private set; }

        //private static readonly string RADS = Path.Combine(@"C:\Riot Games", "League of Legends", "RADS");
        private static readonly string RADS = Path.Combine(Session.AppData, "League of Legends", "RADS");
        private const string SolutionName = "lol_game_client_sln";

        private Session session;
        private Settings settings;

        private string solutionTarget;
        private BlockingCollection<AsyncPatch> wadQueue = new BlockingCollection<AsyncPatch>();

        private PatcherState gameState;
        private PatcherState launcherState;

        internal static readonly string Config = Path.Combine(Session.AppData, "League of Legends", "config");
        internal string ExecutablePath => Path.Combine(solutionTarget, "deploy", "League of Legends.exe");

        public PatcherService(Session session) : base("/patcher") {
            this.session = session;
            this.settings = GetSettings<Settings>();

            gameState = new PatcherState { Phase = PatcherPhase.PATCHING };
            launcherState = new PatcherState { Phase = PatcherPhase.PATCHING };

            Session.Init.ContinueWith(t => OnInitialized());
        }

        [Endpoint("/game")]
        public PatcherState GetState() {
            return gameState;
        }

        [Endpoint("/launcher")]
        public PatcherState GetLauncherState() {
            return launcherState;
        }

        public void OnInitialized() {
            new Thread(Patch) { Name = "Patcher", IsBackground = true }.Start();
        }

        private async void Patch() {
            PatchWAD();

            try {
                await PatchGame(Region.Current, Region.Locale);
            } catch (Exception x) {
                Debug.WriteLine(x);
                Directory.Delete(RADS, true);
                await PatchGame(Region.Current, Region.Locale);
            }

            gameState.Phase = PatcherPhase.NONE;
            Debug.WriteLine("Patching completed");
        }

        private void PatchWAD() {
            using (var web = new WebClient()) {
                try {
                    var listing = web.DownloadString(Region.Current.ReleaseListing("projects", "league_client"));
                    var latest = listing.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).First();
                    WADVersion = latest;

                    while (true) {
                        var wad = wadQueue.Take();
                        var localFile = GetStorage(wad.Name + ".wad");

                        string saved;
                        if (!File.Exists(localFile) || !settings.WADVersions.TryGetValue(wad.Name, out saved) || latest != saved) {
                            var package = web.DownloadString(Region.Current.PackageManifest("league_client", latest));
                            var manifest = package.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Skip(1);
                            var files = manifest.Select(m => new RiotFile(m, Region.Current.UpdateBase));
                            var file = files.Single(s => s.Url.LocalPath.ToLower().EndsWith($"/{wad.Name}/assets.wad.compressed"));

                            while (true) {
                                launcherState.Current = 0;
                                launcherState.Total = file.Length;

                                try {
                                    using (var res = WebRequest.CreateHttp(file.Url).GetResponse())
                                    using (var src = res.GetResponseStream())
                                    using (var wrap = new InflaterInputStream(src))
                                    using (var save = File.Create(localFile)) {
                                        int read;
                                        long total = 0;
                                        var data = new byte[8192];
                                        while ((read = wrap.Read(data, 0, data.Length)) > 0) {
                                            save.Write(data, 0, read);
                                            total += read;
                                            launcherState.Current = Math.Min(total, launcherState.Total);
                                        }
                                    }

                                    break;
                                } catch (ZipException) {
                                    Session.Log($"{wad.Name} wad download failed, trying again...");
                                }
                            }

                            settings.WADVersions[wad.Name] = latest;
                            settings.Save();
                        }

                        Session.Log($"{wad.Name}: v{latest}");
                        wad.Supply(new WADArchive(localFile));
                    }
                } catch (InvalidOperationException) {
                    //Ignore//
                }

                launcherState.Phase = PatcherPhase.NONE;
            }
        }

        internal async Task<WADArchive> PatchWAD(string plugin) {
            WADs.Add(plugin);

            var patch = new AsyncPatch(plugin);
            wadQueue.Add(patch);
            return await patch.Get();
        }

        internal void FinishWAD() {
            wadQueue.CompleteAdding();
        }

        private async Task PatchGame(Region region, string locale) {
            List<string> manifest;
            using (var web = new WebClient()) {
                var rawListing = web.DownloadString(region.ReleaseListing("solutions", SolutionName));
                var version = rawListing.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).First();

                var rawManifest = web.DownloadString(region.SolutionManifest(SolutionName, version));
                manifest = rawManifest.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                solutionTarget = Path.Combine(RADS, "solutions", SolutionName, "releases", version);
                Directory.CreateDirectory(solutionTarget);

                File.WriteAllText(Path.Combine(solutionTarget, "solutionmanifest"), rawManifest);
            }

            int index = manifest.IndexOf(locale.ToLower());
            int count = int.Parse(manifest[index + 2]);
            var required = new Dictionary<string, string>();
            for (var i = 0; i < count; i++) {
                var name = manifest[index + 3 + i];
                var project = manifest.IndexOf(name);
                required[name] = manifest[project + 1];
            }

            using (var config = File.OpenWrite(Path.Combine(solutionTarget, "configurationmanifest")))
            using (var writer = new StreamWriter(config)) {
                writer.WriteLine("RADS Configuration Manifest");
                writer.WriteLine(manifest[1]);//version
                writer.WriteLine(locale.ToLower());
                writer.WriteLine(required.Count);
                foreach (var name in required.Keys) writer.WriteLine(name);
            }

            var patchers = required.Select(pair => new ProjectPatcher(region, RADS, pair.Key, pair.Value)).ToList();
            var tasks = patchers.Select(p => p.Patch()).ToList();

            gameState.Total = patchers.Sum(p => p.TotalBytes);

            while (tasks.Any()) {
                await Task.Delay(1000);
                gameState.Current = patchers.Sum(p => p.DoneBytes);

                foreach (var task in tasks.Where(t => t.IsFaulted)) {
                    throw task.Exception.InnerException;
                }
                tasks.RemoveAll(t => t.IsCompleted);
            }

            foreach (var copy in patchers.SelectMany(p => p.CopyToSln)) {
                var dst = Path.Combine(solutionTarget, "deploy", copy.Item1.FullName);
                var src = copy.Item2;
                try {
                    Directory.CreateDirectory(Path.GetDirectoryName(dst));
                    File.Copy(src, dst);
                } catch {
                    //
                }
            }

            using (File.Create(Path.Combine(solutionTarget, "S_OK"))) { }
        }

        private class Settings : BaseSettings {
            public Dictionary<string, string> WADVersions { get; set; } = new Dictionary<string, string>();
        }

        private class AsyncPatch {
            public string Name { get; }
            private readonly TaskCompletionSource<WADArchive> tcs;

            public AsyncPatch(string name) {
                this.Name = name;
                this.tcs = new TaskCompletionSource<WADArchive>();
            }

            public async Task<WADArchive> Get() => await tcs.Task;
            public void Supply(WADArchive arg) => tcs.SetResult(arg);
        }
    }
}
