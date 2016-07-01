using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Assets.Model {
    public class AssetsInfo : JSONSerializable {
        [JSONField("version")]
        public string Version { get; }
        [JSONField("locale")]
        public string Locale { get; }

        public AssetsInfo(string version, string locale) {
            Version = version;
            Locale = locale;
        }
    }
}
