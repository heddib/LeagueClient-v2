using Kappa.Riot.Domain;
using Kappa.BackEnd.Server.Summoner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner {
    [Docs("group", "Summoner")]
    public class SummonerService : JSONService {
        private Me me;

        private Session session;

        [Async("/me")]
        public event EventHandler<Me> Me;

        public SummonerService(Session session) : base("/summoner") {
            this.session = session;

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
        public async Task<PublicSummoner> GetSummoner(string name) {
            var summ = await this.session.SummonerService.GetSummonerByName(name);
            return summ;
        }
    }
}
