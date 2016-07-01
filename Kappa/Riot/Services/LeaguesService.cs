using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class LeaguesService : Service {
    protected override string Destination => "leaguesServiceProxy";

    public LeaguesService(Session session) : base(session) { }

    /// <summary>
    /// Gets the league positions for the user
    /// </summary>
    /// <returns>Returns the league positions for a user</returns>
    public Task<SummonerLeagueItemsDTO> GetMyLeaguePositions() {
      return InvokeAsync<SummonerLeagueItemsDTO>("getMyLeaguePositions");
    }

    /// <summary>
    /// Gets the top 50 players for a queue type
    /// </summary>
    /// <param name="queueType">Queue type</param>
    /// <returns>Returns the top 50 players league info</returns>
    public Task<SummonerLeaguesDTO> GetChallengerLeague(string queueType) {
      return InvokeAsync<SummonerLeaguesDTO>("getChallengerLeague", queueType);
    }

    /// <summary>
    /// Gets the current leagues for a user's tier (e.g Gold)
    /// </summary>
    /// <returns>Returns the leagues for a user</returns>
    public Task<SummonerLeaguesDTO> GetAllMyLeagues() {
      return InvokeAsync<SummonerLeaguesDTO>("getAllMyLeagues");
    }

    /// <summary>
    /// Gets the league for a team
    /// </summary>
    /// <param name="teamName">The team name</param>
    /// <returns>Returns the league information for a team</returns>
    public Task<SummonerLeaguesDTO> GetLeaguesForTeam(string teamName) {
      return InvokeAsync<SummonerLeaguesDTO>("getLeaguesForTeam", teamName);
    }

    /// <summary>
    /// Get the leagues for a player
    /// </summary>
    /// <param name="summonerId">The summoner id of the player</param>
    /// <returns>Returns the league information for a team</returns>
    public Task<SummonerLeaguesDTO> GetAllLeaguesForPlayer(long summonerId) {
      return InvokeAsync<SummonerLeaguesDTO>("getAllLeaguesForPlayer", summonerId);
    }
  }
}
