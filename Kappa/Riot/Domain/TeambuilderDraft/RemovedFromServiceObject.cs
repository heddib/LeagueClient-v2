using Kappa.Riot.Services.Lcds;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.TeambuilderDraft {
    [LcdsService(LcdsServiceNames.TeambuilderDraft, "removedFromServiceV1")]
    public class RemovedFromServiceObject : LcdsServiceObject {
        public RemovedFromService Content { get; }

        public RemovedFromServiceObject(string messageId, string status, object payload) : base(messageId, status, payload) {
            Content = JSONDeserializer.Deserialize<RemovedFromService>(payload);
        }
    }


    public class RemovedFromService : JSONSerializable {
        [JSONField("reason")]
        public string Reason { get; set; }
    }
}
