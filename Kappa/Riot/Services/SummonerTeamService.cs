using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class SummonerTeamService : Service {
        protected override string Destination => "summonerTeamService";

        public SummonerTeamService(Session session) : base(session) { }

        /// <summary>
        /// 
        /// </summary>
        public Task<PlayerDTO> CreatePlayer() {
            return InvokeAsync<PlayerDTO>("createPlayer");
        }

        /// <summary>
        /// Find a team by the TeamId
        /// </summary>
        /// <param name="teamId">The team Id</param>
        /// <returns>Returns the information for a team</returns>
        public Task<TeamDTO> FindTeamById(TeamId teamId) {
            return InvokeAsync<TeamDTO>("findTeamById", teamId);
        }

        /// <summary>
        /// Find a team by name
        /// </summary>
        /// <param name="teamName">The team name</param>
        /// <returns>Returns the information for a team</returns>
        public Task<TeamDTO> FindTeamByName(string teamName) {
            return InvokeAsync<TeamDTO>("findTeamByName", teamName);
        }

        /// <summary>
        /// Disbands a team
        /// </summary>
        /// <param name="teamId">The team Id</param>
        public Task<object> DisbandTeam(TeamId teamId) {
            return InvokeAsync<object>("disbandTeam", teamId);
        }

        /// <summary>
        /// Checks if a name is available 
        /// </summary>
        /// <param name="teamName">The name that you want to validate</param>
        /// <returns>Returns a boolean as the result</returns>
        public Task<bool> IsTeamNameValidAndAvailable(string teamName) {
            return InvokeAsync<bool>("isNameValidAndAvailable", teamName);
        }

        /// <summary>
        /// Checks if a tag is available 
        /// </summary>
        /// <param name="tagName">The tag that you want to validate</param>
        /// <returns>Returns a boolean as the result</returns>
        public Task<bool> IsTeamTagValidAndAvailable(string tagName) {
            return InvokeAsync<bool>("isTagValidAndAvailable", tagName);
        }

        /// <summary>
        /// Creates a ranked team if the name and tag is valid
        /// </summary>
        /// <param name="teamName">The team name</param>
        /// <param name="tagName">The tag name</param>
        /// <returns>Returns the information for a team</returns>
        public Task<TeamDTO> CreateTeam(string teamName, string tagName) {
            return InvokeAsync<TeamDTO>("createTeam", teamName, tagName);
        }

        /// <summary>
        /// Invites a player to a ranked team
        /// </summary>
        /// <param name="summonerId">The summoner id of the player you want to invite</param>
        /// <param name="teamId">The team id</param>
        /// <returns>Returns the information for a team</returns>
        public Task<TeamDTO> TeamInvitePlayer(long summonerId, TeamId teamId) {
            return InvokeAsync<TeamDTO>("invitePlayer", summonerId, teamId);
        }

        /// <summary>
        /// Kicks a player from a ranked team
        /// </summary>
        /// <param name="summonerId">The summoner id of the player you want to kick</param>
        /// <param name="teamId">The team id</param>
        /// <returns>Returns the information for a team</returns>
        public Task<TeamDTO> KickPlayer(long summonerId, TeamId teamId) {
            return InvokeAsync<TeamDTO>("kickPlayer", summonerId, teamId);
        }

        /// <summary>
        /// Finds a player by Summoner Id
        /// </summary>
        /// <param name="summonerId">The summoner id</param>
        /// <returns>Returns the information for a player</returns>
        public Task<PlayerDTO> FindPlayer(long summonerId) {
            return InvokeAsync<PlayerDTO>("findPlayer", summonerId);
        }
    }
}
