using Kappa.BackEnd.Server.Assets.Model;
using Kappa.Settings;
using MFroehlich.League.Assets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Patcher;
using Kappa.Riot.Domain.JSON.lol_game_data;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Assets {
    [Docs("group", "Assets")]
    public class AssetsService : JSONService {
        private string loginVideo;
        private string loginImage;
        private Settings settings;

        private Task<WADArchive> lolGameData;

        private Task data;

        internal RuneSlots RunesInfo { get; private set; }
        internal MasteriesInfo MasteriesInfo { get; private set; }
        internal List<RuneDetails> RuneDetails { get; private set; }
        internal List<ItemDetails> ItemDetails { get; private set; }


        internal List<ChampionSummary> ChampionSummaries { get; private set; }
        internal List<SummonerSpellDetails> SummonerSpells { get; private set; }

        public AssetsService(PatcherService patcher) : base("/assets") {
            loginVideo = GetStorage("login.webm");
            loginImage = GetStorage("login.png");
            settings = GetSettings<Settings>();

            lolGameData = patcher.PatchWAD("rcp-be-lol-game-data");
            Session.Init.ContinueWith(t => OnInitialized());

            //var locales = new[] { "default", "en_US", "cs_CZ", "de_DE", "el_GR", "en_AU", "en_GB", "en_PH", "en_PL", "en_SG", "es_AR", "es_ES", "es_MX", "fr_FR", "hu_HU", "id_ID", "it_IT", "ja_JP", "ko_KR", "ms_MY", "pl_PL", "pt_BR", "ro_RO", "ru_RU", "th_TH", "tr_TR", "vn_VN", "zh_CN", "zh_MY", "zh_TW"
            //}.Select(l => l.ToLower()).ToArray();

            data = lolGameData.ContinueWith(t => {
                MasteriesInfo = ExtractJSON<MasteriesInfo>(t.Result, GameDataAssets.Masteries);
                RuneDetails = ExtractJSON<Dictionary<string, RuneDetails>>(t.Result, GameDataAssets.Runes).Values.ToList();
                ItemDetails = ExtractJSON<List<ItemDetails>>(t.Result, GameDataAssets.Items);

                SummonerSpells = ExtractJSON<List<SummonerSpellDetails>>(t.Result, GameDataAssets.SummonerSpells);
                ChampionSummaries = ExtractJSON<List<ChampionSummary>>(t.Result, GameDataAssets.ChampionSummary);

                RunesInfo = ExtractJSON<RuneSlots>(t.Result, GameDataAssets.RuneSlot);
                /*
                                var all = new Dictionary<ulong, WADArchive.WADFile>(t.Result.AllFiles);
                                GameDataAssets.Locale = "%locale%";

                                Func<string, string> test = str => {
                                    foreach (string locale in locales) {
                                        var tmp = str.Replace(GameDataAssets.Locale, locale);
                                        var hash = WADArchive.Hash(tmp);

                                        if (!all.Remove(hash) && locale == "default")
                                            Debug.WriteLine(tmp);
                                    }
                                    return str.Replace(GameDataAssets.Locale, "default");
                                };

                                Func<string, string> get = str => {
                                    test(str);
                                    var tmp = str.Replace(GameDataAssets.Locale, "default");
                                    var file = t.Result.GetFile(tmp);
                                    using (var stream = t.Result.File.OpenRead())
                                    using (var zip = new GZipStream(stream, CompressionMode.Decompress))
                                    using (var mem = new MemoryStream()) {
                                        stream.Seek(file.Offset, SeekOrigin.Begin);
                                        zip.CopyToLength(mem, file.Size);
                                        return Encoding.UTF8.GetString(mem.ToArray());
                                    }
                                };

                                Func<string, object> getJSON = str => JSONParser.Parse(get(str));

                                var champs = JSONDeserializer.Deserialize<List<ChampionSummary>>(getJSON(GameDataAssets.ChampionSummary));
                                var runes = get(GameDataAssets.SummonerSpells);
                                var slots = get(GameDataAssets.Masteries);
                                var items = get(GameDataAssets.Items);
                                foreach (var champ in champs) {
                                    var details = JSONDeserializer.Deserialize<ChampionDetails>(getJSON(GameDataAssets.ChampionDetails(champ.Id)));

                                    foreach (var skin in details.Skins) {
                                        test(GameDataAssets.ChampionSplash(skin.Id));
                                        test(GameDataAssets.ChampionTile(skin.Id));
                                        test(GameDataAssets.ChampionCard(skin.Id));

                                        if (skin.SplashVideoPath != null)
                                            test(GameDataAssets.SplashVideo(skin.Id));

                                        if (skin.ChromaPath != null)
                                            test(GameDataAssets.ChromaImage(skin.Id));
                                    }

                                    test(GameDataAssets.ChampionIcon(champ.Id));
                                    test(GameDataAssets.ChampionSfx(champ.Id));
                                    test(GameDataAssets.ChampionQuote(champ.Id));
                                }

                                foreach (var spell in DataDragon.SpellData.Value.data.Values) {
                                    test(GameDataAssets.SummonerSpellIcon(spell.key));
                                }

                                foreach (var icon in DataDragon.IconData.Value.data.Values) {
                                    test(GameDataAssets.ProfileIcon(icon.id));
                                }

                                foreach (var mastery in DataDragon.MasteryData.Value.data.Values) {
                                    test(GameDataAssets.MasteryIcon(mastery.id));
                                }

                                foreach (var item in DataDragon.ItemData.Value.data.Values) {
                                    var id = int.Parse(item.image.full.Substring(0, item.image.full.IndexOf(".", StringComparison.Ordinal)));
                                    test(GameDataAssets.ItemIcon(id));
                                }

                                for (var i = 0; i < 58; i++) {
                                    test(GameDataAssets.WardSkin(i));
                                    test(GameDataAssets.WardSkinShadow(i));
                                }

                                Debug.WriteLine(all.Count);

                                foreach (var file in all) {
                                    var files = Directory.EnumerateFiles(@"C:\Users\Max\Desktop\LCU\output.pbe\rcp-be-lol-game-data", $"*{file.Value.PathHash.ToString()}*").ToList();
                                    File.Copy(files.Single(), Path.Combine(@"C:\Users\max\desktop\tmp", Path.GetFileName(files.Single())));
                                }*/
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

        [Endpoint("/masteries")]
        public async Task<MasteriesInfo> GetMasteriesInfo() {
            await data;
            return MasteriesInfo;
        }

        [Endpoint("/runes")]
        public async Task<List<RuneDetails>> GetRuneDetails() {
            await data;
            return RuneDetails;
        }

        [Endpoint("/runeslots")]
        public async Task<RuneSlots> GetRuneSlots() {
            await data;
            return RunesInfo;
        }

        [Endpoint("/items")]
        public async Task<List<ItemDetails>> GetItemDetails() {
            await data;
            return ItemDetails;
        }

        [Endpoint("/champions")]
        public async Task<List<ChampionSummary>> GetChampionSummaries() {
            await data;
            return ChampionSummaries;
        }

        [Endpoint("/summonerspells")]
        public async Task<List<SummonerSpellDetails>> GetSummonerSpells() {
            await data;
            return SummonerSpells;
        }

        [Endpoint("/champion")]
        public async Task<ChampionDetails> GetChampionDetails(int id) {
            return ExtractJSON<ChampionDetails>(await lolGameData, locale => GameDataAssets.ChampionDetails(id, locale));
        }

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
