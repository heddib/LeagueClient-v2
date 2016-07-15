using System;

namespace Kappa.BackEnd {
    public class Region {
        public enum PatcherBranch {
            LIVE,
            PBE
        }

        private Region(string key) {
            SpectatorServer = $"spectator.{key}.lol.riotgames.com";
            MainServer = $"prod.{key}.lol.riotgames.com";
            ChatServer = $"chat.{key}.lol.riotgames.com";
            LoginQueue = $"lq.{key}.lol.riotgames.com";
        }

        public string MainServer { get; private set; }
        public string SpectatorServer { get; private set; }
        public string ChatServer { get; private set; }
        public string LoginQueue { get; private set; }
        public string Platform { get; private set; }

        public string ReleaseListingSuffix { get; private set; }
        public PatcherBranch Branch { get; private set; } = PatcherBranch.LIVE;

        public Uri UpdateBase => new Uri($"http://l3cdn.riotgames.com/releases/{Branch.ToString().ToLower()}/");

        public Uri ReleaseListing(string category, string name) {
            return new Uri(UpdateBase, $"{category}/{name}/releases/releaselisting_" + ReleaseListingSuffix);
        }

        public Uri ReleaseManifest(string name, string version) {
            return new Uri(UpdateBase, $"projects/{name}/releases/{version}/releasemanifest");
        }

        public Uri SolutionManifest(string name, string version) {
            return new Uri(UpdateBase, $"solutions/{name}/releases/{version}/solutionmanifest");
        }

        public Uri PackageManifest(string name, string version) {
            return new Uri(UpdateBase, $"projects/{name}/releases/{version}/packages/files/packagemanifest");
        }

        public static readonly Region NA = new Region("na2") {
            Platform = "NA1",

            ReleaseListingSuffix = "NA"
        };

        public static readonly Region EUW = new Region("euw1") {
            Platform = "EUW1",

            ReleaseListingSuffix = "EUW"
        };

        public static readonly Region PBE = new Region("pbe1") {
            Platform = "PBE1",

            ReleaseListingSuffix = "PBE",
            Branch = PatcherBranch.PBE
        };

        public static Region Current { get; set; } = NA;
        public static string Locale { get; set; } = "en_US";
    }
}
