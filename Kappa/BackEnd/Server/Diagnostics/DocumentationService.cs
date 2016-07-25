using System;
using MFroehlich.Parsing.JSON;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Kappa.BackEnd.Server.Diagnostics {
    class DocumentationService : HttpService {
        public DocumentationService() : base("/docs") { }

        public override bool Handle(HttpListenerContext context) {
            context.Response.ContentType = "application/json";
            var list = new JSONArray();
            var endpoints = from s in BackEndServer.Services
                            let j = s as JSONService
                            where j != null
                            from e in j.Endpoints
                            select e;

            foreach (var endpoint in endpoints) {
                var json = new JSONObject {
                    ["path"] = endpoint.Key.Substring("/kappa".Length),
                };
                var args = new JSONArray();

                if (endpoint.Value.Method != null) {
                    foreach (var att in endpoint.Value.Method.DeclaringType.GetCustomAttributes<DocsAttribute>()) {
                        json[att.Field] = JSONSerializer.Serialize(att.Value);
                    }
                    foreach (var att in endpoint.Value.Method.GetCustomAttributes<DocsAttribute>()) {
                        json[att.Field] = JSONSerializer.Serialize(att.Value);
                    }
                    var parms = endpoint.Value.Method.GetParameters();
                    foreach (var parm in parms) {
                        args.Add(new JSONObject {
                            ["name"] = parm.Name,
                            ["type"] = parm.ParameterType.Name,
                            ["stringify"] = ShouldStringify(parm.ParameterType)
                        });
                    }
                }

                json["args"] = args;
                list.Add(json);
            }
            var docs = new JSONObject {
                ["name"] = "Kappa Backend",
                ["host"] = BackEndServer.HostName,
                ["base"] = "/kappa",
                ["endpoints"] = list,
            };
            var bytes = docs.ToJSON().GetBytes();
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            return true;
        }

        private static bool ShouldStringify(Type type) {
            return type == typeof(string) || type.IsEnum;
        }
    }
}
