using RtmpSharp.IO.AMF3;
using System.Text;
using System.Reflection;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain {
    public partial class ClientSystemStatesNotification : JSONSerializable, IExternalizable {
        public string Json { get; set; }

        public void ReadExternal(IDataInput input) {
            Json = input.ReadUtf((int) input.ReadUInt32());

            var json = JSONParser.ParseObject(Json);
            var states = JSONDeserializer.Deserialize<ClientSystemStatesNotification>(json);
            foreach (PropertyInfo prop in typeof(ClientSystemStatesNotification).GetProperties()) {
                prop.SetValue(this, prop.GetValue(states));
            }
        }

        public void WriteExternal(IDataOutput output) {
            var bytes = Encoding.UTF8.GetBytes(Json);

            output.WriteInt32(bytes.Length);
            output.WriteBytes(bytes);
        }
    }
}