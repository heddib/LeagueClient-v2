using Kappa.BackEnd.Server.Assets.Model;
using Kappa.Settings;
using MFroehlich.League.Assets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Patcher;
using Kappa.Riot.Domain.JSON.lol_game_data;
using MFroehlich.Parsing.JSON;
using MFroehlich.Parsing.JSON.Dynamic;

namespace Kappa.BackEnd.Server.Assets {
    [Docs("group", "Assets")]
    public class AssetsService : JSONService {
        private string loginVideo;
        private string loginImage;
        private Settings settings;

        private Task<WADArchive> lolGameData;

        private Task data;

        internal RuneSlots RunesInfo { get; private set; }
        internal MasteriesInfo Masteries { get; private set; }
        internal List<RuneDetails> Runes { get; private set; }
        internal List<ItemDetails> Items { get; private set; }

        internal List<MapSummary> Maps { get; private set; }
        internal List<WardSkinSummary> WardSkins { get; private set; }
        internal List<ChampionSummary> Champions { get; private set; }
        internal List<SummonerSpellDetails> SummonerSpells { get; private set; }

        public AssetsService(PatcherService patcher) : base("/assets") {
            loginVideo = GetStorage("login.webm");
            loginImage = GetStorage("login.png");
            settings = GetSettings<Settings>();

            lolGameData = patcher.PatchWAD("rcp-be-lol-game-data");
            Session.Init.ContinueWith(t => OnInitialized());

            #region Locales

            //var locales = new[] {
            //    "default",
            //    "en_US",
            //    "cs_CZ",
            //    "de_DE",
            //    "el_GR",
            //    "en_AU",
            //    "en_GB",
            //    "en_PH",
            //    "en_PL",
            //    "en_SG",
            //    "es_AR",
            //    "es_ES",
            //    "es_MX",
            //    "fr_FR",
            //    "hu_HU",
            //    "id_ID",
            //    "it_IT",
            //    "ja_JP",
            //    "ko_KR",
            //    "ms_MY",
            //    "pl_PL",
            //    "pt_BR",
            //    "ro_RO",
            //    "ru_RU",
            //    "th_TH",
            //    "tr_TR",
            //    "vn_VN",
            //    "zh_CN",
            //    "zh_MY",
            //    "zh_TW"
            //}.Select(l => l.ToLower()).ToArray();

            #endregion

            data = lolGameData.ContinueWith(t => {
                Masteries = ExtractJSON<MasteriesInfo>(t.Result, GameDataAssets.Masteries);
                Runes = ExtractJSON<Dictionary<string, RuneDetails>>(t.Result, GameDataAssets.Runes).Values.ToList();
                Items = ExtractJSON<List<ItemDetails>>(t.Result, GameDataAssets.Items);

                Maps = ExtractJSON<List<MapSummary>>(t.Result, GameDataAssets.Maps);
                WardSkins = ExtractJSON<List<WardSkinSummary>>(t.Result, GameDataAssets.WardSkins);
                Champions = ExtractJSON<List<ChampionSummary>>(t.Result, GameDataAssets.ChampionSummary);
                SummonerSpells = ExtractJSON<List<SummonerSpellDetails>>(t.Result, GameDataAssets.SummonerSpells);

                RunesInfo = ExtractJSON<RuneSlots>(t.Result, GameDataAssets.RuneSlot);

                //var all = new Dictionary<ulong, WADArchive.WADFile>(t.Result.AllFiles);
                //const string replace = "%locale%";

                //Func<string, bool, bool> test = (str, print) => {
                //    bool success = true;
                //    foreach (string locale in locales) {
                //        var tmp = str.Replace(replace, locale);
                //        var hash = WADArchive.Hash(tmp);

                //        if (!all.Remove(hash) && locale == GameDataAssets.DefaultLocale) {
                //            if (print) Debug.WriteLine(tmp);
                //            success = false;
                //        }
                //    }
                //    return success;
                //};

                //Func<string, string> get = str => {
                //    test(str, false);
                //    var tmp = str.Replace(replace, GameDataAssets.DefaultLocale);
                //    WADArchive.WADFile file;
                //    t.Result.TryGetFile(tmp, out file);
                //    using (var stream = t.Result.File.OpenRead())
                //    using (var zip = new GZipStream(stream, CompressionMode.Decompress))
                //    using (var mem = new MemoryStream()) {
                //        stream.Seek(file.Offset, SeekOrigin.Begin);
                //        zip.CopyToLength(mem, file.Size);
                //        return Encoding.UTF8.GetString(mem.ToArray());
                //    }
                //};

                //Func<string, object> getJSON = str => JSONParser.Parse(get(str));

                //test(GameDataAssets.SettingsToPersist(replace), true);
                //test(GameDataAssets.ChampionSummary(replace), true);
                //test(GameDataAssets.SummonerSpells(replace), true);
                //test(GameDataAssets.Masteries(replace), true);
                //test(GameDataAssets.RuneSlot(replace), true);
                //test(GameDataAssets.Runes(replace), true);
                //test(GameDataAssets.Items(replace), true);
                //test(GameDataAssets.Maps(replace), true);
                //test(GameDataAssets.WardSkins(replace), true);

                ////var assets = (JSONObject) getJSON(GameDataAssets.MapAssets(replace));
                ////foreach (var map in assets.Values) {
                ////    foreach (string asset in ((JSONObject) ((JSONObject) map)["assets"]).Values) {
                ////        var split = asset.Split('/');
                ////        test(GameDataAssets.MapAsset(split[4], split[5], split[6]), true);
                ////    }
                ////}

                //foreach (var champ in Champions) {
                //    var details = JSONDeserializer.Deserialize<ChampionDetails>(getJSON(GameDataAssets.ChampionDetails(champ.Id, replace)));

                //    foreach (var skin in details.Skins) {
                //        test(GameDataAssets.ChampionSplash(skin.Id, replace), true);
                //        test(GameDataAssets.ChampionTile(skin.Id, replace), true);
                //        test(GameDataAssets.ChampionCard(skin.Id, replace), true);

                //        if (skin.SplashVideoPath != null)
                //            test(GameDataAssets.SplashVideo(skin.Id, replace), true);

                //        if (skin.ChromaPath != null)
                //            test(GameDataAssets.ChromaImage(skin.Id, replace), true);

                //        if (skin.Chromas != null) {
                //            foreach (var chroma in skin.Chromas) {
                //                test(GameDataAssets.ChampionCard(chroma.Id, replace), true);
                //                test(GameDataAssets.ChromaImage(chroma.Id, replace), true);
                //            }
                //        }
                //    }

                //    test(GameDataAssets.SplashMetaData(champ.Id, replace), true);
                //    test(GameDataAssets.ChampionIcon(champ.Id, replace), true);
                //    test(GameDataAssets.ChampionSfx(champ.Id, replace), true);
                //    test(GameDataAssets.ChampionQuote(champ.Id, replace), true);
                //}

                //foreach (var spell in SummonerSpells) {
                //    test(GameDataAssets.SummonerSpellIcon(spell.Id, replace), true);
                //}

                //for (var i = 0; i < 10000; i++) {
                //    test(GameDataAssets.ProfileIcon(i, replace), false);
                //}

                //foreach (var mastery in Masteries.Data.Values) {
                //    test(GameDataAssets.MasteryIcon(mastery.Id, replace), true);
                //}

                //foreach (var item in Items) {
                //    test(GameDataAssets.ItemIcon(item.Id, replace), true);
                //}

                //foreach (var ward in WardSkins) {
                //    test(GameDataAssets.WardSkin(ward.Id, replace), true);
                //    test(GameDataAssets.WardSkinShadow(ward.Id, replace), true);
                //}

                //Debug.WriteLine(all.Count);

                //const string dst = @"C:\Users\max\desktop\tmp";
                //Directory.CreateDirectory(dst);
                //foreach (var file in all) {
                //    var files = Directory.EnumerateFiles(@"C:\Users\Max\Desktop\LCU\output.pbe\rcp-be-lol-game-data", $"*{file.Value.PathHash.ToString()}*").ToList();
                //    File.Copy(files.Single(), Path.Combine(dst, Path.GetFileName(files.Single())));
                //}
            });
        }

        private async void OnInitialized() {
            await UpdateLoginAnimation();
        }

        public override bool Handle(HttpListenerContext context) {
            if (base.Handle(context))
                return true;

            string path = context.Request.Url.LocalPath.Substring(BaseUrl.Length);

            switch (path) {
            case "/login/video":
                HandleFile(context, loginVideo);
                return true;
            case "/login/image":
                HandleFile(context, loginImage);
                return true;
            }

            #region lol-game-data

            if (!lolGameData.IsCompleted) {
                context.Response.StatusCode = 500;
                return true;
            }

            int index = path.EndOf("/game-data/");
            if (index < 0) return false;

            index = path.IndexOf("/", index, StringComparison.Ordinal);
            if (index < 0) return false;

            path = path.Substring(index);

            var split = path.Split(new[] { '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            int id;
            string url = null;
            if (path.StartsWith("/champion/icon/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ChampionIcon(id);
            }

            if (path.StartsWith("/champion/tile/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ChampionTile(id);
            }

            if (path.StartsWith("/champion/card/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ChampionCard(id);
            }

            if (path.StartsWith("/champion/splash/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ChampionSplash(id);
            }

            if (path.StartsWith("/champion/quote/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ChampionQuote(id);
            }

            if (path.StartsWith("/champion/sfx/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ChampionSfx(id);
            }

            if (path.StartsWith("/masteries/icon/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.MasteryIcon(id);
            }

            if (path.StartsWith("/items/icon/") && int.TryParse(split[2], out id)) {
                url = GameDataAssets.ItemIcon(id);
            }

            if (path.StartsWith("/profileicon/") && int.TryParse(split[1], out id)) {
                url = GameDataAssets.ProfileIcon(id);
            }

            if (path.StartsWith("/summonerspell/") && int.TryParse(split[1], out id)) {
                url = GameDataAssets.SummonerSpellIcon(id);
            }

            if (url == null) return false;
            WADArchive.WADFile file;
            if (!lolGameData.Result.TryGetFile(url, out file))
                return false;

            context.Response.Headers.Add("Cache-Control", "max-age=31536000, public");
            context.Response.ContentType = GetMimeType(context.Request.Url.LocalPath);

            using (var stream = lolGameData.Result.File.OpenRead())
                HandleStream(context, stream, file.Offset, file.Size);

            #endregion

            return false;
        }

        [Endpoint("/info")]
        public AssetsInfo GetInfo() {
            return new AssetsInfo(DataDragon.LatestVersion, Region.Locale);
        }

        #region lol-game-data

        [Endpoint("/game-data/masteries")]
        public async Task<MasteriesInfo> GetMasteriesInfo() {
            await data;
            return Masteries;
        }

        [Endpoint("/game-data/runes")]
        public async Task<List<RuneDetails>> GetRuneDetails() {
            await data;
            return Runes;
        }

        [Endpoint("/game-data/runeslots")]
        public async Task<RuneSlots> GetRuneSlots() {
            await data;
            return RunesInfo;
        }

        [Endpoint("/game-data/items")]
        public async Task<List<ItemDetails>> GetItemDetails() {
            await data;
            return Items;
        }

        [Endpoint("/game-data/champions")]
        public async Task<List<ChampionSummary>> GetChampionSummaries() {
            await data;
            return Champions;
        }

        [Endpoint("/game-data/summonerspells")]
        public async Task<List<SummonerSpellDetails>> GetSummonerSpells() {
            await data;
            return SummonerSpells;
        }

        [Endpoint("/game-data/maps")]
        public async Task<List<MapSummary>> GetMaps(int id) {
            await data;
            return Maps;
        }

        [Endpoint("/game-data/wardskins")]
        public async Task<List<WardSkinSummary>> GetWardSkins(int id) {
            await data;
            return WardSkins;
        }

        [Endpoint("/game-data/champion")]
        public async Task<ChampionDetails> GetChampionDetails(int id) {
            return ExtractJSON<ChampionDetails>(await lolGameData, locale => GameDataAssets.ChampionDetails(id, locale));
        }

        #endregion

        private async Task UpdateLoginAnimation() {
            using (var web = new WebClient()) {
                var themeUrl = Session.Latest.GetFiles("theme.properties").Single();
                string props = await web.DownloadStringTaskAsync(themeUrl.Url);
                int start = props.IndexOf("themeConfig=", StringComparison.Ordinal) + 12;
                int end = props.IndexOf(",", start, StringComparison.Ordinal);
                string theme = props.Substring(start, end - start);

                if (theme != settings.LoginTheme || !File.Exists(loginVideo)) {
                    var image = Session.Latest.GetFiles($"lgn/themes/{theme}/cs_bg_champions.png").Single();
                    await web.DownloadFileTaskAsync(image.Url, loginImage);

                    string tmp = Path.GetTempFileName();
                    var videoUrl = Session.Latest.GetFiles(theme + "/flv/login-loop.flv").Single();

                    await web.DownloadFileTaskAsync(videoUrl.Url, tmp);
                    var ffmpeg = Process.Start(new ProcessStartInfo {
                        FileName = "ffmpeg.exe",
                        Arguments = $"-y -i \"{tmp}\" -vb 2000k \"{loginVideo}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    });

                    await ffmpeg.WaitForExitAsync();
                    File.Delete(tmp);

                    settings.LoginTheme = theme;
                    settings.Save();

                    Session.Log($"Login animation up to date ({theme})");
                }
            }
        }

        private static T ExtractJSON<T>(WADArchive archive, Func<string, string> path) {
            WADArchive.WADFile file;

            if (archive.TryGetFile(path(Region.Locale.ToLowerInvariant()), out file))
                return JSONDeserializer.Deserialize<T>(JSONParser.Parse(archive.Extract(file)));

            if (archive.TryGetFile(path(GameDataAssets.DefaultLocale), out file))
                return JSONDeserializer.Deserialize<T>(JSONParser.Parse(archive.Extract(file)));

            throw new KeyNotFoundException($"File {path(GameDataAssets.DefaultLocale)} not found");
        }

        private class Settings : BaseSettings {
            public string LoginTheme { get; set; }
        }
    }
}

