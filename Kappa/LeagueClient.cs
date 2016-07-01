using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Kappa.BackEnd;
using Kappa.BackEnd.Server;
using MFroehlich.Parsing.JSON;
using RtmpSharp.IO;

namespace Kappa {
    public class LeagueClient {
        private const string ContentFile = @"content.ext";
        private const string AssetsFile = @"assets.ext";

        private readonly FrontEnd front = new FrontEnd(ContentFile, AssetsFile);

        public static void Main(string[] args) {
#if DEBUG
            new LeagueClient().Start();
#else
            try {
                new LeagueClient().Start();
            } catch (Exception x) {
                File.WriteAllText(@"C:\Users\Max\Desktop\ex.txt", x.ToString());
            }
#endif
        }

        public LeagueClient() {
            BackEndServer.AddService(this.front);

            Session.Initialize();

            JSONSerializer.AddSerializer(Serialize);
            JSONDeserializer.AddDeserializer<object>(Deserialize);
        }

        private void Start() {
            front.Build(@"..\..\FrontEnd", @"C:\Users\max\desktop\assets");

            var kappa = new Session();
            front.Start();
            kappa.Start().Wait();
        }

        private static bool Deserialize(Type t, object value, out object result) {
            var att = t.GetCustomAttribute<SerializedNameAttribute>();
            if (att != null && !typeof(JSONSerializable).IsAssignableFrom(t)) {
                string _class;
                if (((JSONObject) value).TryGetValue("_class", out _class)) {
                    t = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(t2 => t2.GetCustomAttribute<SerializedNameAttribute>(false)?.SerializedName == _class);
                }
                result = Activator.CreateInstance(t);
                foreach (var member in Member.GetMembers(t)) {
                    var field = member.Value.GetCustomAttribute<SerializedNameAttribute>();
                    var name = field?.SerializedName ?? member.Name;
                    object v;
                    if (((JSONObject) value).TryGetValue(name, out v))
                        member.SetValue(result, JSONDeserializer.Deserialize(member.MemberType, v));
                }
                return true;
            }
            result = null;
            return false;
        }

        private static bool Serialize(object value, out object result) {
            var att = value.GetType().GetCustomAttribute<SerializedNameAttribute>(false);
            if (att != null && !(value is JSONSerializable) && !(value is IEnumerable)) {
                var json = new JSONObject {
                    ["_class"] = att.SerializedName
                };
                foreach (var member in Member.GetMembers(value.GetType())) {
                    var v = member.GetValue(value);
                    var field = member.Value.GetCustomAttribute<SerializedNameAttribute>();
                    if (field != null)
                        json[field.SerializedName] = JSONSerializer.Serialize(v);
                }
                result = json;
                return true;
            }
            result = null;
            return false;
        }
    }
}
