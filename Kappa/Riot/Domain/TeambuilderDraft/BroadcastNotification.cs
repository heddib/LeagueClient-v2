using System.Text;
using System.Collections.Generic;
using MFroehlich.Parsing.JSON;
using RtmpSharp.IO.AMF3;

namespace Kappa.Riot.Domain {
    public partial class BroadcastNotification : IExternalizable {
        public List<BroadcastMessage> BroadcastMessages { get; set; }

        public string Json { get; set; }

        public void ReadExternal(IDataInput input) {
            Json = input.ReadUtf((int) input.ReadUInt32());

            var json = JSONParser.ParseObject(Json);

            if (json.ContainsKey("broadcastMessages")) {
                BroadcastMessages = JSONDeserializer.Deserialize<List<BroadcastMessage>>(json["broadcastMessages"]);
            }
        }

        public void WriteExternal(IDataOutput output) {
            var bytes = Encoding.UTF8.GetBytes(Json);

            output.WriteInt32(bytes.Length);
            output.WriteBytes(bytes);
        }
    }

    public class BroadcastMessage : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }
        [JSONField("active")]
        public bool Active { get; set; }
        [JSONField("content")]
        public string Content { get; set; }
        [JSONField("messageKey")]
        public string MessageKey { get; set; }
        [JSONField("severity")]
        public string Severity { get; set; }
    }
}
