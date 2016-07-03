using Kappa.Settings;
using MFroehlich.Parsing.JSON;
using RtmpSharp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Kappa.BackEnd;
using MFroehlich.League.DataDragon;

namespace TypescriptGenerator {
    public class Generator {
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

            Generate(typeof(Session).Assembly, "", "test.ts");
        }

        private static void Generate(Assembly ass, string space, string fileName) {
            using (var raw = File.OpenWrite(Path.Combine(@"C:\Users\Max\Desktop", fileName)))
            using (var file = new StreamWriter(raw)) {
                foreach (var type in ass.GetTypes()) {
                    if (type.IsSubclassOf(typeof(BaseSettings))) continue;
                    if (!type.Namespace?.Contains(space) ?? false) continue;

                    var jsonAtt = type.GetCustomAttribute<JSONSerializableAttribute>();

                    if (type.IsEnum) {

                    }
                    else if (typeof(JSONSerializable).IsAssignableFrom(type)) {
                        file.WriteLine("export interface " + type.Name + " {");
                        foreach (var member in Member.GetMembers(type)) {
                            var att = member.GetAttributes<JSONFieldAttribute>().ToList();
                            if (!att.Any()) { }
                            file.WriteLine($"  {att.FirstOrDefault()?.FieldName ?? member.Name}: {GetJsType(member.MemberType)};");
                        }
                        file.WriteLine("}");
                    }
                    else if (jsonAtt != null) {
                        file.WriteLine("export interface " + type.Name + " {");
                        foreach (var member in Member.GetMembers(type)) {
                            string name = member.Name;
                            if (jsonAtt.CorrectPascalCase)
                                name = char.ToLower(name[0]) + name.Substring(1);

                            file.WriteLine($"  {name}: {GetJsType(member.MemberType)};");
                        }
                        file.WriteLine("}");
                    }
                    else if (type.Name == "GameParticipant" || type.GetCustomAttributes<SerializedNameAttribute>().Any()) {
                        var parent = type.BaseType;
                        var matches = parent.Name == "GameParticipant" || parent.GetCustomAttributes<SerializedNameAttribute>().Any();
                        file.WriteLine($"interface {type.Name}{(matches ? " extends " + parent.Name : "")} {{");
                        foreach (var member in Member.GetMembers(type)) {
                            var att = member.GetAttributes<SerializedNameAttribute>().ToList();
                            if (!att.Any()) {
                                continue;
                            }
                            file.WriteLine($"    {att.First().SerializedName}: {GetJsType(member.MemberType)};");
                        }
                        file.WriteLine("}");
                    }
                }
            }
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
                    return GetJsType(type.GetGenericArguments()[0]) + "?";
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
