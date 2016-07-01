using System.Collections.Generic;
using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.Loot {
    [LcdsService(LcdsServiceNames.Loot, "getAllQueries")]
    public class QueryListObject : LcdsServiceObject {
        public QueryListDTO Content { get; }

        public QueryListObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<QueryListDTO>(payload);
        }
    }

    public class QueryListDTO : JSONSerializable {
        [JSONField("queryToLootNames")]
        public Dictionary<string, List<string>> QueryToLootNames { get; set; }

        [JSONField("lastUpdate")]
        public long LastUpdate { get; set; }
    }
}
