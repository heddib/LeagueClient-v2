using MFroehlich.League.Assets;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;
using System.Linq;

namespace Kappa.BackEnd.Server.Assets.Model {
    [JSONSerializable]
    public class AudioAssets {
        public Dictionary<string, string> Urls { get; set; }

        public AudioAssets(RiotVersion version) {
            Urls = new Dictionary<string, string>();

            foreach (var champ in DataDragon.ChampData.Value.data.Values) {
                var matches = version.GetFiles("en_US/champions/" + champ.id + ".mp3");
                var file = matches.Single();
                Set("champ_quote", champ.key.ToString(), file.Url.AbsoluteUri);
            }
        }

        private void Set(params string[] keys) {
            Urls[string.Join(".", keys.Take(keys.Length - 1))] = keys.Last();
        }
    }
}