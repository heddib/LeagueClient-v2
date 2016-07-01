using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd;
using MFroehlich.League.RiotAPI;
using MFroehlich.Parsing.JSON;
using Kappa.Riot.Domain.JSON;

namespace Kappa.Riot.Services.Http {
    public class MatchHistoryService {
        private Session session;

        public MatchHistoryService(Session session) {
            this.session = session;
        }
        public async Task<PlayerHistory> GetMatchHistory(long accountId) {
            return await FetchAsync<PlayerHistory>($"https://acs.leagueoflegends.com/v1/stats/player_history/{BackEnd.Region.Current.Platform}/{accountId}?begIndex=0&endIndex=20");
        }

        public async Task<MatchAPI.Timeline> GetMatchTimeline(long gameId) {
            return await FetchAsync<MatchAPI.Timeline>($"https://acs.leagueoflegends.com/v1/stats/game/{BackEnd.Region.Current.Platform}/{gameId}/timeline");
        }

        public async Task<MatchDetails> GetMatchDetails(long gameId) {
            return await FetchAsync<MatchDetails>($"https://acs.leagueoflegends.com/v1/stats/game/{BackEnd.Region.Current.Platform}/{gameId}");
        }

        public async Task<PlayerDeltas> GetDeltas() {
            return await FetchAsync<PlayerDeltas>("https://acs.leagueoflegends.com/v1/deltas/auth");
        }

        private async Task<T> FetchAsync<T>(string url) where T : new() {
            var req = System.Net.WebRequest.Create(url);
            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(session.LoginQueue.GasToken.ToJSON()));
            req.Headers.Add("region", BackEnd.Region.Current.Platform);
            req.Headers.Add("authorization", "GasTokenRaw " + b64);

            using (var res = await req.GetResponseAsync())
            using (var mem = new MemoryStream())
            using (var stream = res.GetResponseStream()) {
                stream.CopyTo(mem);
                return JSONDeserializer.Deserialize<T>(JSONParser.Parse(mem.ToArray()));
            }
        }
    }
}
