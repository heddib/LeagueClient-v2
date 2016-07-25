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
        public static LogService LogService { get; private set; }

        private static LogService.Log log;
        private static HttpListener server;
        private static WebSocket clientAsync;

        private static List<HttpService> services = new List<HttpService>();

        public static IEnumerable<HttpService> Services => services;

        public static string HostName { get; private set; }

        public static void Initialize() {
            var tmp = new TcpListener(IPAddress.Any, 0);
            tmp.Start();

            HostName = "localhost:" + ((IPEndPoint) tmp.LocalEndpoint).Port;

            tmp.Stop();

            var docs = new DocumentationService();
            AddService(docs);

            LogService = new LogService();
            log = LogService.CreateLog("REST");
        }

        public static void Start() {
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

        public static async void Stop() {
            MainThread.Abort();
            server.Stop();
            server.Close();
            try {
                if (clientAsync != null)
                    await clientAsync.Close(WebSocketCloseStatus.Empty);
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
                        var handled = false;
                        var json = false;

                        try {
                            if (context.Request.Url.LocalPath == "/async") {
                                HandleAsyncConnection(context);
                                handled = true;
                            }

                            foreach (var service in services) {
                                if (!context.Request.Url.LocalPath.StartsWith(service.BaseUrl)) continue;
                                try {
                                    handled = service.Handle(context);
                                } catch (HttpListenerException x) when (x.ErrorCode == 1236) {
                                    //Connection closed
                                    return;
                                }
                                if (handled) {
                                    json = service is JSONService;
                                    break;
                                }
                            }

                            if (!handled) {
                                context.Response.StatusCode = 404;
                            }
                        } catch (HttpListenerException) {
                            //Ignore//
                        } finally {
                            try {
                                if (!context.Request.IsWebSocketRequest || !handled) {
                                    context.Response.Close();
                                }
                            } catch (ObjectDisposedException) {
                                //Ignore
                            }
                        }

#if DEBUG
                        log.Write(json ? "REST" : "Request", context.Request.Url.LocalPath, new JSONObject {
                            ["handled"] = handled
                        });
#endif
                    });
                }
            } catch (HttpListenerException x) when (x.ErrorCode == 1 || x.ErrorCode == 995) { }
        }

        private static async void HandleAsyncConnection(HttpListenerContext context) {
            if (clientAsync != null) {
                context.Response.StatusCode = 400;
            }

            var raw = await context.AcceptWebSocketAsync("protocolTwo");
            clientAsync = new WebSocket(raw.WebSocket);
            clientAsync.OnClose += (s, e) => clientAsync = null;
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

    public class DocsAttribute : Attribute {
        public string Field { get; }
        public object Value { get; }

        public DocsAttribute(string field, string value) {
            Field = field;
            Value = value;
        }
    }
}
