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
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += OnException;
#endif
            new LeagueClient().Start();
        }

        private static void OnException(object sender, UnhandledExceptionEventArgs args) {
            var dst = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "error.txt");
            File.WriteAllText(dst, args.ExceptionObject.ToString());
        }

        public LeagueClient() {
            JSONDeserializer.AddDeserializer<object>(Deserialize);
            JSONSerializer.AddSerializer(Serialize);

            BackEndServer.Initialize();
            Session.Initialize();

            BackEndServer.AddService(this.front);
        }

        private void Start() {
            front.Build(@"..\..\FrontEnd", @"C:\Users\max\desktop\assets");

            var kappa = new Session();
            front.Start(BackEndServer.HostName);
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
