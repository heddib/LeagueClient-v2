using Kappa.Settings;
using MFroehlich.Parsing.JSON;
using RtmpSharp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Kappa.BackEnd;
using MFroehlich.League.DataDragon;

namespace TypescriptGenerator {
    public class Generator : IDisposable {
        private const int IndentSize = 4;

        private StringBuilder build = new StringBuilder();
        private TextWriter writer;

        private int indent;

        public Generator(string path, string space = null) {
            writer = File.CreateText(path);

            if (space != null) {
                Start($"namespace {space}");
                indent = IndentSize;
            }
        }

        public void WriteField(string name, string type) {
            WriteLine($"{name}: {type};");
        }

        public void Start(string name) {
            WriteLine($"{name} {{");
            indent += IndentSize;
        }

        public void Stop() {
            indent -= IndentSize;
            WriteLine("}");
        }

        private void WriteLine(string line) {
            build.Clear();
            build.Append(' ', indent);

            writer.Write(build);
            writer.WriteLine(line);
        }

        public void Dispose() {
            while (indent > 0) Stop();
            writer.Dispose();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main() {
            //var http = WebRequest.CreateHttp("https://playerpreferences.riotgames.com/swagger.json");
            ////var http = WebRequest.CreateHttp("https://playerpreferences.riotgames.com/playerPref/v1/getPreference/210492987/NA1/ItemSets");
            ////var http = WebRequest.CreateHttp("https://playerpreferences.riotgames.com/playerPref/v1/getPreference/210492987/NA1/GamePreferences");
            //http.Headers["Authorization"] = "JwtBearer eyJhbGciOiJSUzI1NiJ9.eyJsb2wiOlt7InVpZCI6MjEwNDkyOTg3LCJjdWlkIjoyMTA0OTI5ODcsInVuYW1lIjoiY29vbGR1ZGVmNDAiLCJjcGlkIjoiTkExIiwicGlkIjoiTkExIn1dLCJleHAiOjE0NjU4NjgwNzEsInN1YiI6IjkyZmNjYThiLTU0YzgtNWE2YS1iMzRiLTA0M2FlMTNlYjJkNSIsInRhZyI6W10sImF1ZCI6ImxvbCIsImlzcyI6Imh0dHBzOlwvXC9nYXMucmlvdGdhbWVzLmNvbSIsImlhdCI6MTQ2NTc4MTY3MSwiYW1yIjoiY2xpZW50X2Jhc2ljIn0.igDHaW2LImMfVLQXvwZ1_e3BE3Fj1H71K0ndA4ZqGWn2szP2E7fySVY0j1XZoxqpNZUQNz6T_1SMDGNDurhIya029eg5-cpoG0fI6XUoXHv1kiAuV8H9ad7JxLhG7SQQJ5GSj6SLOonG281yLGICeuJgFQgDhVKl1PLRYRKgaT2k817w9NclkRPRr_8mmZMmx7wDdbELIIi_e21VPWWfZxTnhdCCcs7JLXQar3TYnN9lWxss7MCa_8lBSiEyMq4wz3ZyBpbLa_ah_L77Disvwv8SUzZlhgMwXesPrCwGRpbIsUNAstG5OSVOvxtGwE9IBYTgNJ6Ps07GXKWZqtw5uw";
            //using (var req = http.GetResponse())
            //using (var stream = req.GetResponseStream())
            //using (var mem = new MemoryStream()) {
            //    stream.CopyTo(mem);
            //    Console.WriteLine(Encoding.UTF8.GetString(mem.ToArray()));
            //    //var json = JSONParser2.ParseObject(stream);
            //    //var data = Convert.FromBase64String((string) json["data"]);
            //}
            //return;

            var ass = typeof(Session).Assembly;
            var filename = Path.Combine(@"C:\Users\Max\Desktop", "test2.ts");

            using (var generator = new Generator(filename, "Domain")) {
                var namespaces = from t in ass.GetTypes()
                                 where t.Namespace != null
                                 where t.Namespace.Contains("Model")
                                 let name = t.Namespace.Split('.')[3]
                                 group t by name into space
                                 orderby space.Key
                                 select space;

                foreach (var space in namespaces) {
                    Generate(generator, space.Key, space);
                }

                var gamedata = ass.GetTypes().Where(t => t.Namespace == "Kappa.Riot.Domain.JSON.lol_game_data");
                Generate(generator, "GameData", gamedata);

                var history = ass.GetTypes().Where(t => t.Namespace == "Kappa.Riot.Domain.JSON.MatchHistory");
                Generate(generator, "MatchHistory", history);
            }
        }

        private static void Generate(Generator file, string space, IEnumerable<Type> types) {
            file.Start($"export namespace {space}");

            foreach (var type in types.OrderBy(t => t.Name)) {
                if (type.IsSubclassOf(typeof(BaseSettings))) continue;

                var jsonAtt = type.GetCustomAttribute<JSONSerializableAttribute>();

                if (type.IsEnum) { }
                else if (typeof(JSONSerializable).IsAssignableFrom(type)) {
                    file.Start($"export interface {type.Name}");
                    foreach (var member in Member.GetMembers(type)) {
                        if (member.GetAttributes<JSONSkipAttribute>().Any()) continue;

                        string name = member.Name;
                        var att = member.GetAttributes<JSONFieldAttribute>().ToList();
                        if (att.Any()) name = att.First().FieldName;

                        file.WriteField(name, GetJsType(member.MemberType));
                    }
                    file.Stop();
                }
                else if (jsonAtt != null) {
                    file.Start($"export interface {type.Name}");
                    foreach (var member in Member.GetMembers(type)) {
                        if (member.GetAttributes<JSONSkipAttribute>().Any()) continue;

                        string name = member.Name;
                        if (jsonAtt.CorrectPascalCase)
                            name = JSON.CorrectPascalCase(name);

                        file.WriteField(name, GetJsType(member.MemberType));
                    }
                    file.Stop();
                }

            }

            file.Stop();
        }

        private static string GetJsType(Type type) {
            var notnums = new[] {
                TypeCode.Boolean,
                TypeCode.Char,
                TypeCode.DateTime,
                TypeCode.DBNull,
                TypeCode.Empty,
                TypeCode.Object,
                TypeCode.String
            };
            var map = new Dictionary<Type, string>() {
                [typeof(string)] = "string",
                [typeof(bool)] = "boolean",
                [typeof(DateTime)] = "Date",
                [typeof(AsObject)] = "any",
                [typeof(Guid)] = "string"
            };
            string result;
            if (map.TryGetValue(type, out result)) {
            }
            else if (type.IsEnum) {
                result = "string";
            }
            else if (!notnums.Contains(Type.GetTypeCode(type))) {
                result = "number";
            }
            else if (type.IsArray) {
                return GetJsType(type.GetElementType()) + "[]";
            }
            else if (type.IsGenericType) {
                if (type.GetGenericTypeDefinition() == typeof(List<>)) {
                    return GetJsType(type.GetGenericArguments()[0]) + "[]";
                }
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) {
                    var args = type.GetGenericArguments();
                    return $"{{ [id: {GetJsType(args[0])}]: {GetJsType(args[1])} }}";
                }
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    return GetJsType(type.GetGenericArguments()[0]);// + "?";
                }
                return "";
            }
            else {
                result = type.Name;
            }
            return result;
        }
    }
}
