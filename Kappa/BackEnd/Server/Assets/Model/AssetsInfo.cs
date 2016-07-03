using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Assets.Model {
    [JSONSerializable]
    public class AssetsInfo {
        public string Version { get; }
        public string Locale { get; }

        public AssetsInfo(string version, string locale) {
            Version = version;
            Locale = locale;
        }
    }
}
