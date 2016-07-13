using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Net.WebSockets;
using MFroehlich.Parsing.JSON;
using Kappa.BackEnd.Server.Diagnostics;

namespace Kappa.BackEnd.Server {
    public static class BackEndServer {
        public static Thread MainThread { get; private set; }

        private static HttpListener server;
        private static WebSocket clientAsync;
        private static WebSocket logAsync;
        private static List<LogItem> log = new List<LogItem>();

        private static List<HttpService> services = new List<HttpService>();

        public static IEnumerable<HttpService> Services => services;
        public static IEnumerable<LogItem> Logs => log;

        public static string HostName { get; private set; }

        public static void Initialize() {
            var tmp = new TcpListener(IPAddress.Any, 0);
            tmp.Start();

            HostName = "localhost:" + ((IPEndPoint) tmp.LocalEndpoint).Port;

            tmp.Stop();
        }

        public static void Start() {
            var docService = new DocumentationService();
            var logService = new LogService();
            AddService(docService);
            AddService(logService);

            server = new HttpListener();
            server.Prefixes.Add($"http://{HostName}/");

            MainThread = new Thread(ServerWorker) { IsBackground = true };
            MainThread.Start();
        }

        public static void AddService(HttpService service) {
            services.Add(service);
        }

        public static void Async(string source, JSONObject arg) {
            var json = new JSONObject {
                ["source"] = source,
                ["body"] = arg
            };
            Send("async", json);
        }

        public static async void Log(string category, string summary, JSONValue content) {
            var item = new LogItem(category, summary, content);
            var json = item.Serialize().ToJSON();
            log.Add(item);
            if (logAsync != null)
                await logAsync.Send(json);
        }

        public static async void Stop() {
            MainThread.Abort();
            server.Stop();
            server.Close();
            try {
                if (clientAsync != null)
                    await clientAsync.Close(WebSocketCloseStatus.Empty);
                if (logAsync != null)
                    await logAsync.Close(WebSocketCloseStatus.Empty);
            } catch {
                // ignored
            }
        }


        private static void ServerWorker() {
            server.Start();

            try {
                while (server.IsListening) {
                    var context = server.GetContext();

                    ThreadPool.QueueUserWorkItem(o => {
                        try {
                            if (context.Request.IsWebSocketRequest) {
                                HandleWebSocketRequest(context);
                            }
                            else {
                                using (context.Response) {
                                    var handled = false;
                                    foreach (var service in services) {
                                        if (!context.Request.Url.LocalPath.StartsWith(service.BaseUrl)) continue;
                                        try {
                                            handled = service.Handle(context);
                                        } catch (HttpListenerException x) when (x.ErrorCode == 1236) {
                                            //Connection closed
                                            return;
                                        }
                                        if (handled) break;
                                    }

                                    if (!handled) {
                                        context.Response.StatusCode = 404;
                                    }
                                }
                            }
                        } catch (HttpListenerException) { }
                    });
                }
            } catch (HttpListenerException x) when (x.ErrorCode == 1 || x.ErrorCode == 995) { }
        }

        private static async void HandleWebSocketRequest(HttpListenerContext context) {
            var raw = await context.AcceptWebSocketAsync("protocolTwo");
            var socket = new WebSocket(raw.WebSocket);
            switch (context.Request.Url.LocalPath) {
            case "/async":
                if (clientAsync != null) {
                    context.Response.StatusCode = 400;
                }

                clientAsync = socket;
                clientAsync.OnClose += (s, e) => clientAsync = null;
                break;

            case "/log":
                if (logAsync != null) {
                    context.Response.StatusCode = 400;
                }

                logAsync = socket;
                logAsync.OnClose += (s, e) => logAsync = null;
                break;
            }
        }


        private static async void Send(string type, object json) {
            var bytes = new JSONObject {
                ["type"] = type,
                ["data"] = json
            }.ToJSON().GetBytes();
            try {
                if (clientAsync != null)
                    await clientAsync.Send(bytes);
            } catch (ObjectDisposedException) { } catch (SocketException) { }
        }
    }

    public class LogItem : JSONSerializable {
        [JSONField("category")]
        public string Category { get; }
        [JSONField("summary")]
        public string Summary { get; }
        [JSONField("content")]
        public JSONValue Content { get; }
        [JSONField("time")]
        public DateTime Time { get; }

        public LogItem(string category, string summary, JSONValue content) {
            Category = category;
            Summary = summary;
            Content = content;
            Time = DateTime.Now;
        }
    }

    public class DocsAttribute : Attribute {
        public string Field { get; }
        public object Value { get; }

        public DocsAttribute(string field, string value) {
            Field = field;
            Value = value;
        }
    }
}
