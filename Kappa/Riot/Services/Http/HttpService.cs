using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Kappa.BackEnd;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Services.Http {
    internal abstract class HttpService {
        private Session session;
        private Uri baseUri;

        protected HttpService(Session session, string baseUri) {
            this.session = session;
            this.baseUri = new Uri(baseUri);
        }

        protected async Task<T> Get<T>(string path) {
            var req = WebRequest.CreateHttp(new Uri(baseUri, path));
            req.Headers.Add("Authorization", "JwtBearer " + session.LoginQueue.IdToken);

            using (var res = await req.GetResponseAsync()) {
                var t = (T) JSONParser2.Parse(res.GetResponseStream());
                return t;
            }
        }

        protected async Task Put(string path, byte[] data, Dictionary<string, string> query = null) {
            var req = WebRequest.CreateHttp(new Uri(baseUri, path));
            req.Method = "PUT";
            req.GetRequestStream().Write(data, 0, data.Length);
            req.Headers.Add("Authorization", "JwtBearer " + session.LoginQueue.IdToken);

            using (await req.GetResponseAsync()) { }
        }
    }
}
