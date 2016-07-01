using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication.Model {
    public class AccountState : JSONValuable {
        public bool InGame { get; }

        public AccountState(AuthResult auth) {
            InGame = auth.Content.InGameCredentials?.InGame == true;
        }

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["inGame"] = InGame
            };
        }
    }
}
