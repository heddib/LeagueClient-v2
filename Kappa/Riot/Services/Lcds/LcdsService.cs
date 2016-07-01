using MFroehlich.Parsing.JSON;
using System;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services.Lcds {
    internal abstract class LcdsService : IRequirable {
        protected abstract string Destination { get; }

        private LcdsProxyService proxy;
        private Session session;

        protected LcdsService(Session session, LcdsProxyService proxy) {
            this.session = session;
            this.proxy = proxy;
        }

        protected async Task<LcdsServiceObject> Invoke(string method, JSONValue args) {
            string str = JSON.Stringify(args);
            var id = Guid.NewGuid();
            var task = proxy.CallLcds(id.ToString(), Destination, method, str);

            Predicate<LcdsServiceObject> filter = lcds => lcds.MessageId == id.ToString();
            //var t1 = session.Peek(filter);
            var t2 = session.Peek(filter);

            await task;
            //var ack = await t1;
            var res = await t2;

            Session.RtmpLogLcds(Destination, method, str, JSON.Stringify(res.Payload));

            return res;
        }
    }
}
