using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class GameInvitationService : Service {
    protected override string Destination => "lcdsGameInvitationService";

    public GameInvitationService(Session session) : base(session) { }

    /// <summary>
    /// Gets pending invitations
    /// </summary>
    /// <returns></returns>
    public Task<InvitationRequest[]> GetPendingInvitations() {
      return InvokeAsync<InvitationRequest[]>("getPendingInvitations");
    }

    /// <summary>
    /// Creates a team builder lobby
    /// </summary>
    /// <param name="queueId">The queue ID for the lobby (61) </param>
    /// <param name="uuid">The generated UUID of the lobby</param>
    /// <returns></returns>
    public Task<LobbyStatus> CreateGroupFinderLobby(int queueId, string uuid) {
      return InvokeAsync<LobbyStatus>("createGroupFinderLobby", queueId, uuid);
    }

    /// <summary>
    /// Creates an arranged team lobby
    /// </summary>
    /// <param name="queueId">The queue ID for the lobby</param>
    /// <returns></returns>
    public Task<LobbyStatus> CreateArrangedTeamLobby(int queueId) {
      return InvokeAsync<LobbyStatus>("createArrangedTeamLobby", queueId);
    }

    /// <summary>
    /// Creates an arranged team lobby for a co-op vs AI queue
    /// </summary>
    /// <param name="queueId">The queue ID for the lobby</param>
    /// <param name="botDifficulty">The difficulty for the bots</param>
    /// <returns></returns>
    public Task<LobbyStatus> CreateArrangedBotTeamLobby(int queueId, string botDifficulty) {
      return InvokeAsync<LobbyStatus>("createArrangedBotTeamLobby", queueId, botDifficulty);
    }

    /// <summary>
    /// Creates an arranged team lobby for a ranked team game
    /// </summary>
    /// <param name="queueId">The queue ID for the lobby</param>
    /// <param name="teamName">The name of the team</param>
    /// <returns></returns>
    public Task<LobbyStatus> CreateArrangedRankedTeamLobby(int queueId, string teamName) {
      return InvokeAsync<LobbyStatus>("createArrangedRankedTeamLobby", queueId, teamName);
    }

    /// <summary>
    /// Grants invite privelages to a summoner in an arranged lobby
    /// </summary>
    /// <param name="summonerId">The summoner to give invite privelages</param>
    /// <returns></returns>
    public Task GrantInvitePrivileges(long summonerId) {
      return InvokeAsync<LobbyStatus>("grantInvitePrivileges", summonerId);
    }

    /// <summary>
    /// Grants invite privelages to a summoner in an arranged lobby
    /// </summary>
    /// <param name="summonerId">The summoner to give invite privelages</param>
    /// <returns></returns>
    public Task RevokeInvitePrivileges(long summonerId) {
      return InvokeAsync<LobbyStatus>("revokeInvitePrivileges", summonerId);
    }

    /// <summary>
    /// Transfers ownership of a lobby to a summoner in an arranged lobby
    /// </summary>
    /// <param name="summonerId">The summoner to transfer ownership to</param>
    /// <returns></returns>
    public Task TransferOwnership(long summonerId) {
      return InvokeAsync<LobbyStatus>("transferOwnership", summonerId);
    }

    /// <summary>
    /// Invites a summoner to an arranged lobby
    /// </summary>
    /// <param name="summonerId">The summoner to invite</param>
    /// <returns></returns>
    public Task Invite(long summonerId) {
      return InvokeAsync<LobbyStatus>("invite", summonerId);
    }

    /// <summary>
    /// Leaves an arranged lobby
    /// </summary>
    /// <returns></returns>
    public Task Leave() {
      return InvokeAsync<object>("leave");
    }

    /// <summary>
    /// Kicks a summoner from a lobby
    /// </summary>
    /// <param name="summonerId"></param>
    /// <returns></returns>
    public Task Kick(long summonerId) {
      return InvokeAsync<object>("kick");
    }

    /// <summary>
    /// Accepts an invitation to an arranged lobby 
    /// </summary>
    /// <param name="invitationId">The id of the invitation to accept, looks like INVID158928100</param>
    /// <returns></returns>
    public Task<LobbyStatus> Accept(string invitationId) {
      return InvokeAsync<LobbyStatus>("accept", invitationId);
    }

    /// <summary>
    /// Declines an invitation to an arranged lobby 
    /// </summary>
    /// <param name="invitationId">The id of the invitation to decline, looks like INVID158928100</param>
    /// <returns></returns>
    public Task<LobbyStatus> Decline(string invitationId) {
      return InvokeAsync<LobbyStatus>("decline", invitationId);
    }

    public Task<LobbyStatus> CheckLobbyStatus() {
      return InvokeAsync<LobbyStatus>("checkLobbyStatus");
    }
  }
}
