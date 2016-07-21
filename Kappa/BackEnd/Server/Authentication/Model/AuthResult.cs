using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON;
using LeagueSharp;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication.Model {
    public class AuthResult : JSONValuable {
        public LoginQueueDto Content { get; }
        public string Username { get; }
        public string Password { get; }

        public AuthResult(LoginQueueDto dto, string user, string pass) {
            Content = dto;
            Username = user;
            Password = pass;
        }

        public AuthenticationCredentials GetAuthCredentials() {
            return new AuthenticationCredentials {
                Username = Username,
                Password = Password,
                Locale = Region.Locale,
                Domain = "lolclient.lol.riotgames.com",
                AuthToken = Content.Token
            };
        }

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["status"] = Content?.Status,
                ["reason"] = Content?.Reason
            };
        }
    }
}
