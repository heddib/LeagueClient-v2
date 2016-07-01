using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.Riot.Domain.TeambuilderDraft {
    [LcdsService(LcdsServiceNames.TeambuilderDraft, "gatekeeperRestrictedV2")]
    public class GatekeeperExceptionObject : LcdsServiceObject {
        public GatekeeperException Content { get; }

        public GatekeeperExceptionObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<GatekeeperException>(payload);
        }
    }

    public class GatekeeperException : JSONSerializable {
        [JSONField("gatekeeperRestrictions")]
        public List<Restriction> GatekeeperRestrictions { get; set; }
    }

    public class Restriction : JSONSerializable {
        [JSONField("reason")]
        public string Reason { get; set; }
        [JSONField("remainingMillis")]
        public long RemainingMillis { get; set; }
    }
}
