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
using LeagueSharp;
using LeagueSharp.Archives;
using LeagueSharp.RADS;
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

            data = lolGameData.ContinueWith(t => {
                Masteries = ExtractJSON<MasteriesInfo>(t.Result, GameDataAssets.Masteries);
                Runes = ExtractJSON<Dictionary<string, RuneDetails>>(t.Result, GameDataAssets.Runes).Values.ToList();
                Items = ExtractJSON<List<ItemDetails>>(t.Result, GameDataAssets.Items);

                Maps = ExtractJSON<List<MapSummary>>(t.Result, GameDataAssets.Maps);
                WardSkins = ExtractJSON<List<WardSkinSummary>>(t.Result, GameDataAssets.WardSkins);
                Champions = ExtractJSON<List<ChampionSummary>>(t.Result, GameDataAssets.ChampionSummary);
                SummonerSpells = ExtractJSON<List<SummonerSpellDetails>>(t.Result, GameDataAssets.SummonerSpells);

                RunesInfo = ExtractJSON<RuneSlots>(t.Result, GameDataAssets.RuneSlot);
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
            WADArchive.FileInfo fileInfo;
            if (!lolGameData.Result.TryGetFile(url, out fileInfo))
                return false;

            context.Response.Headers.Add("Cache-Control", "max-age=31536000, public");
            context.Response.ContentType = GetMimeType(context.Request.Url.LocalPath);

            using (var stream = lolGameData.Result.File.OpenRead())
                HandleStream(context, stream, fileInfo.Offset, fileInfo.Size);

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
        public async Task<List<MapSummary>> GetMaps() {
            await data;
            return Maps;
        }

        [Endpoint("/game-data/wardskins")]
        public async Task<List<WardSkinSummary>> GetWardSkins() {
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
            WADArchive.FileInfo fileInfo;

            if (archive.TryGetFile(path(Region.Locale.ToLowerInvariant()), out fileInfo))
                return JSONDeserializer.Deserialize<T>(JSONParser.Parse(archive.Extract(fileInfo)));

            if (archive.TryGetFile(path(GameDataAssets.DefaultLocale), out fileInfo))
                return JSONDeserializer.Deserialize<T>(JSONParser.Parse(archive.Extract(fileInfo)));

            throw new KeyNotFoundException($"File {path(GameDataAssets.DefaultLocale)} not found");
        }

        private class Settings : BaseSettings {
            public string LoginTheme { get; set; }
        }
    }
}

