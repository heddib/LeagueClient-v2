using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using MFroehlich.Parsing.JSON;
using Kappa.BackEnd.Server.Diagnostics;

namespace Kappa.BackEnd.Server {
    public static class BackEndServer {
        public static Thread MainThread { get; private set; }

        private static HttpListener server;
        private static List<WebSocket> asyncs = new List<WebSocket>();
        private static List<WebSocket> logs = new List<WebSocket>();
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
            foreach (var socket in logs) {
                await socket.Send(json);
            }
        }

        public static void Stop() {
            //// ReSharper disable once NotAccessedVariable
            //Task t;
            //// ReSharper disable RedundantAssignment
            //foreach (var open in asyncs.ToList())
            //    t = open.Close(WebSocketCloseStatus.EndpointUnavailable);
            //foreach (var open in logs.ToList())
            //    t = open.Close(WebSocketCloseStatus.EndpointUnavailable);
            //// ReSharper restore RedundantAssignment
            MainThread.Abort();
            server.Stop();
            server.Close();
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
                asyncs.Add(socket);
                socket.OnClose += (s, e) => {
                    asyncs.Remove(socket);
                };
                break;

            case "/log":
                logs.Add(socket);
                socket.OnClose += (s, e) => {
                    logs.Remove(socket);
                };
                break;
            }
        }


        private static void Send(string type, object json) {
            var bytes = new JSONObject {
                ["type"] = type,
                ["data"] = json
            }.ToJSON().GetBytes();
            foreach (var socket in asyncs) {
                try {
                    // ReSharper disable once UnusedVariable
                    var t = socket.Send(bytes);
                } catch (ObjectDisposedException) { } catch (SocketException) { }
            }
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
