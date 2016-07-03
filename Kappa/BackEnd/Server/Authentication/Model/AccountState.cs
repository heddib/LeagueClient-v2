using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication.Model {
    [JSONSerializable]
    public class AccountState {
        public bool InGame { get; }

        public AccountState(AuthResult auth) {
            InGame = auth.Content.InGameCredentials?.InGame == true;
        }
    }
}
