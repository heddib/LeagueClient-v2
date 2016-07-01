using RtmpSharp.IO;
using System;

namespace Kappa.Riot.Domain {
    [Serializable]
    [SerializedName("com.riotgames.platform.serviceproxy.dispatch.LcdsServiceProxyResponse")]
    public class LcdsServiceProxyResponse {
        [SerializedName("status")]
        public string Status { get; set; }

        [SerializedName("compressedPayload")]
        public bool CompressedPayload { get; set; }

        [SerializedName("payload")]
        public string Payload { get; set; }

        [SerializedName("messageId")]
        public string MessageId { get; set; }

        [SerializedName("methodName")]
        public string MethodName { get; set; }

        [SerializedName("serviceName")]
        public string ServiceName { get; set; }
    }
}
