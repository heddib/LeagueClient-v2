using Kappa.BackEnd.Server.Assets.Model;
using Kappa.Settings;
using MFroehlich.League.Assets;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kappa.BackEnd.Server.Patcher;

namespace Kappa.BackEnd.Server.Assets {
    [Docs("group", "Assets")]
    public class AssetsService : JSONService {
        private string loginVideo;
        private string loginImage;
        private Settings settings;

        private Task<WADArchive> lolGameData;

        public AssetsService(PatcherService patcher) : base("/assets") {
            loginVideo = GetStorage("login.webm");
            loginImage = GetStorage("login.png");
            settings = GetSettings<Settings>();

            lolGameData = patcher.PatchWAD("rcp-be-lol-game-data");
            Session.Init.ContinueWith(t => OnInitialized());
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
                HandleFile(context, loginVideo);
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

            if (url == null) return false;
            var file = lolGameData.Result.GetFile(url);

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
                        FileName = @"ffmpeg.exe",
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

        private class Settings : BaseSettings {
            public string LoginTheme { get; set; }
        }
    }
}
