using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class GameService : Service {
        protected override string Destination => "gameService";

        public GameService(Session session) : base(session) { }

        /// <summary>
        /// Gets all the practice games
        /// </summary>
        /// <returns>Returns an array of practice games</returns>
        public Task<PracticeGameSearchResult[]> ListAllPracticeGames() {
            return InvokeAsync<PracticeGameSearchResult[]>("listAllPracticeGames");
        }

        /// <summary>
        /// Joins a game
        /// </summary>
        /// <param name="gameId">The game id the user wants to join</param>
        public Task<object> JoinGame(long gameId) {
            return InvokeAsync<object>("joinGame", gameId, null);
        }

        /// <summary>
        /// Joins a private game
        /// </summary>
        /// <param name="gameId">The game id the user wants to join</param>
        /// <param name="password">The password of the game</param>
        /// <returns></returns>
        public Task<object> JoinGame(long gameId, string password) {
            return InvokeAsync<object>("joinGame", gameId, password);
        }

        public Task<object> ObserveGame(long gameId) {
            return InvokeAsync<object>("observeGame", gameId, null);
        }

        public Task<object> ObserveGame(long gameId, string password) {
            return InvokeAsync<object>("observeGame", gameId, password);
        }

        /// <summary>
        /// Switches the teams in a custom game
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <returns>Returns true if successful</returns>
        public Task<bool> SwitchTeams(long gameId) {
            return InvokeAsync<bool>("switchTeams", gameId);
        }

        /// <summary>
        /// Switches from a player to spectator
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <returns>Returns true if successful</returns>
        public Task<bool> SwitchPlayerToObserver(long gameId) {
            return InvokeAsync<bool>("switchPlayerToObserver", gameId);
        }

        /// <summary>
        /// Switches from a spectator to player
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <param name="team">Team id to switch to</param>
        /// <returns>Returns true if successful</returns>
        public Task<bool> SwitchObserverToPlayer(long gameId, int team) {
            return InvokeAsync<bool>("switchObserverToPlayer", gameId, team);
        }

        /// <summary>
        /// Quits from the current game
        /// </summary>
        public Task<object> QuitGame() {
            return InvokeAsync<object>("quitGame");
        }

        /// <summary>
        /// Creates a practice game.
        /// </summary>
        /// <param name="config">The configuration for the practice game</param>
        /// <returns>Returns a GameDTO if successfully created, otherwise null</returns>
        public Task<GameDTO> CreatePracticeGame(PracticeGameConfig config) {
            return InvokeAsync<GameDTO>("createPracticeGame", config);
        }

        /// <summary>
        /// Creates a practice game.
        /// </summary>
        /// <param name="queueId">??</param>
        /// <returns>Returns a GameDTO if successfully created, otherwise null</returns>
        public Task<GameDTO> CreateTutorialGame(int queueId) {
            return InvokeAsync<GameDTO>("createTutorialGame", queueId);
        }

        /// <summary>
        /// Starts champion selection for a custom game
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <param name="optomisticLock">The optomistic lock (provided by GameDTO)</param>
        /// <returns>Returns a StartChampSelectDTO</returns>
        public Task<StartChampSelectDTO> StartChampionSelection(long gameId, long optomisticLock) {
            return InvokeAsync<StartChampSelectDTO>("startChampionSelection", gameId, optomisticLock);
        }

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <param name="argument">The argument to be passed</param>
        public Task<object> SetClientReceivedGameMessage(long gameId, string argument) {
            return InvokeAsync<object>("setClientReceivedGameMessage", gameId, argument);
        }

        /// <summary>
        /// Get the current gametimer state
        /// </summary>
        /// <returns>The game timer state</returns>
        public Task<GameTimerDTO> GetCurrentTimerForGame() {
            return InvokeAsync<GameTimerDTO>("getCurrentTimerForGame");
        }

        /// <summary>
        /// Gets the latest GameDTO for a game
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <param name="gameState">The current game state</param>
        /// <param name="pickTurn">The current pick turn</param>
        /// <returns>Returns the latest GameDTO</returns>
        public Task<GameDTO> GetLatestGameTimerState(long gameId, GameState gameState, int pickTurn) {
            return InvokeAsync<GameDTO>("getLatestGameTimerState", gameId, gameState, pickTurn);
        }

        /// <summary>
        /// Selects the spells for a player for the current game
        /// </summary>
        /// <param name="spellOneId">The spell id for the first spell</param>
        /// <param name="spellTwoId">The spell id for the second spell</param>
        public Task<object> SelectSpells(int spellOneId, int spellTwoId) {
            return InvokeAsync<object>("selectSpells", spellOneId, spellTwoId);
        }
        /// <summary>
        /// Selects a champion for use
        /// </summary>
        /// <param name="championId">The selected champion id</param>
        public Task<object> SelectChampion(int championId) {
            return InvokeAsync<object>("selectChampion", championId);
        }

        /// <summary>
        /// Selects a champion skin for a champion
        /// </summary>
        /// <param name="championId">The selected champion id</param>
        /// <param name="skinId">The selected champion skin</param>
        public Task<object> SelectChampionSkin(int championId, int skinId) {
            return InvokeAsync<object>("selectChampionSkin", championId, skinId);
        }

        /// <summary>
        /// Lock in your champion selection
        /// </summary>
        public Task<object> ChampionSelectCompleted() {
            return InvokeAsync<object>("championSelectCompleted");
        }

        /// <summary>
        /// Gets the spectator game info for a summoner
        /// </summary>
        /// <returns>Returns the game info</returns>
        public Task<PlatformGameLifecycleDTO> RetrieveInProgressGameInfo() {
            return InvokeAsync<PlatformGameLifecycleDTO>("retrieveInProgressGameInfo");
        }

        /// <summary>
        /// Gets the spectator game info for a summoner
        /// </summary>
        /// <param name="summonerName">The summoner name</param>
        /// <returns>Returns the game info</returns>
        public Task<PlatformGameLifecycleDTO> RetrieveInProgressSpectatorGameInfo(string summonerName) {
            return InvokeAsync<PlatformGameLifecycleDTO>("retrieveInProgressSpectatorGameInfo", summonerName);
        }

        /// <summary>
        /// Accepts a popped queue
        /// </summary>
        /// <param name="acceptGame">Accept or decline the queue</param>
        public Task<object> AcceptPoppedGame(bool acceptGame) {
            return InvokeAsync<object>("acceptPoppedGame", acceptGame);
        }

        /// <summary>
        /// Bans a user from a custom game
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <param name="accountId">The account id of the player</param>
        public Task<object> BanUserFromGame(long gameId, long accountId) {
            return InvokeAsync<object>("banUserFromGame", gameId, accountId);
        }

        /// <summary>
        /// Bans a user from a custom game
        /// </summary>
        /// <param name="gameId">The game id</param>
        /// <param name="accountId">The account id of the player</param>
        public Task<object> BanObserverFromGame(long gameId, long accountId) {
            return InvokeAsync<object>("banObserverFromGame", gameId, accountId);
        }

        /// <summary>
        /// Bans a champion from the game (must be during PRE_CHAMP_SELECT and the users PickTurn)
        /// </summary>
        /// <param name="championId">The champion id</param>
        public Task<object> BanChampion(int championId) {
            return InvokeAsync<object>("banChampion", championId);
        }

        /// <summary>
        /// Gets the champions from the other team to ban
        /// </summary>
        /// <returns>Returns an array of champions to ban</returns>
        public Task<ChampionBanInfoDTO[]> GetChampionsForBan() {
            return InvokeAsync<ChampionBanInfoDTO[]>("getChampionsForBan");
        }
    }
}
