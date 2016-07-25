using System;
using System.Collections.Generic;
using System.Linq;
using MFroehlich.Parsing.JSON;
using System.Net;

namespace Kappa.BackEnd.Server.Diagnostics {
    [Docs("group", "Diagnostics")]
    public class LogService : JSONService {
        private WebSocket socket;
        private Dictionary<string, Log> logs = new Dictionary<string, Log>();

        public LogService() : base("/logs") { }

        internal Log CreateLog(string category) {
            var log = new Log(category);
            log.Written += LogOnWritten;
            logs.Add(log.Name, log);
            return log;
        }

        private async void LogOnWritten(object sender, LogItem logItem) {
            if (socket != null) {
                var json = new JSONObject {
                    ["name"] = ((Log) sender).Name,
                    ["entry"] = logItem
                };
                await socket.Send(json.ToJSON());
            }
        }

        public override bool Handle(HttpListenerContext context) {
            if (base.Handle(context)) return true;
            if (context.Request.IsWebSocketRequest) {
                ConnectSocket(context);
                return true;
            }

            return false;
        }

        private async void ConnectSocket(HttpListenerContext context) {
            if (socket != null) {
                context.Response.StatusCode = 400;
            }

            var raw = await context.AcceptWebSocketAsync("protocolTwo");
            socket = new WebSocket(raw.WebSocket);
            socket.OnClose += (s, e) => socket = null;
        }

        [Endpoint("/categories")]
        public List<string> GetCategories() {
            return logs.Keys.ToList();
        }

        [Endpoint("/log")]
        public Log GetLog(string name) {
            Log log;

            if (!logs.TryGetValue(name, out log)) return null;

            return log;
        }

        [JSONSerializable]
        public class Log {
            public string Name { get; }
            public List<LogItem> Entries { get; } = new List<LogItem>();

            public event EventHandler<LogItem> Written;

            public Log(string name) {
                Name = name;
            }

            public void Write(string category, string summary, JSONValue content) {
                var item = new LogItem(category, summary, content);
                Entries.Add(item);

                Written?.Invoke(this, item);
            }
        }

        [JSONSerializable]
        public class LogItem {
            public string Category { get; }
            public string Summary { get; }
            public JSONValue Content { get; }
            public DateTime Time { get; }

            public LogItem(string category, string summary, JSONValue content) {
                Category = category;
                Summary = summary;
                Content = content;
                Time = DateTime.Now;
            }
        }
    }
}
