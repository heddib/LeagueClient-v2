using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication.Model {
    [JSONSerializable]
    public class SavedAccount {
        public string User { get; }
        public string Name { get; }
        public int Icon { get; }

        public SavedAccount(string user, Account settings) {
            User = user;
            Name = settings.SummonerName;
            Icon = settings.ProfileIcon;
        }
    }
}
