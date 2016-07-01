using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class LcdsProxyService : Service {
    protected override string Destination => "lcdsServiceProxy";

    public LcdsProxyService(Session session) : base(session) { }

    /// <summary>
    /// Creates a team builder lobby
    /// </summary>
    /// <param name="queueId">The queue ID for the lobby</param>
    /// <param name="uuid">The generated UUID of the lobby</param>
    /// <returns></returns>
    public Task<LobbyStatus> CreateGroupFinderLobby(int queueId, string uuid) {
      return InvokeAsync<LobbyStatus>("createGroupFinderLobby", queueId, uuid);
    }

    /// <summary>
    /// Sends a call to the LCDS Service Proxy
    /// </summary>
    /// <param name="uuid">The generated UUID of the service call</param>
    /// <param name="subService">The lcds service</param>
    /// <param name="procedureCall">The procedure to call</param>
    /// <param name="parameters">The parameters to pass in JSON encoded format</param>
    /// <returns></returns>
    public Task CallLcds(string uuid, string subService, string procedureCall, string parameters) {
      return InvokeAsync<object>("call", uuid, subService, procedureCall, parameters);
    }
  }
}
