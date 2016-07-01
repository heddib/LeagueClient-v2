using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class LoginService : Service {
    protected override string Destination => "loginService";

    public LoginService(Session session) : base(session) { }

    /// <summary>
    /// Login to Riot's servers.
    /// </summary>
    /// <param name="credentials">The credentials for the user</param>
    /// <returns>Session information for the user</returns>
    public Task<LoginSession> Login(AuthenticationCredentials credentials) {
      return InvokeAsync<LoginSession>("login", credentials);
    }

    /// <summary>
    /// Heartbeat to send every 2 minutes.
    /// </summary>
    /// <param name="accountId">The users id</param>
    /// <param name="sessionToken">The token for the user</param>
    /// <param name="heartbeatCount">The current amount that heartbeat has been sent</param>
    /// <param name="currentTime">The current time in GMT-0700 in format ddd MMM d yyyy HH:mm:ss</param>
    public Task<string> PerformLcdsHeartBeat(long accountId, string sessionToken, int heartbeatCount, string currentTime) {
      return InvokeAsync<string>("performLCDSHeartBeat", accountId, sessionToken, heartbeatCount, currentTime);
    }

    /// <summary>
    /// Gets the store url with token information for the current user.
    /// </summary>
    /// <returns>Returns the store URL</returns>
    public Task<string> GetStoreUrl() {
      return InvokeAsync<string>("getStoreUrl");
    }

    /// <summary>
    /// Log out of Riot's servers
    /// </summary>
    /// <returns></returns>
    public Task Logout(string token) {
      return InvokeAsync<object>("logout", token);
    }
  }
}
