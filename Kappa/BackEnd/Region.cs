using System;

namespace Kappa.BackEnd {
    public class Region {
        public enum PatcherBranch {
            LIVE,
            PBE
        }

        public string MainServer { get; private set; }
        public string ChatServer { get; private set; }
        public string Platform { get; private set; }

        public string NewsURL { get; private set; }
        public string LoginQueueURL { get; private set; }

        public string ReleaseListingSuffix { get; private set; }
        public PatcherBranch Branch { get; private set; }

        //public static readonly Uri UpdateBase = new Uri("http://l3cdn.riotgames.com/");
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

        public static readonly Region NA = new Region {
            MainServer = "prod.na2.lol.riotgames.com",
            LoginQueueURL = "https://lq.na2.lol.riotgames.com/",
            Platform = "NA1",

            NewsURL = "http://na.leagueoflegends.com/en/rss.xml",
            ChatServer = "chat.na2.lol.riotgames.com",

            ReleaseListingSuffix = "NA",
            Branch = PatcherBranch.LIVE
        };

        public static readonly Region EUW = new Region {
            MainServer = "prod.euw1.lol.riotgames.com",
            LoginQueueURL = "https://lq.euw1.lol.riotgames.com/",
            Platform = "EUW1",

            NewsURL = "http://euw.leagueoflegends.com/en/rss.xml",
            ChatServer = "chat.euw1.lol.riotgames.com",

            ReleaseListingSuffix = "EUW",
            Branch = PatcherBranch.LIVE
        };

        public static readonly Region PBE = new Region {
            MainServer = "prod.pbe1.lol.riotgames.com",
            LoginQueueURL = "https://lq.pbe1.lol.riotgames.com/",
            Platform = "PBE1",

            ChatServer = "chat.pbe1.lol.riotgames.com",

            ReleaseListingSuffix = "PBE",
            Branch = PatcherBranch.PBE
        };

        public static Region Current { get; set; } = NA;
        public static string Locale { get; set; } = "en_US";
    }
}
