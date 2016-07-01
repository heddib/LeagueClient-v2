using System;
using Kappa.Riot.Domain.JSON;
using Kappa.BackEnd.Server.Authentication.Model;
using Kappa.Settings;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

            var req = WebRequest.Create(Region.Current.LoginQueueURL + "login-queue/rest/queue/authenticate");
            req.Method = "POST";

            string str;
            using (var outputStream = req.GetRequestStream()) {
                outputStream.Write(Encoding.ASCII.GetBytes(query), 0, Encoding.ASCII.GetByteCount(query));

                using (var res = await req.GetResponseAsync())
                using (var mem = new MemoryStream()) {
                    res.GetResponseStream().CopyTo(mem);
                    str = Encoding.UTF8.GetString(mem.ToArray());
                }
            }

            var json = JSONParser.ParseObject(str);
            var dto = JSONDeserializer.Deserialize<LoginQueueDto>(json);

            var result = new AuthResult(dto, user, pass);

            switch (result.Content.Status) {
            case "LOGIN":
                this.auth = result;

                if (save) {
                    if (!settings.Accounts.ContainsKey(user)) {
                        settings.Accounts[user] = new Account();
                    }

                    settings.Accounts[user].SetPassword(pass);
                    settings.Save();
                }
                break;

            case "QUEUE":
                Debug.WriteLine("QUEUE: " + result.Content.Backlog);
                break;
            }

            return result;
        }

        [Endpoint("/login")]
        public async Task<AccountState> Login() {
            await session.Login(auth);
            BackEndServer.Async("/kappa/defer/auth", new JSONObject());
            return new AccountState(auth);
        }

        private class LoginSettings : BaseSettings {
            public Dictionary<string, Account> Accounts { get; set; }
        }
    }
}
