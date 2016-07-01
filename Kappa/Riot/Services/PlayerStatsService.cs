using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class PlayerStatsService : Service {
    protected override string Destination => "playerStatsService";

    public PlayerStatsService(Session session) : base(session) { }

    /// <summary>
    /// Sends the skill of the player to the server when initially logging in to seed MMR.
    /// </summary>
    /// <param name="playerSkill">The skill of the player</param>
    /// <returns></returns>
    public Task<object> ProcessEloQuestionaire(PlayerSkill playerSkill) {
      return InvokeAsync<object>("processEloQuestionaire", playerSkill.ToString());
    }

    /// <summary>
    /// Gets the players overall stats
    /// </summary>
    /// <param name="accountId">The account id</param>
    /// <param name="season">The season you want to retrieve stats from</param>
    /// <returns>Returns the player stats for a season</returns>
    public Task<PlayerLifetimeStats> RetrievePlayerStatsByAccountId(long accountId, string season) {
      return InvokeAsync<PlayerLifetimeStats>("retrievePlayerStatsByAccountId", accountId, season);
    }

    /// <summary>
    /// Gets the top 3 played champions for a player
    /// </summary>
    /// <param name="accountId">The account id</param>
    /// <param name="gameMode">The game mode</param>
    /// <returns>Returns an array of the top 3 champions</returns>
    public Task<ChampionStatInfo[]> RetrieveTopPlayedChampions(long accountId, string gameMode) {
      return InvokeAsync<ChampionStatInfo[]>("retrieveTopPlayedChampions", accountId, gameMode);
    }

    /// <summary>
    /// Gets the aggregated stats of a players ranked games
    /// </summary>
    /// <param name="summonerId">The summoner id of a player</param>
    /// <param name="gameMode">The game mode requested</param>
    /// <param name="season">The season you want to retrieve stats from</param>
    /// <returns>Returns the aggregated stats requested</returns>
    public Task<AggregatedStats> GetAggregatedStats(long summonerId, string gameMode, string season) {
      return InvokeAsync<AggregatedStats>("getAggregatedStats", summonerId, gameMode, season);
    }

    ///// <summary>
    ///// Gets the top 10 recent games for a player
    ///// </summary>
    ///// <param name="AccountId">The account id of a player</param>
    ///// <returns>Returns the recent games for a player</returns>
    //public Task<RecentGames> GetRecentGames(Int64 AccountId) {
    //  return InvokeAsync<RecentGames>("getRecentGames", AccountId);
    //}

    /// <summary>
    /// Gets the aggregated stats for a team for all game modes
    /// </summary>
    /// <param name="teamId">The team id</param>
    /// <returns>Returns an array </returns>
    public Task<TeamAggregatedStatsDTO[]> GetTeamAggregatedStats(TeamId teamId) {
      return InvokeAsync<TeamAggregatedStatsDTO[]>("getTeamAggregatedStats", teamId);
    }

    /// <summary>
    /// Gets the end of game stats for a team for any game
    /// </summary>
    /// <param name="teamId">The team id</param>
    /// <param name="gameId">The game id</param>
    /// <returns>Returns the end of game stats for a game</returns>
    public Task<EndOfGameStats> GetTeamEndOfGameStats(TeamId teamId, double gameId) {
      return InvokeAsync<EndOfGameStats>("getTeamEndOfGameStats", teamId, gameId);
    }
  }
}
