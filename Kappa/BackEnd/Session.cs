using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using Kappa.BackEnd.Server.Authentication.Model;
using Kappa.BackEnd.Server.Chat;
using Kappa.BackEnd.Server.Collection;
using Kappa.BackEnd.Server.Game;
using Kappa.BackEnd.Server.Meta;
using Kappa.BackEnd.Server.Summoner.Model;
using Kappa.Riot.Domain;
using Kappa.Riot.Services.Lcds;
using MFroehlich.League.RiotAPI;
using MFroehlich.Parsing.JSON;
using RtmpSharp.IO;
using RtmpSharp.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kappa.BackEnd.Server;
using Kappa.BackEnd.Server.Assets;
using Kappa.BackEnd.Server.Authentication;
using Kappa.BackEnd.Server.Diagnostics;
using Kappa.BackEnd.Server.Patcher;
using Kappa.BackEnd.Server.Replay;
using Kappa.Riot.Domain.JSON;
using Kappa.Riot.Services.Http;
using RtmpSharp.Net;
using SummonerService = Kappa.BackEnd.Server.Summoner.SummonerService;

namespace Kappa.BackEnd {
    public class Session {
        public static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Kappa");

        public static string RiotGamesDirectory { get; } = @"C:\Riot Games\" + (Region.Current == Region.PBE ? "PBE" : "League of Legends");
        public static RiotVersion Latest { get; private set; }

        private static string clientVersion;
        private static string rtmpLogFile;
        private static string logFile;

        internal static RiotAPI RiotAPI { get; private set; }
        internal event EventHandler Authed;
        internal event EventHandler<HandledEventArgs<object>> InternalMessageReceived;

        internal static Task Init { get; private set; }
        internal LoginQueueDto LoginQueue { get; private set; }
        internal Maestro Maestro { get; private set; }
        internal Me Me { get; private set; }
        internal AssetsService Assets { get; }

        private List<IMessageBlock> blocks = new List<IMessageBlock>();
        private RtmpClient rtmp;

        private LoginSession loginSession;

        private SummonerService summoner;
        private ChatService chat;

        public Session() {
            AccountService = new Riot.Services.AccountService(this);
            ChampionTradeService = new Riot.Services.ChampionTradeService(this);
            ClientFacadeService = new Riot.Services.ClientFacadeService(this);
            GameInvitationService = new Riot.Services.GameInvitationService(this);
            GameService = new Riot.Services.GameService(this);
            InventoryService = new Riot.Services.InventoryService(this);
            LcdsProxyService = new Riot.Services.LcdsProxyService(this);
            LeaguesService = new Riot.Services.LeaguesService(this);
            LoginService = new Riot.Services.LoginService(this);
            MasteryBookService = new Riot.Services.MasteryBookService(this);
            MatchmakingService = new Riot.Services.MatchmakingService(this);
            PlayerStatsService = new Riot.Services.PlayerStatsService(this);
            RerollService = new Riot.Services.RerollService(this);
            SpellBookService = new Riot.Services.SpellBookService(this);
            SummonerIconService = new Riot.Services.SummonerIconService(this);
            SummonerRuneService = new Riot.Services.SummonerRuneService(this);
            SummonerService = new Riot.Services.SummonerService(this);
            SummonerTeamService = new Riot.Services.SummonerTeamService(this);

            LootService = new LootService(this, LcdsProxyService);
            ChampionMasteryService = new ChampionMasteryService(this, LcdsProxyService);
            TeambuilderDraftService = new TeambuilderDraftService(this, LcdsProxyService);

            PlayerPreferencesService = new PlayerPreferencesService(this);
            MatchHistoryService = new MatchHistoryService(this);

            var patcher = new PatcherService(this);
            this.chat = new ChatService(this);

            this.Maestro = new Maestro(chat, patcher);

            var settings = new SettingsService(this);

            var hextech = new HextechService(this);
            var champions = new ChampionsService(this);
            var masteries = new MasteriesService(this);
            var runes = new RunesService(this);
            var skins = new SkinsService(this);

            var matches = new Server.Profile.MatchHistoryService(this);

            this.summoner = new SummonerService(this);
            this.Assets = new AssetsService(patcher);

            var rooms = new ChatRoomService(this, chat);
            var login = new AuthService(this);

            var game = new PlayLoopService(this, rooms);
            var invite = new InviteService(this, game);

            var meta = new MetaService(this);
            var debug = new DebugService(this);

            var replay = new ReplayService(this);

            patcher.FinishWAD();

            var info = new InfoService(this);
        }

        internal Riot.Services.AccountService AccountService { get; }
        internal Riot.Services.ChampionTradeService ChampionTradeService { get; }
        internal Riot.Services.ClientFacadeService ClientFacadeService { get; }
        internal Riot.Services.GameInvitationService GameInvitationService { get; }
        internal Riot.Services.GameService GameService { get; }
        internal Riot.Services.InventoryService InventoryService { get; }
        internal Riot.Services.LcdsProxyService LcdsProxyService { get; }
        internal Riot.Services.LeaguesService LeaguesService { get; }
        internal Riot.Services.LoginService LoginService { get; }
        internal Riot.Services.MasteryBookService MasteryBookService { get; }
        internal Riot.Services.MatchmakingService MatchmakingService { get; }
        internal Riot.Services.PlayerStatsService PlayerStatsService { get; }
        internal Riot.Services.RerollService RerollService { get; }
        internal Riot.Services.SpellBookService SpellBookService { get; }
        internal Riot.Services.SummonerIconService SummonerIconService { get; }
        internal Riot.Services.SummonerRuneService SummonerRuneService { get; }
        internal Riot.Services.SummonerService SummonerService { get; }
        internal Riot.Services.SummonerTeamService SummonerTeamService { get; }

        internal ChampionMasteryService ChampionMasteryService { get; }
        internal TeambuilderDraftService TeambuilderDraftService { get; }
        internal LootService LootService { get; }

        internal PlayerPreferencesService PlayerPreferencesService { get; }
        internal MatchHistoryService MatchHistoryService { get; }

        #region | Instance Methods |

        public async Task Start() {
            BackEndServer.Start();
            BackEndServer.MainThread.Join();

            if (rtmp != null) {
                try {
                    await Logout();
                } catch (ThreadAbortException) { }
            }
        }

        public Task<T> Invoke<T>(string service, string method, object[] args) {
            try {
                return rtmp.InvokeAsync<T>("my-rtmps", service, method, args);
            } catch (InvocationException x) {
                Log(x);
                throw;
            }
        }

        internal async Task<T> Peek<T>(Predicate<T> filter = null) where T : class {
            var block = new MessageBlock<T>(filter);
            blocks.Add(block);

            var t = await block.Get();

            return t;
        }

        internal async Task Login(AuthResult auth) {
            if (!Init.IsCompleted) await Init;
            if (auth.Content.Status != "LOGIN") throw new ArgumentException("Invalid auth credentials");

            var context = Riot.Services.Service.RegisterObjects();
            rtmp = new RtmpClient(new Uri("rtmps://" + Region.Current.MainServer + ":2099"), context, ObjectEncoding.Amf3);
            rtmp.MessageReceived += Rtmp_MessageReceived;
            await rtmp.ConnectAsync();

            chat.Connect(auth);

            LoginSession session = null;
            do {
                try {
                    var creds = auth.GetAuthCredentials();
                    creds.ClientVersion = clientVersion;
                    session = await LoginService.Login(creds);
                } catch (InvocationException x) when (x.RootCause is ClientVersionMismatchException) {
                    var inner = (ClientVersionMismatchException) x.RootCause;
                    clientVersion = (string) inner.SubstitutionArguments[1];
                }
            } while (session == null);

            string bc = $"bc-{session.AccountSummary.AccountId}";
            string gn = $"gn-{session.AccountSummary.AccountId}";
            string cn = $"cn-{session.AccountSummary.AccountId}";
            await Task.WhenAll(
                rtmp.SubscribeAsync("my-rtmps", "messagingDestination", "bc", bc),
                rtmp.SubscribeAsync("my-rtmps", "messagingDestination", gn, gn),
                rtmp.SubscribeAsync("my-rtmps", "messagingDestination", cn, cn)
            );

            await rtmp.LoginAsync(auth.Username.ToLower(), session.Token);
            string state = await AccountService.GetAccountState();

            if (state != "ENABLED") {
                Log(state);
                throw new AuthenticationException("Not Enabled: " + state);
            }

            this.LoginQueue = auth.Content;
            this.loginSession = session;
            this.Me = await summoner.Connect(session);

            try {
                Authed?.Invoke(this, new EventArgs());
            } catch (Exception x) {
                Log("Caught exception while dispatching auth: " + x);
            }

            BackEndServer.Async("/kappa/defer/auth", new JSONObject());

            new Thread(HeartBeatLoop) { IsBackground = true }.Start();
        }

        internal async Task Logout() {
            var service = new Riot.Services.LoginService(this);
            await service.Logout(loginSession.Token);
            await rtmp.LogoutAsync();
            rtmp.Close();
        }

        internal void Exit() {
            BackEndServer.Stop();
        }

        internal void HandleMessage(object msg) {
            if (msg is LcdsServiceProxyResponse) {
                msg = LcdsServiceObject.GetObject((LcdsServiceProxyResponse) msg);
            }

            var block = blocks.FirstOrDefault(b => b.Matches(msg));

            if (block != null) {
                blocks.Remove(block);
                block.Supply(msg);
                return;
            }

            RtmpLogAsync(msg);

            if (msg is SimpleDialogMessage) {
                Log("Simple dialog message: ");
                Log("  " + JSON.Stringify(JSONSerializer.Serialize(msg), 2, 2));
            }

            var args = new HandledEventArgs<object>(msg);
            InternalMessageReceived?.Invoke(this, args);
            if (args.Handled) return;

            Log("Unhandled message: " + msg.GetType());
        }

        private void HeartBeatLoop() {
            var loginService = new Riot.Services.LoginService(this);
            int i = 1;
            while (!rtmp.IsDisconnected) {
                loginService.PerformLcdsHeartBeat(loginSession.AccountSummary.AccountId, loginSession.Token, i, DateTime.Now.ToString(CultureInfo.InvariantCulture));
                i++;
                Thread.Sleep(60 * 1000);
            }
        }

        private void Rtmp_MessageReceived(object sender, MessageReceivedEventArgs e) {
            HandleMessage(e.Body);
        }

        #endregion

        #region | Static Methods |

        public static void Initialize() {
            logFile = Path.Combine(AppData, "logs", "client", DateTime.Now.ToString("M-d H-mm") + ".txt");
            Directory.CreateDirectory(Path.GetDirectoryName(logFile));
#if DEBUG
            rtmpLogFile = Path.Combine(AppData, "logs", "rtmp", DateTime.Now.ToString("M-d H-mm") + ".txt");
            Directory.CreateDirectory(Path.GetDirectoryName(rtmpLogFile));
#endif

            RiotAPI = RiotAPI.Debug(MFroehlich.League.RiotAPI.Region.NA, "5d8ea4aa-db1c-43ea-aacd-888129dadf11");

            Log("Patch: " + (clientVersion = MFroehlich.League.Assets.DataDragon.CurrentVersion));

            //Static init//
            Init = Task.Run(async () => {
                Latest = await RiotVersion.GetLatestVersion();

                Log($"Air: {Latest.AirVersion}");
                Log($"Game: {Latest.GameVersion}");
                Log($"Solution: {Latest.SolutionVersion}");
            });
        }

        internal static void Log(object msg) {
            Debug.WriteLine(msg);

            var str = (msg?.ToString() ?? "null") + Environment.NewLine;
            File.AppendAllText(logFile, str);
        }

        [Conditional("DEBUG")]
        internal static void RtmpLogAsync(object msg) {
            object json;
            var lcds = msg as LcdsServiceObject;
            if (lcds != null) {
                json = lcds.Payload;

                var att = lcds.GetType().GetCustomAttribute<LcdsServiceAttribute>();
                BackEndServer.Log("Async", att.Service + "." + att.Method, new JSONObject {
                    ["type"] = "async_proxy",
                    ["body"] = lcds.Payload
                });
            }
            else {
                json = JSONSerializer.Serialize(msg);

                BackEndServer.Log("Async", msg.GetType().Name, new JSONObject {
                    ["type"] = "async",
                    ["body"] = json
                });
            }

            var str = new StringBuilder();
            str.AppendLine("Async:");
            str.AppendLine("  " + JSON.Stringify(json));
            str.AppendLine();
            File.AppendAllText(rtmpLogFile, str.ToString());
        }

        [Conditional("DEBUG")]
        internal static void RtmpLogInvoke(string service, string method, object[] args, object response) {
            if (service == "lcdsServiceProxy" && method == "call") return;//skip LCDS invocations
            BackEndServer.Log("Invoke", service + "." + method, new JSONObject {
                ["type"] = "invoke",
                ["args"] = JSONSerializer.Serialize(args),
                ["return"] = JSONSerializer.Serialize(response)
            });

            var str = new StringBuilder();
            str.AppendLine($"Invoke: [{service}.{method}]");
            foreach (var arg in args) {
                var json = JSONSerializer.Serialize(arg);
                str.AppendLine("  " + JSON.Stringify(json));
            }
            str.AppendLine("  " + JSON.Stringify(JSONSerializer.Serialize(response)));
            str.AppendLine();
            File.AppendAllText(rtmpLogFile, str.ToString());
        }

        [Conditional("DEBUG")]
        internal static void RtmpLogLcds(string service, string method, string args, string payload) {
            BackEndServer.Log("Invoke", service + "." + method, new JSONObject {
                ["type"] = "invoke_proxy",
                ["args"] = JSONParser.Parse(args),
                ["return"] = JSONParser.Parse(payload ?? "{}")
            });

            var str = new StringBuilder();
            str.AppendLine($"Invoke LCDS: [{service}.{method}]");
            str.AppendLine("  " + args);
            str.AppendLine("  " + payload);
            str.AppendLine();
            File.AppendAllText(rtmpLogFile, str.ToString());
        }

        #endregion

        private class MessageBlock<T> : IMessageBlock {
            private readonly Predicate<T> filter;
            private readonly TaskCompletionSource<T> tcs;

            public MessageBlock(Predicate<T> filter) {
                this.filter = filter;
                this.tcs = new TaskCompletionSource<T>();
            }

            public async Task<T> Get() {
                return await tcs.Task;
            }

            public void Supply(object arg) {
                if (!(arg is T)) throw new ArgumentException(arg + " is of wrong type");
                else tcs.SetResult((T) arg);
            }

            public bool Matches(object arg) {
                return arg is T && (filter == null || filter((T) arg));
            }
        }

        private interface IMessageBlock {
            void Supply(object arg);
            bool Matches(object arg);
        }
    }
}
