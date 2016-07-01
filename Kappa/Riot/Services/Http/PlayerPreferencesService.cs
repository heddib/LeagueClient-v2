using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kappa.BackEnd;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Services.Http {
    internal class PlayerPreferencesService : HttpService {
        private SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();

        public PlayerPreferencesService(Session session) : base(session, "https://playerpreferences.riotgames.com:443/") { }

        public async Task<string> GetPreferences(PlayerPreferencesType type) {
            var json = await Get<JSONObject>("/playerPref/v1/getPreference/210492987/NA1/" + type);
            var raw = (string) json["data"];

            using (var src = new MemoryStream(Convert.FromBase64String(raw)))
            using (var dst = new MemoryStream())
            using (var gzip = new DeflateStream(src, CompressionMode.Decompress)) {
                gzip.CopyTo(dst);
                return Encoding.UTF8.GetString(dst.ToArray());
            }
        }

        public async Task SetPreferences(PlayerPreferencesType type, string prefs) {
            var hash = sha256.ComputeHash(prefs.GetBytes());
            byte[] data;

            using (var src = new MemoryStream(prefs.GetBytes()))
            using (var dst = new MemoryStream())
            using (var gzip = new DeflateStream(src, CompressionMode.Decompress)) {
                gzip.CopyTo(dst);
                data = dst.ToArray();
            }

            var json = new JSONObject {
                ["version"] = "1.0",
                ["type"] = type.ToString(),
                ["data"] = Convert.ToBase64String(data),
                ["hash"] = Convert.ToBase64String(hash)
            };

            await Put("/playerPref/v1/getPreference/210492987/NA1/" + type, json.ToJSON().GetBytes());
        }

        // ReSharper disable InconsistentNaming
        public enum PlayerPreferencesType {
            GamePreferences,
            ItemSets
        }
        // ReSharper restore InconsistentNaming
    }
}
