using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kappa.BackEnd.Server {
    public class WebSocket {
        public delegate void MessageHandler(object sender, MessageEventArgs e);
        public delegate void CloseHandler(object sender, EventArgs e);

        public event MessageHandler OnMessage;
        public event CloseHandler OnClose;

        public System.Net.WebSockets.WebSocket Base { get; }
        private int bufferSize;
        private SemaphoreSlim slim = new SemaphoreSlim(1);

        public WebSocket(System.Net.WebSockets.WebSocket socket, int bufferSize = 8192) {
            this.Base = socket;
            this.bufferSize = bufferSize;

            new Thread(Receive).Start();
        }

        public async Task Send(string data) => await Send(Encoding.UTF8.GetBytes(data));

        public async Task Send(byte[] data, int off = 0, int len = -1) {
            await slim.WaitAsync();
            if (len < 0) len = data.Length;
            await Base.SendAsync(new ArraySegment<byte>(data, off, len), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            slim.Release();
        }

        public async Task Close(System.Net.WebSockets.WebSocketCloseStatus status) {
            await Base.CloseAsync(status, "", CancellationToken.None);
        }

        private async void Receive() {
            try {
                var buffer = new byte[bufferSize];
                while (Base.State == System.Net.WebSockets.WebSocketState.Open) {
                    using (var mem = new MemoryStream()) {
                        System.Net.WebSockets.WebSocketReceiveResult result;
                        do {
                            result = await Base.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            mem.Write(buffer, 0, result.Count);
                        } while (!result.EndOfMessage);

                        if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close) {
                            await Base.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            OnClose?.Invoke(this, EventArgs.Empty);
                            Base.Dispose();
                        } else {
                            OnMessage?.Invoke(this, new MessageEventArgs(mem.ToArray(), result));
                        }
                    }
                }
            } catch (ApplicationException) {
                OnClose?.Invoke(this, EventArgs.Empty);
            } catch (System.Net.WebSockets.WebSocketException) {
                OnClose?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public class MessageEventArgs : EventArgs {
        public byte[] Data { get; set; }
        public System.Net.WebSockets.WebSocketReceiveResult Result { get; set; }

        public MessageEventArgs(byte[] raw, System.Net.WebSockets.WebSocketReceiveResult result) {
            Result = result;
            Data = raw;
        }
    }
}
