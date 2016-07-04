using System;
using Kappa.Riot.Domain.JSON;
using Kappa.BackEnd.Server.Authentication.Model;
using Kappa.Settings;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Kappa.Util;

namespace Kappa.BackEnd.Server.Authentication {
    [Docs("group", "Authentication")]
    public class AuthService : JSONService {
        private LoginSettings settings;
        private AuthResult auth;
        private Session session;

        public AuthService(Session session) : base("/login") {
            this.session = session;
            this.session.Authed += Session_Authed;
            settings = GetSettings<LoginSettings>();
        }

        private void Session_Authed(object sender, EventArgs e) {
            var user = settings.Accounts[session.Me.Username];
            user.ProfileIcon = session.Me.ProfileIcon;
            user.SummonerName = session.Me.Name;
            settings.Save();
        }

        [Endpoint("/saved")]
        public List<SavedAccount> GetSavedAccounts() {
            if (settings.Accounts == null) settings.Accounts = new Dictionary<string, Account>();
            return (from pair in settings.Accounts
                    select new SavedAccount(pair.Key, pair.Value)).ToList();
        }

        [Endpoint("/load")]
        public async Task<AuthResult> AuthenticateSaved(string user) {
            var src = settings.Accounts[user];
            return await Authenticate(user, src.GetPassword(), true);
        }

        [Endpoint("/auth")]
        public async Task<AuthResult> Authenticate(string user, string pass, bool save) {
            string payload = "user=" + user + ",password=" + pass;
            string query = "payload=" + payload;

            var url = Region.Current.LoginQueueURL + "login-queue/rest/queue/authenticate";
            var json = await QuickHttp.Request("POST", url).Send(query).JSONObject();

            this.auth = new AuthResult(JSONDeserializer.Deserialize<LoginQueueDto>(json), user, pass);

            if (json.ContainsKey("lqt") || this.auth.Content.Token == null) {
                Debug.WriteLine(json.ToJSON());
            }

            switch (this.auth.Content.Status) {
            case "LOGIN":
                if (save) {
                    if (!settings.Accounts.ContainsKey(user)) {
                        settings.Accounts[user] = new Account();
                    }

                    settings.Accounts[user].SetPassword(pass);
                    settings.Save();
                }
                break;

            case "QUEUE":
                Debug.WriteLine("QUEUE: " + this.auth.Content.Backlog);
                break;
            }

            return this.auth;
        }

        //[Endpoint("/queue")]
        //public async Task<QueuePosition> Queue() {
        //    var url = Region.Current.LoginQueueURL + $"login-queue/rest/queues/lol/ticker/{auth.Content.Champ}";
        //    var json = await QuickHttp.Request("GET", url).JSONObject();
        //    var raw = (string) json[auth.Content.Node.ToString()];
        //    var num = int.Parse(raw, NumberStyles.HexNumber);

        //    var index = Math.Max(0, auth.Content.Node - num);

        //    if (index == 0) {
        //        var tokenUrl = Region.Current.LoginQueueURL + "login-queue/rest/queue/token";
        //        var tokenJson = await QuickHttp.Request("POST", tokenUrl).Send("").String();
        //        Debug.WriteLine(tokenJson);
        //    }

        //    return new QueuePosition { Position = index };
        //}

        [Endpoint("/login")]
        public async Task<AccountState> Login() {
            for (var i = 0; i < 5; i++) {
                try {
                    await session.Login(auth);
                    break;
                } catch (ArgumentException) {
                    if (i == 4) return null;
                }
            }
            BackEndServer.Async("/kappa/defer/auth", new JSONObject());
            return new AccountState(auth);
        }

        private class LoginSettings : BaseSettings {
            public Dictionary<string, Account> Accounts { get; set; }
        }
    }
}
