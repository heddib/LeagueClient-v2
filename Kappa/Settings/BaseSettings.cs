using MFroehlich.Parsing.JSON;
using System.Collections.Generic;
using System.IO;

namespace Kappa.Settings {
    public abstract class BaseSettings : JSONSerializable {
        private string file;

        private static Dictionary<string, BaseSettings> loaded = new Dictionary<string, BaseSettings>();

        public static T Create<T>(string sourceFile) where T : BaseSettings, new() {
            JSONObject raw;
            T t;
            if (loaded.ContainsKey(sourceFile))
                return (T) loaded[sourceFile];
            else if (File.Exists(sourceFile) && JSONParser.TryParseObject(File.ReadAllText(sourceFile), out raw))
                t = JSONDeserializer.Deserialize<T>(raw);
            else {
                Directory.CreateDirectory(Path.GetDirectoryName(sourceFile));
                t = new T();
            }
            loaded.Add(sourceFile, t);
            t.file = sourceFile;
            return t;
        }

        public void Save() {
            lock (file) {
                var raw = Serialize().ToJSON(2, 0);
                File.WriteAllText(file, raw);
            }
        }

        //protected T GetValue<T>([CallerMemberName] string key = "") {
        //    return content.Get<T>(key);
        //}

        //protected void SetValue(object value, [CallerMemberName] string key = "") {
        //    content[key] = value;
        //    Save();
        //}
    }
}
