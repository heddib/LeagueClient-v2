using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
using Kappa.BackEnd.Server.Chat;
using Kappa.BackEnd.Server.Patcher;
using Kappa.Riot.Domain;
using MFroehlich.Parsing;

namespace Kappa.BackEnd {
    public class Maestro {
        private const short MaestroPort = 8393;

        private ChatService chat;
        private PatcherService patcher;

        private TcpListener server;

        public event EventHandler GameClientClosed;

        public Maestro(ChatService chat, PatcherService patcher) {
            this.patcher = patcher;
            this.chat = chat;

            this.server = new TcpListener(IPAddress.Any, MaestroPort);

            new Thread(ConnectLoop) {
                IsBackground = true,
                Name = "Maestro"
            }.Start();
        }

        private void Launch(string args) {
            var info = new ProcessStartInfo {
                FileName = patcher.ExecutablePath,
                Arguments = $"\"{MaestroPort}\" \"LoLPatcher.exe\" \"\" \"{args}\"",
                WorkingDirectory = Path.GetDirectoryName(patcher.ExecutablePath)
            };
            Process.Start(info);
        }

        internal void JoinGame(PlayerCredentialsDto creds) {
            string str = $"{creds.ServerIp} {creds.ServerPort} {creds.EncryptionKey} {creds.SummonerId}";
            Launch(str);
        }

        internal void ReplayGame(string host, string key, long gameId) {
            string str = $"replay {host} {key} {gameId} NA1";
            Launch(str);
        }

        internal void SpectateGame(string host, string key, long gameId) {
            string str = $"spectator {host} {key} {gameId} NA1";
            Launch(str);
        }

        private void ConnectLoop() {
            server.Start();
            using (var socket = server.AcceptSocket())
            using (var stream = new NetworkStream(socket)) {
                Session.Log("Maestro connected");
                try {
                    while (socket.Connected) {
                        var header = stream.ReadStruct<MaestroMessageHeader>();
                        var payload = new byte[header.Size];
                        stream.ReadFully(payload, 0, payload.Length);

                        switch (header.Type) {
                        case MaestroMessageType.HEARTBEAT:
                            var beat = new MaestroMessageHeader(MaestroMessageType.HEARTBEAT, 0);
                            stream.WriteStruct(beat);
                            break;

                        case MaestroMessageType.ACK:
                            //ignore?
                            break;

                        case MaestroMessageType.CHATMESSAGE_FROM_GAME:
                            var str = Encoding.UTF8.GetString(payload);
                            var xml = new XmlDocument();
                            xml.LoadXml($"<root>{str}</root>");
                            var type = xml.SelectSingleNode("/root/type").InnerText;
                            switch (type) {
                            case "rqUpdate":
                                SendUpdate(stream);
                                break;
                            case "sndMessage":
                                var name = xml.SelectSingleNode("/root/summoner").InnerText;
                                var msg = xml.SelectSingleNode("/root/message").InnerText;
                                var friend = chat.Friends.SingleOrDefault(f => f.Name == name);
                                chat.SendMessage(friend.User, msg);
                                break;
                            default:
                                Session.Log($"Unknown Maestro message type: {type}");
                                break;
                            }

                            goto default;

                        default:
                            var ack = new MaestroMessageHeader(MaestroMessageType.ACK, 0);
                            stream.WriteStruct(ack);
                            break;
                        }

                        Debug.WriteLine(header.Type + " (GAME): ");
                        if (payload.Any())
                            Debug.WriteLine("  " + Encoding.UTF8.GetString(payload));
                    }
                } catch {
                    this.GameClientClosed?.Invoke(this, EventArgs.Empty);
                    Session.Log("Maestro disconnected");
                }
            }
        }

        private void SendUpdate(Stream dst) {
            var str = new StringBuilder();
            str.Append("<>");
            str.Append("<type>updBuddyList</type>");
            foreach (var friend in chat.Friends) str.Append($"<buddy>{friend.Name}</buddy>");
            str.Append("</>");
            var header = new MaestroMessageHeader(MaestroMessageType.CHATMESSAGE_TO_GAME, str.Length);
            var payload = str.ToString().GetBytes();
            dst.WriteStruct(header);
            dst.Write(payload);

            str.Clear();
            str.Append("<>");
            str.Append("<type>updIgnoreList</type>");
            str.Append("</>");
            header = new MaestroMessageHeader(MaestroMessageType.CHATMESSAGE_TO_GAME, str.Length);
            payload = str.ToString().GetBytes();
            dst.WriteStruct(header);
            dst.Write(payload);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MaestroMessageHeader {
            public int HeaderLength;
            public int Version;
            public MaestroMessageType Type;
            public int Size;

            public MaestroMessageHeader(MaestroMessageType type, int size) {
                HeaderLength = Marshal.SizeOf<MaestroMessageHeader>();
                Version = 1;
                Type = type;
                Size = size;
            }
        }

        // ReSharper disable UnusedMember.Local
        private enum MaestroMessageType {
            EXIT = 3,
            ACK = 5,
            HEARTBEAT = 4,

            GAMESTART = 0,

            GAMECLIENT_STOPPED = 1,
            GAMECLIENT_CRASHED = 2,

            GAMECLIENT_ABANDONED = 7,
            GAMECLIENT_LAUNCHED = 8,
            GAMECLIENT_CONNECTED = 10,
            CHATMESSAGE_TO_GAME = 11,
            CHATMESSAGE_FROM_GAME = 12,
        }
        // ReSharper restore UnusedMember.Local
    }
}
