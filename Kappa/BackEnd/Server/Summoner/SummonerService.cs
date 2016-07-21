using Kappa.Riot.Domain;
using Kappa.BackEnd.Server.Summoner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mail;
using Kappa.BackEnd.Server.Authentication;
using Kappa.Settings;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner {
    [Docs("group", "Summoner")]
    public class SummonerService : JSONService {
        private Me me;

        private Session session;
        private SummonerSettings settings;

        [Async("/me")]
        public event EventHandler<Me> Me;

        public SummonerService(Session session) : base("/summoner") {
            this.session = session;
            this.settings = GetSettings<SummonerSettings>();

            var messages = new MessageConsumer(session);

            messages.Consume<StoreAccountBalanceNotification>(OnStoreAccountBalanceNotification);
        }

        public async Task<Me> Connect(LoginSession login) {
            var packet = await this.session.ClientFacadeService.GetLoginDataPacketForUser();

            me = new Me(packet, login.AccountSummary);
            Me?.Invoke(this, me);

            return me;
        }

        private bool OnStoreAccountBalanceNotification(StoreAccountBalanceNotification balance) {
            me.OnBalance(balance);
            Me?.Invoke(this, me);
            return true;
        }

        internal async Task<long> GetSummonerId(string name) {
            name = Minify(name);
            long id;
            if (settings.Summoners.TryGetValue(name, out id)) return id;

            var summ = await session.SummonerService.GetSummonerByName(name);
            settings.Summoners.Add(name, summ.SummonerId);
            return summ.SummonerId;
        }

        private static string Minify(string name) {
            return name.ToLowerInvariant().Replace(" ", "");
        }

        [Endpoint("/store")]
        public async Task<string> GetStore() {
            return await this.session.LoginService.GetStoreUrl();
        }

        [Endpoint("/icon")]
        public async Task<Dictionary<long, int>> GetIcons(long[] ids) {
            var raw = await this.session.SummonerService.GetSummonerIcons(ids);
            var json = JSONParser.ParseObject(raw);
            return json.ToDictionary(p => long.Parse(p.Key), p => (int) p.Value);
        }

        [Endpoint("/get")]
        public async Task<Model.SummonerSummary> GetSummoner(string name) {
            var summ = await this.session.SummonerService.GetSummonerByName(name);
            return new Model.SummonerSummary(summ);
        }

        [Endpoint("/details")]
        public async Task<Model.SummonerDetails> GetDetails(long account) {
            var summ = await this.session.SummonerService.GetAllPublicSummonerDataByAccount(account);
            return new SummonerDetails(summ);
        }

        [Endpoint("/kudos")]
        public async Task<SummonerKudos> GetKudos(long id) {
            var raw = await this.session.ClientFacadeService.GetKudosTotals(id);
            return new SummonerKudos {
                // the first one is unknown
                Friendlies = raw[1],
                Helpfuls = raw[2],
                Teamworks = raw[3],
                Honorables = raw[4]
            };
        }

        private class SummonerSettings : BaseSettings {
            public Dictionary<string, long> Summoners { get; set; } = new Dictionary<string, long>();
        }
    }
}