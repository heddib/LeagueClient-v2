using MFroehlich.Parsing.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Kappa.Settings;

namespace Kappa.BackEnd.Server {
    public abstract class JSONService : HttpService {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private const string Prefix = "/kappa";

        private readonly Dictionary<string, Delegate> endpoints = new Dictionary<string, Delegate>();

        public IReadOnlyDictionary<string, Delegate> Endpoints => endpoints;

        protected JSONService(string baseUrl) : base(Prefix + baseUrl) {
            foreach (var method in GetType().GetMethods(Flags)) {
                var att = method.GetCustomAttribute<EndpointAttribute>();
                if (att == null) continue;

                var parms = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var type = method.ReturnType == typeof(void) ? Expression.GetActionType(parms) : Expression.GetFuncType(parms.Extend(method.ReturnType));

                string url = BaseUrl + att.EndpointPath;
                endpoints.Add(url, method.CreateDelegate(type, this));
            }

            foreach (var evnt in GetType().GetEvents(Flags)) {
                var att = evnt.GetCustomAttribute<AsyncAttribute>();
                if (att != null) {
                    evnt.AddEventHandler(this, CreateAsync(this.BaseUrl + att.AsyncPath, evnt));
                }
            }

            BackEndServer.AddService(this);
        }

        public override bool Handle(HttpListenerContext context) {
            Delegate endpoint;

            if (endpoints.TryGetValue(context.Request.Url.LocalPath, out endpoint)) {
                JSONObject result;

                #region Dispatch
                try {
                    var rawArgs = new JSONArray();
                    if (context.Request.ContentLength64 > 0) {
                        string str;
                        using (var mem = new MemoryStream((int) context.Request.ContentLength64)) {
                            context.Request.InputStream.CopyTo(mem);
                            str = Encoding.UTF8.GetString(mem.ToArray());
                        }
                        rawArgs = JSONParser.ParseArray(str);
                    }

                    var types = endpoint.Method.GetParameters();
                    var changed = new object[rawArgs.Count];
                    for (var i = 0; i < rawArgs.Count; i++)
                        changed[i] = JSONDeserializer.Deserialize(types[i].ParameterType, rawArgs[i]);
                    var raw = endpoint.DynamicInvoke(changed);

                    result = GetResult(raw);
                } catch (Exception x) {
                    while (x.InnerException != null) x = x.InnerException;
                    result = new JSONObject {
                        ["error"] = new JSONObject {
                            ["id"] = 2,
                            ["trace"] = x.StackTrace,
                            ["message"] = x.Message,
                            ["exception"] = x.GetType().Name,
                        }
                    };
                }
                #endregion

                var bytes = Encoding.UTF8.GetBytes(result.ToJSON());
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }

        protected string GetStorage(params string[] path) {
            var arr = new string[path.Length + 3];
            arr[0] = Session.AppData;
            arr[1] = "storage";
            arr[2] = BaseUrl.Substring(Prefix.Length).Replace('/', '.').Substring(1);
            path.CopyTo(arr, 3);
            return Path.Combine(arr);
        }

        protected T GetSettings<T>() where T : BaseSettings, new() {
            var file = GetStorage("settings.json");
            return BaseSettings.Create<T>(file);
        }

        private static Delegate CreateAsync(string url, EventInfo evnt) {
            var parms = evnt.EventHandlerType.GetMethod("Invoke").GetParameters().Select(p => p.ParameterType).ToArray();
            var method = new DynamicMethod(string.Empty, null, parms, typeof(JSONService));
            var async = typeof(JSONService).GetMethod(nameof(Async), BindingFlags.NonPublic | BindingFlags.Static);
            var gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldstr, url);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Call, async);
            gen.Emit(OpCodes.Ret);
            return method.CreateDelegate(evnt.EventHandlerType);
        }

        private static void Async(string source, object arg) {
            if (arg?.GetType() == typeof(EventArgs))
                arg = new JSONObject();
            BackEndServer.Async(source, GetResult(arg));
        }

        private static JSONObject GetResult(dynamic tmp) {
            var result = new JSONObject();
            while (true) {
                if (tmp is Task) {
                    tmp.Wait();
                    try { tmp = tmp.Result; } catch { tmp = null; }
                }
                else {
                    result["value"] = JSONSerializer.Serialize(tmp);
                    break;
                }
            }
            return result;
        }
    }

    public class EndpointAttribute : Attribute {
        public string EndpointPath { get; }
        public EndpointAttribute(string name) {
            EndpointPath = name;
        }
    }

    public class AsyncAttribute : Attribute {
        public string AsyncPath { get; }
        public AsyncAttribute(string name) {
            AsyncPath = name;
        }
    }
}
