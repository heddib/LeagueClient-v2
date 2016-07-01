using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication.Model {
    public class SavedAccount : JSONValuable {
        public string User { get; }
        public string Name { get; }
        public int ProfileIcon { get; }

        public SavedAccount(string user, Account settings) {
            User = user;
            Name = settings.SummonerName;
            ProfileIcon = settings.ProfileIcon;
        }

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["user"] = User,
                ["name"] = Name,
                ["icon"] = ProfileIcon,
            };
        }
    }
}
