using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.BackEnd {
    public class RiotVersion {
        public const string
          AirPath = @"RADS\projects\lol_air_client\releases",
          GamePath = @"RADS\projects\lol_game_client\releases",
          SolutionPath = @"RADS\solutions\lol_game_client_sln\releases";

        public Version AirVersion { get; }
        public Version GameVersion { get; }
        public Version SolutionVersion { get; }

        public List<RiotFile> AirFiles { get; } = new List<RiotFile>();

        private RiotVersion(Version air, Version game, Version solution) {
            AirVersion = air;
            GameVersion = game;
            SolutionVersion = solution;
        }

        public IEnumerable<RiotFile> GetFiles(string end) {
            end = end.ToLower();
            return from f in AirFiles
                   where f.Url.LocalPath.ToLower().EndsWith(end)
                   select f;
        }

        private async Task GetManifest() {
            var req = WebRequest.CreateHttp(new Uri(Region.Current.UpdateBase, $"projects/lol_air_client/releases/{AirVersion}/packages/files/packagemanifest"));
            using (var mem = new MemoryStream())
            using (var res = await req.GetResponseAsync()) {
                res.GetResponseStream()?.CopyTo(mem);
                string manifest = Encoding.UTF8.GetString(mem.ToArray()).Replace("PKG1", "");
                var lines = manifest.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                AirFiles.AddRange(lines.Select(l => new RiotFile(l, Region.Current.UpdateBase)));
            }
        }

        public static async Task<RiotVersion> GetInstalledVersion(string leagueDir) {
            var airInstalled = Directory.EnumerateDirectories(Path.Combine(leagueDir, AirPath));
            var airVersions = from dir in airInstalled
                              select Version.Parse(Path.GetFileName(dir)) into v
                              orderby v descending
                              select v;
            var gameInstalled = Directory.EnumerateDirectories(Path.Combine(leagueDir, GamePath));
            var gameVersions = from dir in gameInstalled
                               select Version.Parse(Path.GetFileName(dir)) into v
                               orderby v descending
                               select v;
            var slnInstalled = Directory.EnumerateDirectories(Path.Combine(leagueDir, SolutionPath));
            var slnVersions = from dir in slnInstalled
                              select Version.Parse(Path.GetFileName(dir)) into v
                              orderby v descending
                              select v;

            var ver = new RiotVersion(airVersions.FirstOrDefault(), gameVersions.FirstOrDefault(), slnVersions.FirstOrDefault());
            await ver.GetManifest();
            return ver;
        }

        public static async Task<RiotVersion> GetLatestVersion() {
            using (var web = new WebClient()) {
                var airList = await web.DownloadStringTaskAsync(Region.Current.ReleaseListing("projects", "lol_air_client"));
                var gameList = await web.DownloadStringTaskAsync(Region.Current.ReleaseListing("projects", "lol_game_client"));
                var solutionList = await web.DownloadStringTaskAsync(Region.Current.ReleaseListing("solutions", "lol_game_client_sln"));

                Version airVersion, gameVersion, solutionVersion;
                Version.TryParse(airList.Split('\n').FirstOrDefault(), out airVersion);
                Version.TryParse(gameList.Split('\n').FirstOrDefault(), out gameVersion);
                Version.TryParse(solutionList.Split('\n').FirstOrDefault(), out solutionVersion);

                var v = new RiotVersion(airVersion, gameVersion, solutionVersion);
                await v.GetManifest();
                return v;
            }
        }
    }

    public class RiotFile {
        public Uri Url { get; }
        public string Bin { get; }
        /// <summary>
        /// Offset of the file within the bin
        /// </summary>
        public int Offset { get; }
        /// <summary>
        /// Length of the file
        /// </summary>
        public int Length { get; }
        public int Unknown { get; }

        public RiotFile(string line, Uri root) {
            var thingies = line.Split(',');
            Url = new Uri(root, thingies[0].Substring(1));
            Bin = thingies[1];
            Offset = int.Parse(thingies[2]);
            Length = int.Parse(thingies[3]);
            Unknown = int.Parse(thingies[4]);
        }
    }
}
