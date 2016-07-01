using RtmpSharp.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RtmpSharp {
    public static class TypeGenerator {
        private static List<Dictionary<string, string>> cache = new List<Dictionary<string, string>>();

        public static string Invoke(object o) {
            var aso = o as AsObject;

            if (o == null) o = new object();

            if (aso != null) {
                var name = aso.TypeName;
                var shortName = GetShortName(name);

                var map = aso.Select(pair => Tuple.Create(pair.Key, Invoke(pair.Value))).ToDictionary(t => t.Item1, t => t.Item2);
                if (cache.Any(c => c.SequenceEqual(map))) {
                    return shortName;
                }
                cache.Add(map);

                Debug.WriteLine($"[Serializable, SerializedName(\"{name}\")]");
                Debug.WriteLine($"public class {shortName} {{");
                foreach (var pair in aso) {
                    if (pair.Key == "futureData" || pair.Key == "dataVersion") continue;

                    var type = Invoke(pair.Value);
                    var sanitized = SanitizeName(pair.Key);
                    Debug.WriteLine($"  [SerializedName(\"{pair.Key}\")]");
                    Debug.WriteLine($"  public {GetShortName(type)} {sanitized} {{ get; set; }}");
                }
                Debug.WriteLine("}");
                Debug.WriteLine("");
                return shortName;
            }

            if (o.GetType().Namespace == typeof(Debug).Namespace) {
                return o.GetType().FullName;
            }

            if (o.GetType().GetCustomAttributes<SerializableAttribute>() != null) {
                return o.GetType().FullName;
            }
            return o.GetType().FullName;
        }

        public static void Missing(string name, string memberName, Type type) {
            if (memberName == "futureData" || memberName == "dataVersion") return;
            Debug.WriteLine($"Type {name} is missing member {memberName} of type {type.FullName}");
        }

        private static string SanitizeName(string key) {
            return char.ToUpper(key[0]) + key.Substring(1);
        }

        private static string GetShortName(string name) {
            return name.Split('.').Last();
        }
    }
}
