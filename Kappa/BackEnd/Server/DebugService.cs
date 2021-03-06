﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Kappa.Riot.Domain.TeambuilderDraft;
using MFroehlich.Parsing;
using MFroehlich.Parsing.JSON;
using Kappa.Riot.Domain;

// ReSharper disable All

namespace Kappa.BackEnd.Server {
    public class DebugService : JSONService {
        private Session session;

        public DebugService(Session session) : base("/debug") {
            this.session = session;
            this.session.Authed += Session_Authed;

            new Thread(() => {
                //const string name = "MrStealYOWIFE";
                //var summ = await Session.RiotAPI.SummonerAPI.ByName(name);
                //var game = await Session.RiotAPI.CurrentGameAPI.BySummoner(summ.Values.First().id);
                //var file = @"C:\users\max\desktop\2232035041.lol";
                //var replay = new Replay.Replay(file);
                //var replay = Replay.Replay.Record(game.gameId, game.observers.encryptionKey, file);

                //session.Maestro.ReplayGame("localhost", BackEndServer.HttpPort, replay.MetaData.EncryptionKey, replay.MetaData.GameKey.GameId);
            }).Start();
            //new Thread(RunMaestro) { IsBackground = true }.Start();
            /*
            var dict = new Dictionary<string, string> {
                ["MINIONS_KILLED"] = "totalMinionsKilled",
                ["NEUTRAL_MINIONS_KILLED_YOUR_JUNGLE"] = "neutralMinionsKilledTeamJungle",
                ["MAGIC_DAMAGE_DEALT_PLAYER"] = "magicDamageDealt",
                ["TRUE_DAMAGE_DEALT_PLAYER"] = "trueDamageDealt",
                ["PHYSICAL_DAMAGE_DEALT_PLAYER"] = "physicalDamageDealt",
                ["MAGIC_DAMAGE_TAKEN"] = "magicalDamageTaken",
                ["WARD_KILLED"] = "wardsKilled",
                ["WARD_PLACED"] = "wardsPlaced",
                ["LEVEL"] = "champLevel",
                ["CHAMPIONS_KILLED"] = "kills",
                ["BARRACKS_KILLED"] = "inhibitorKills",
                ["TURRETS_KILLED"] = "turretKills",
                ["NUM_DEATHS"] = "deaths"
            };

            var raw = File.ReadAllText(@"C:\Users\Max\Documents\Visual Studio 2015\Projects\Kappa\Stuff\eogstats.json");
            var eog = JSONDeserializer.Deserialize<EndOfGameStats>(JSONParser.ParseObject(raw));
            var participant = eog.TeamPlayerParticipantStats.First();

            var stats = new ParticipantStats();
            foreach (var stat in participant.Statistics) {
                string str;
                if (!dict.TryGetValue(stat.StatTypeName, out str)) {
                    var build = new StringBuilder();
                    var caps = true;
                    foreach (var c in stat.StatTypeName) {
                        if (c == '_') {
                            caps = true;
                            continue;
                        }
                        if (caps) build.Append(char.ToUpper(c));
                        else build.Append(char.ToLower(c));
                        caps = false;
                    }
                    str = build.ToString();
                }
                else {
                    str = char.ToUpper(str[0]) + str.Substring(1);
                }
                var prop = typeof(ParticipantStats).GetProperty(str);
                if (prop == null) Debugger.Break();
                else if (prop.PropertyType == typeof(bool)) {
                    prop.SetValue(stats, stat.Value != 0);
                }
                else {
                    prop.SetValue(stats, stat.Value);
                }
            }*/
        }

        private void Session_Authed(object sender, EventArgs e) {
        }

        [Endpoint("/eog")]
        public void DebugEog() {
            var file = @"C:\Users\max\desktop\eog.txt";
            var lines = File.ReadAllLines(file);
            new Thread(() => {
                var stats = JSONDeserializer.Deserialize<EndOfGameStats>(JSONParser.Parse(lines[0]));
                var a = JSONDeserializer.Deserialize<SimpleDialogMessage>(JSONParser.Parse(lines[1]));
                var b = JSONDeserializer.Deserialize<SimpleDialogMessage>(JSONParser.Parse(lines[2]));
                Thread.Sleep(1000);
                session.HandleMessage(stats);
                Thread.Sleep(1000);
                session.HandleMessage(a);
                Thread.Sleep(1000);
                session.HandleMessage(b);
            }).Start();
        }

        [Endpoint("/draft")]
        public void DebugDraft() {
            var file = @"C:\Users\max\desktop\full draft game.txt";
            var lines = File.ReadAllLines(file);
            new Thread(() => {
                int index = 0;
                while (true) {
                    while (!lines[index].StartsWith("  {\"counter\":")) index++;
                    var lcds = new GameDataObject("", "OK", (JSONObject) JSONParser.Parse(lines[index]));
                    Thread.Sleep(1000);
                    session.HandleMessage(lcds);
                    index++;
                }
            }).Start();
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

        private enum MaestroMessageType {
            EXIT = 3,
            ACK = 5,
            HEARTBEAT = 4,
            GAMESTART = 0,
            GAMEEND = 1,
            GAMECRASHED = 2,

            GAMECLIENT_LAUNCHED = 8,
            GAMECLIENT_CONNECTED = 10,
            CHATMESSAGE_TO_GAME = 11,
            CHATMESSAGE_FROM_GAME = 12,
        }
    }
}
