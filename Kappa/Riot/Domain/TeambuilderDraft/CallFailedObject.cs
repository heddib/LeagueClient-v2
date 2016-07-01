using Kappa.Riot.Services.Lcds;

namespace Kappa.Riot.Domain.TeambuilderDraft {
    [LcdsService(LcdsServiceNames.TeambuilderDraft, "callFailedV1")]
    public class CallFailedObject : LcdsServiceObject {
        public CallFailedObject(string messageId, string status, object payload) : base(messageId, status, payload) {

        }
    }
}
