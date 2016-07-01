using MFroehlich.Parsing.JSON;
using System.Net;

namespace Kappa.BackEnd.Server.Diagnostics {
    class LogService : HttpService {
        public LogService() : base("/logs") { }

        public override bool Handle(HttpListenerContext context) {
            if (context.Request.Url.LocalPath == "/logs") {

                context.Response.ContentType = "application/json";
                var docs = new JSONObject {
                    ["logs"] = new JSONArray(BackEndServer.Logs)
                };
                var bytes = docs.ToJSON().GetBytes();
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }
    }
}
