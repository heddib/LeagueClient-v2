interface BroadcastNotification {
}
interface BroadcastMessage {
    id: number;
    active: boolean;
    content: string;
    messageKey: string;
    severity: string;
}
interface ClientDynamicConfigurationNotification {
    configs: string;
    delta: boolean;
}
interface ClientSystemStatesNotification {
    championTradeThroughLCDS: boolean;
    practiceGameEnabled: boolean;
    advancedTutorialEnabled: boolean;
    minNumPlayersForPracticeGame: number;
    practiceGameTypeConfigIdList: number[];
    freeToPlayChampionIdList: number[];
    freeToPlayChampionForNewPlayersIdList: number[];
    freeToPlayChampionsForNewPlayersMaxLevel: number;
    inactiveChampionIdList: Object[];
    gameModeToInactiveSpellIds: any;
    inactiveSpellIdList: number[];
    inactiveTutorialSpellIdList: number[];
    inactiveClassicSpellIdList: number[];
    inactiveOdinSpellIdList: number[];
    inactiveAramSpellIdList: number[];
    enabledQueueIdsList: number[];
    unobtainableChampionSkinIDList: number[];
    archivedStatsEnabled: boolean;
    queueThrottleDTO: any;
    gameMapEnabledDTOList: any[];
    storeCustomerEnabled: boolean;
    runeUniquePerSpellBook: boolean;
    tribunalEnabled: boolean;
    observerModeEnabled: boolean;
    spectatorSlotLimit: number;
    clientHeartBeatRateSeconds: number;
    observableGameModes: string[];
    observableCustomGameModes: string;
    teamServiceEnabled: boolean;
    leagueServiceEnabled: boolean;
    modularGameModeEnabled: boolean;
    riotDataServiceDataSendProbability: number;
    displayPromoGamesPlayedEnabled: boolean;
    masteryPageOnServer: boolean;
    maxMasteryPagesOnServer: number;
    tournamentSendStatsEnabled: boolean;
    tournamentShortCodesEnabled: boolean;
    replayServiceAddress: string;
    kudosEnabled: boolean;
    buddyNotesEnabled: boolean;
    localeSpecificChatRoomsEnabled: boolean;
    replaySystemStates: any;
    sendFeedbackEventsEnabled: boolean;
    knownGeographicGameServerRegions: string[];
    leaguesDecayMessagingEnabled: boolean;
    currentSeason: number;
}
interface ClientVersionMismatchException {
    message: string;
    suppressed: Object[];
    rootCauseClassname: string;
    localizedMessage: string;
    cause: Object;
    substitutionArguments: Object[];
    errorCode: string;
}
interface UnexpectedServiceException {
    message: string;
    suppressed: Object[];
    rootCauseClassname: string;
    localizedMessage: string;
    cause: Object;
    substitutionArguments: Object[];
    errorCode: string;
}
interface FailedJoinPlayer {
    reasonFailed: string;
    summoner: Summoner;
}
interface QueueDodger extends FailedJoinPlayer {
    dodgePenaltyRemainingTime: number;
    reasonFailed: string;
    summoner: Summoner;
}
interface LcdsServiceProxyResponse {
    status: string;
    payload: string;
    messageId: string;
    methodName: string;
    serviceName: string;
}
interface GameParticipant {
    summonerName: string;
    summonerInternalName: string;
    pickMode: number;
    pickTurn: number;
    badges: number;
    isMe: boolean;
    isGameOwner: boolean;
    team: number;
    teamName: string;
    isFriendly: boolean;
    clubTag: string;
}
interface ObfuscatedParticipant extends GameParticipant {
    index: number;
    clientInSynch: boolean;
    gameUniqueId: number;
    summonerName: string;
    summonerInternalName: string;
    pickMode: number;
    pickTurn: number;
    badges: number;
    isMe: boolean;
    isGameOwner: boolean;
    team: number;
    teamName: string;
    isFriendly: boolean;
    clubTag: string;
}
interface ARAMPlayerParticipant extends PlayerParticipant {
    pointSummary: PointSummary;
    accountId: number;
    queueRating: number;
    botDifficulty: string;
    minor: boolean;
    locale: Object;
    lastSelectedSkinIndex: number;
    partnerId: string;
    profileIconId: number;
    rankedTeamGuest: boolean;
    summonerId: number;
    voterRating: number;
    selectedRole: Object;
    teamParticipantId: Object;
    timeAddedToQueue: Object;
    index: number;
    originalAccountNumber: number;
    adjustmentFlags: number;
    teamOwner: boolean;
    teamRating: number;
    clientInSynch: boolean;
    originalPlatformId: string;
    selectedPosition: Object;
    summonerName: string;
    summonerInternalName: string;
    pickMode: number;
    pickTurn: number;
    badges: number;
    isMe: boolean;
    isGameOwner: boolean;
    team: number;
    teamName: string;
    isFriendly: boolean;
    clubTag: string;
}
interface BotParticipant extends GameParticipant {
    botSkillLevel: number;
    botSkillLevelName: string;
    champion: ChampionDTO;
    teamId: string;
    summonerName: string;
    summonerInternalName: string;
    pickMode: number;
    pickTurn: number;
    badges: number;
    isMe: boolean;
    isGameOwner: boolean;
    team: number;
    teamName: string;
    isFriendly: boolean;
    clubTag: string;
}
interface PlayerParticipant extends GameParticipant {
    accountId: number;
    queueRating: number;
    botDifficulty: string;
    minor: boolean;
    locale: Object;
    lastSelectedSkinIndex: number;
    partnerId: string;
    profileIconId: number;
    rankedTeamGuest: boolean;
    summonerId: number;
    voterRating: number;
    selectedRole: Object;
    teamParticipantId: Object;
    timeAddedToQueue: Object;
    index: number;
    originalAccountNumber: number;
    adjustmentFlags: number;
    teamOwner: boolean;
    teamRating: number;
    clientInSynch: boolean;
    originalPlatformId: string;
    selectedPosition: Object;
    summonerName: string;
    summonerInternalName: string;
    pickMode: number;
    pickTurn: number;
    badges: number;
    isMe: boolean;
    isGameOwner: boolean;
    team: number;
    teamName: string;
    isFriendly: boolean;
    clubTag: string;
}
interface AuthenticationCredentials {
    oldPassword: string;
    clientVersion: string;
    password: string;
    partnerCredentials: Object;
    macAddress: string;
    domain: string;
    operatingSystem: string;
    securityAnswer: Object;
    locale: string;
    authToken: string;
    username: string;
}
interface AccountSummary {
    groupCount: number;
    username: string;
    accountId: number;
    summonerInternalName: string;
    dataVersion: number;
    admin: boolean;
    hasBetaAccess: boolean;
    summonerName: string;
    partnerMode: boolean;
    needsPasswordReset: boolean;
    futureData: Object;
}
interface LoginSession {
    token: string;
    password: string;
    accountSummary: AccountSummary;
}
interface SummaryAggStat {
    statType: string;
    count: number;
    dataVersion: number;
    value: number;
    futureData: Object;
}
interface SummaryAggStats {
    statsJson: Object;
    dataVersion: number;
    stats: SummaryAggStat[];
    futureData: Object;
}
interface PlayerStatSummary {
    maxRating: number;
    playerStatSummaryTypeString: string;
    aggregatedStats: SummaryAggStats;
    modifyDate: Date;
    leaves: number;
    futureData: Object;
    dataVersion: number;
    playerStatSummaryType: string;
    userId: number;
    losses: number;
    rating: number;
    aggregatedStatsJson: Object;
    wins: number;
}
interface PlayerStatSummaries {
    season: number;
    dataVersion: number;
    playerStatSummarySet: PlayerStatSummary[];
    userId: number;
    futureData: Object;
}
interface Talent {
    index: number;
    level5Desc: string;
    minLevel: number;
    maxRank: number;
    level4Desc: string;
    tltId: number;
    level3Desc: string;
    futureData: Object;
    talentGroupId: number;
    gameCode: number;
    minTier: number;
    prereqTalentGameCode: Object;
    dataVersion: number;
    level2Desc: string;
    name: string;
    talentRowId: number;
    level1Desc: string;
}
interface TalentRow {
    index: number;
    dataVersion: number;
    talents: Talent[];
    tltGroupId: number;
    maxPointsInRow: number;
    pointsToActivate: number;
    tltRowId: number;
    futureData: Object;
}
interface TalentGroup {
    index: number;
    talentRows: TalentRow[];
    dataVersion: number;
    name: string;
    tltGroupId: number;
    version: number;
    futureData: Object;
}
interface RuneType {
    runeTypeId: number;
    dataVersion: number;
    name: string;
    futureData: Object;
}
interface RuneSlot {
    id: number;
    minLevel: number;
    dataVersion: number;
    runeType: RuneType;
    futureData: Object;
}
interface SummonerCatalog {
    items: Object;
    talentTree: TalentGroup[];
    spellBookConfig: RuneSlot[];
}
interface Effect {
    effectId: number;
    gameCode: string;
    dataVersion: number;
    name: string;
    categoryId: Object;
    runeType: RuneType;
    futureData: Object;
}
interface ItemEffect {
    effectId: number;
    itemEffectId: number;
    effect: Effect;
    dataVersion: number;
    value: string;
    itemId: number;
    futureData: Object;
}
interface Rune {
    imagePath: Object;
    toolTip: Object;
    tier: number;
    itemId: number;
    runeType: RuneType;
    futureData: Object;
    duration: number;
    gameCode: number;
    itemEffects: ItemEffect[];
    baseType: string;
    dataVersion: number;
    description: string;
    name: string;
    uses: Object;
}
interface SlotEntry {
    dataVersion: number;
    runeId: number;
    runeSlotId: number;
    runeSlot: RuneSlot;
    rune: Rune;
    futureData: Object;
}
interface SpellBookPageDTO {
    dataVersion: number;
    pageId: number;
    name: string;
    current: boolean;
    slotEntries: SlotEntry[];
    createDate: Date;
    summonerId: number;
    futureData: Object;
}
interface SpellBookDTO {
    bookPagesJson: Object;
    dataVersion: number;
    bookPages: SpellBookPageDTO[];
    dateString: string;
    summonerId: number;
    futureData: Object;
}
interface SummonerDefaultSpells {
    dataVersion: number;
    summonerDefaultSpellsJson: Object;
    summonerDefaultSpellMap: any;
    summonerId: number;
    futureData: Object;
}
interface SummonerTalentsAndPoints {
    talentPoints: number;
    dataVersion: number;
    modifyDate: Date;
    createDate: Date;
    summonerId: number;
    futureData: Object;
}
interface Summoner {
    internalName: string;
    previousSeasonHighestTier: string;
    acctId: number;
    helpFlag: boolean;
    sumId: number;
    profileIconId: number;
    displayEloQuestionaire: boolean;
    lastGameDate: Date;
    previousSeasonHighestTeamReward: number;
    revisionDate: Date;
    advancedTutorialFlag: boolean;
    revisionId: number;
    futureData: Object;
    dataVersion: number;
    name: string;
    nameChangeFlag: boolean;
    tutorialFlag: boolean;
}
interface TalentEntry {
    rank: number;
    talentId: number;
    dataVersion: number;
    talent: Talent;
    summonerId: number;
    futureData: Object;
}
interface MasteryBookPageDTO {
    talentEntries: TalentEntry[];
    dataVersion: number;
    pageId: number;
    name: string;
    current: boolean;
    createDate: Object;
    summonerId: number;
    futureData: Object;
}
interface MasteryBookDTO {
    bookPagesJson: Object;
    dataVersion: number;
    bookPages: MasteryBookPageDTO[];
    dateString: string;
    summonerId: number;
    futureData: Object;
}
interface SummonerLevelAndPoints {
    infPoints: number;
    dataVersion: number;
    expPoints: number;
    summonerLevel: number;
    summonerId: number;
    futureData: Object;
}
interface SummonerLevel {
    expTierMod: number;
    grantRp: number;
    expForLoss: number;
    dataVersion: number;
    summonerTier: number;
    infTierMod: number;
    expToNextLevel: number;
    expForWin: number;
    summonerLevel: number;
    futureData: Object;
}
interface AllSummonerData {
    spellBook: SpellBookDTO;
    dataVersion: number;
    summonerDefaultSpells: SummonerDefaultSpells;
    summonerTalentsAndPoints: SummonerTalentsAndPoints;
    summoner: Summoner;
    masteryBook: MasteryBookDTO;
    summonerLevelAndPoints: SummonerLevelAndPoints;
    summonerLevel: SummonerLevel;
    futureData: Object;
}
interface PendingKudosDTO {
    pendingCounts: number[];
}
interface GameTypeConfigDTO {
    allowTrades: boolean;
    banTimerDuration: number;
    maxAllowableBans: number;
    crossTeamChampionPool: boolean;
    teamChampionPool: boolean;
    postPickTimerDuration: number;
    futureData: Object;
    id: number;
    duplicatePick: boolean;
    dataVersion: number;
    exclusivePick: boolean;
    mainPickTimerDuration: number;
    name: string;
    pickMode: string;
}
interface LoginDataPacket {
    restrictedGamesRemainingForRanked: number;
    playerStatSummaries: PlayerStatSummaries;
    restrictedChatGamesRemaining: number;
    minutesUntilShutdown: number;
    minor: boolean;
    maxPracticeGameSize: number;
    summonerCatalog: SummonerCatalog;
    ipBalance: number;
    reconnectInfo: PlatformGameLifecycleDTO;
    languages: string[];
    simpleMessages: Object[];
    allSummonerData: AllSummonerData;
    customMinutesLeftToday: number;
    displayPrimeReformCard: boolean;
    platformGameLifecycleDTO: PlatformGameLifecycleDTO;
    coOpVsAiMinutesLeftToday: number;
    bingeData: Object;
    inGhostGame: boolean;
    bingePreventionSystemEnabledForClient: boolean;
    pendingBadges: number;
    bannedUntilDateMillis: number;
    broadcastNotification: BroadcastNotification;
    minutesUntilMidnight: number;
    timeUntilFirstWinOfDay: number;
    coOpVsAiMsecsUntilReset: number;
    clientSystemStates: ClientSystemStatesNotification;
    bingeMinutesRemaining: number;
    pendingKudosDTO: PendingKudosDTO;
    leaverBusterPenaltyTime: number;
    platformId: string;
    emailStatus: string;
    matchMakingEnabled: boolean;
    minutesUntilShutdownEnabled: boolean;
    rpBalance: number;
    showEmailVerificationPopup: boolean;
    bingeIsPlayerInBingePreventionWindow: boolean;
    gameTypeConfigs: GameTypeConfigDTO[];
    minorShutdownEnforced: boolean;
    competitiveRegion: string;
    customMsecsUntilReset: number;
}
interface MatchingThrottleConfig {
    limit: number;
    matchingThrottleProperties: Object[];
    dataVersion: number;
    cacheName: string;
    futureData: Object;
}
interface GameQueueConfig {
    blockedMinutesThreshold: number;
    ranked: boolean;
    minimumParticipantListSize: number;
    maxLevel: number;
    thresholdEnabled: boolean;
    gameTypeConfigId: number;
    minLevel: number;
    queueState: string;
    type: string;
    cacheName: string;
    id: number;
    queueBonusKey: string;
    dataVersion: number;
    maxSummonerLevelForFirstWinOfTheDay: number;
    queueStateString: string;
    pointsConfigKey: string;
    teamOnly: boolean;
    minimumQueueDodgeDelayTime: number;
    randomizeTeamSides: boolean;
    supportedMapIds: number[];
    futureData: Object;
    gameMode: string;
    typeString: string;
    numPlayersPerTeam: number;
    disallowFreeChampions: boolean;
    maximumParticipantListSize: number;
    mapSelectionAlgorithm: string;
    botsCanSpawnOnBlueSide: boolean;
    gameMutators: string[];
    thresholdSize: number;
    matchingThrottleConfig: MatchingThrottleConfig;
}
interface SummonerActiveBoostsDTO {
    xpBoostEndDate: number;
    xpBoostPerWinCount: number;
    xpLoyaltyBoost: number;
    ipBoostPerWinCount: number;
    ipLoyaltyBoost: number;
    summonerId: number;
    ipBoostEndDate: number;
}
interface ChampionSkinDTO {
    lastSelected: boolean;
    stillObtainable: boolean;
    purchaseDate: number;
    winCountRemaining: number;
    endDate: number;
    championId: number;
    freeToPlayReward: boolean;
    sources: Object[];
    skinId: number;
    owned: boolean;
}
interface ChampionDTO {
    rankedPlayEnabled: boolean;
    winCountRemaining: number;
    botEnabled: boolean;
    endDate: number;
    freeToPlayReward: boolean;
    sources: Object[];
    owned: boolean;
    purchased: number;
    championSkins: ChampionSkinDTO[];
    purchaseDate: number;
    active: boolean;
    championId: number;
    freeToPlay: boolean;
}
interface SummonerRune {
    purchased: Date;
    dataVersion: number;
    purchaseDate: Date;
    runeId: number;
    quantity: number;
    rune: Rune;
    summonerId: number;
    futureData: Object;
}
interface SummonerRuneInventory {
    dataVersion: number;
    summonerRunesJson: Object;
    dateString: string;
    summonerRunes: SummonerRune[];
    summonerId: number;
    futureData: Object;
}
interface LeagueItemDTO {
    previousDayLeaguePosition: number;
    timeLastDecayMessageShown: number;
    seasonEndTier: string;
    seasonEndRank: string;
    hotStreak: boolean;
    leagueName: string;
    miniSeries: Object;
    tier: string;
    freshBlood: boolean;
    lastPlayed: number;
    timeUntilInactivityStatusChanges: number;
    inactivityStatus: string;
    playerOrTeamId: string;
    leaguePoints: number;
    demotionWarning: number;
    inactive: boolean;
    seasonEndApexPosition: number;
    rank: string;
    veteran: boolean;
    queueType: string;
    losses: number;
    timeUntilDecay: number;
    displayDecayWarning: boolean;
    playerOrTeamName: string;
    wins: number;
}
interface SummonerLeagueItemsDTO {
    summonerLeagues: LeagueItemDTO[];
}
interface TeamId {
    fullId: string;
}
interface TeamStatDetail {
    inSeasonWins: number;
    maxRating: number;
    seedRating: number;
    averageGamesPlayed: number;
    teamId: TeamId;
    futureData: Object;
    inSeasonLosses: number;
    dataVersion: number;
    teamIdString: string;
    seasonId: number;
    losses: number;
    rating: number;
    teamStatTypeString: string;
    wins: number;
    teamStatType: string;
}
interface TeamStatSummary {
    dataVersion: number;
    teamStatDetails: TeamStatDetail[];
    teamIdString: string;
    teamId: TeamId;
    futureData: Object;
}
interface MatchHistorySummary {
    gameMode: string;
    mapId: number;
    assists: number;
    opposingTeamName: string;
    invalid: boolean;
    deaths: number;
    gameId: number;
    kills: number;
    win: boolean;
    date: number;
    opposingTeamKills: number;
}
interface TeamMemberInfoDTO {
    joinDate: Date;
    playerName: Object;
    inviteDate: Date;
    status: string;
    playerId: number;
}
interface RosterDTO {
    ownerId: number;
    memberList: TeamMemberInfoDTO[];
}
interface TeamDTO {
    secondsUntilEligibleForDeletion: number;
    secondLastJoinDate: Date;
    lastJoinDate: Date;
    teamStatSummary: TeamStatSummary;
    matchHistory: MatchHistorySummary[];
    status: string;
    tag: string;
    name: string;
    thirdLastJoinDate: Date;
    roster: RosterDTO;
    lastGameDate: Date;
    modifyDate: Date;
    messageOfDay: Object;
    createDate: Date;
    teamId: TeamId;
}
interface TeamInfo {
    secondsUntilEligibleForDeletion: number;
    memberStatusString: string;
    dataVersion: number;
    tag: string;
    name: string;
    memberStatus: string;
    teamId: TeamId;
    futureData: Object;
}
interface PlayerDTO {
    playerId: number;
    teamsSummary: TeamDTO[];
    createdTeams: Object[];
    playerTeams: TeamInfo[];
}
interface SummonerSummary {
    id: number;
    internalName: string;
    level: number;
    dataVersion: number;
    name: string;
    losses: number;
    leaves: number;
    wins: number;
    futureData: Object;
}
interface PublicSummoner {
    internalName: string;
    dataVersion: number;
    acctId: number;
    name: string;
    profileIconId: number;
    revisionDate: Date;
    revisionId: number;
    summonerLevel: number;
    summonerId: number;
    futureData: Object;
}
interface Player {
    summonerName: string;
    summonerId: number;
}
interface Member {
    hasDelegatedInvitePower: boolean;
    summonerName: string;
    summonerId: number;
}
interface Invitee {
    inviteeStateAsString: string;
    summonerName: string;
    inviteeState: string;
    summonerId: number;
}
interface LobbyStatus {
    chatKey: string;
    gameMetaData: string;
    owner: Player;
    members: Member[];
    invitees: Invitee[];
    invitationId: string;
}
interface LeagueListDTO {
    queue: string;
    name: string;
    tier: string;
    requestorsRank: string;
    entries: LeagueItemDTO[];
    nextApexUpdate: Object;
    maxLeagueSize: number;
    requestorsName: string;
}
interface SummonerLeaguesDTO {
    summonerLeagues: LeagueListDTO[];
}
interface GameMap {
    displayName: string;
    name: string;
    mapId: number;
    totalPlayers: number;
    description: string;
    minCustomPlayers: number;
    futureData: Object;
    dataVersion: Object;
}
interface PracticeGameConfig {
    passbackDataPacket: Object;
    gameName: string;
    gameMode: string;
    allowSpectators: string;
    region: string;
    gameTypeConfig: number;
    gamePassword: string;
    maxNumPlayers: number;
    gameMap: GameMap;
    gameMutators: Object[];
    passbackUrl: Object;
}
interface FeaturedGameInfo {
    dataVersion: number;
    championVoteInfoList: Object[];
    futureData: Object;
}
interface PlayerChampionSelectionDTO {
    summonerInternalName: string;
    dataVersion: number;
    spell2Id: number;
    selectedSkinIndex: number;
    championId: number;
    spell1Id: number;
    futureData: Object;
}
interface GameDTO {
    spectatorsAllowed: string;
    passwordSet: boolean;
    practiceGameRewardsDisabledReasons: string[];
    gameType: string;
    gameTypeConfigId: number;
    gameState: number;
    observers: Object[];
    statusOfParticipants: string;
    id: number;
    ownerSummary: PlayerParticipant;
    teamTwoPickOutcome: Object;
    teamTwo: GameParticipant[];
    bannedChampions: BannedChampion[];
    dataVersion: number;
    roomName: string;
    name: string;
    spectatorDelay: number;
    teamOne: GameParticipant[];
    terminatedCondition: string;
    queueTypeName: string;
    featuredGameInfo: FeaturedGameInfo;
    passbackUrl: Object;
    roomPassword: string;
    optimisticLock: number;
    teamOnePickOutcome: Object;
    maxNumPlayers: number;
    queuePosition: number;
    terminatedConditionString: string;
    futureData: Object;
    gameMode: string;
    expiryTime: number;
    mapId: number;
    banOrder: number[];
    gameStateString: string;
    pickTurn: number;
    playerChampionSelections: PlayerChampionSelectionDTO[];
    gameMutators: string[];
    joinTimerDuration: number;
    passbackDataPacket: Object;
}
interface StartChampSelectDTO {
    invalidPlayers: Object[];
}
interface GameTimerDTO {
    remainingTimeInMillis: number;
    currentGameState: string;
}
interface LcdsResponseString {
    value: string;
}
interface PointSummary {
    pointsToNextRoll: number;
    maxRolls: number;
    numberOfRolls: number;
    pointsCostToRoll: number;
    currentPoints: number;
}
interface TimeTrackedStat {
    timestamp: Date;
    dataVersion: number;
    type: string;
    futureData: Object;
}
interface PlayerStats {
    timeTrackedStats: TimeTrackedStat[];
    promoGamesPlayed: number;
    dataVersion: number;
    promoGamesPlayedLastUpdated: Object;
    lifetimeGamesPlayed: any;
    futureData: Object;
}
interface PlayerLifetimeStats {
    playerStatSummaries: PlayerStatSummaries;
    dataVersion: number;
    previousFirstWinOfDay: Date;
    userId: number;
    dodgeStreak: number;
    dodgePenaltyDate: Object;
    playerStatsJson: Object;
    playerStats: PlayerStats;
    futureData: Object;
}
interface AggregatedStat {
    statType: string;
    count: number;
    dataVersion: number;
    value: number;
    championId: number;
    futureData: Object;
}
interface ChampionStatInfo {
    totalGamesPlayed: number;
    accountId: number;
    dataVersion: number;
    stats: AggregatedStat[];
    championId: number;
    futureData: Object;
}
interface AggregatedStatsKey {
    gameMode: string;
    season: number;
    dataVersion: number;
    userId: number;
    gameModeString: string;
    futureData: Object;
}
interface AggregatedStats {
    lifetimeStatistics: AggregatedStat[];
    dataVersion: number;
    modifyDate: Date;
    key: AggregatedStatsKey;
    aggregatedStatsJson: Object;
    futureData: Object;
}
interface TeamPlayerAggregatedStatsDTO {
    postSeasonGamesPlayed: number;
    playerId: number;
    postSeasonWins: number;
    seasonId: number;
    aggregatedStats: AggregatedStats;
    teamId: TeamId;
    teamStatType: string;
}
interface TeamAggregatedStatsDTO {
    queueType: string;
    serializedToJson: string;
    playerAggregatedStatsList: TeamPlayerAggregatedStatsDTO[];
    teamId: TeamId;
}
interface RawStatDTO {
    dataVersion: number;
    value: number;
    statTypeName: string;
    futureData: Object;
}
interface PlayerParticipantStatsSummary {
    skinName: string;
    gameId: number;
    profileIconId: number;
    elo: number;
    leaver: boolean;
    leaves: number;
    teamId: number;
    statistics: RawStatDTO[];
    eloChange: number;
    level: number;
    botPlayer: boolean;
    userId: number;
    spell2Id: number;
    losses: number;
    summonerName: string;
    championId: number;
    wins: number;
    spell1Id: number;
}
interface EndOfGameStats {
    battleBoostIpEarned: number;
    ranked: boolean;
    talentPointsGained: number;
    skinIndex: number;
    basePoints: number;
    teamPlayerParticipantStats: PlayerParticipantStatsSummary[];
    difficulty: string;
    partyRewardsBonusIpEarned: number;
    boostXpEarned: number;
    invalid: boolean;
    roomName: Object;
    userId: Object;
    rpEarned: number;
    experienceTotal: number;
    gameId: number;
    loyaltyBoostXpEarned: number;
    elo: number;
    roomPassword: Object;
    firstWinBonus: number;
    eloChange: number;
    myTeamInfo: TeamInfo;
    summonerName: string;
    customMsecsUntilReset: number;
    leveledUp: boolean;
    gameType: string;
    queueBonusEarned: number;
    imbalancedTeamsNoPoints: boolean;
    experienceEarned: number;
    reportGameId: Object;
    gameLength: number;
    otherTeamInfo: TeamInfo;
    customMinutesLeftToday: number;
    coOpVsAiMinutesLeftToday: number;
    pointsPenalties: Object[];
    otherTeamPlayerParticipantStats: PlayerParticipantStatsSummary[];
    loyaltyBoostIpEarned: number;
    boostIpEarned: number;
    coOpVsAiMsecsUntilReset: number;
    completionBonusPoints: number;
    newSpells: Object[];
    timeUntilNextFirstWinBonus: number;
    ipEarned: number;
    sendStatsToTournamentProvider: boolean;
    gameMode: string;
    gameMutators: Object[];
    odinBonusIp: number;
    queueType: string;
    myTeamStatus: string;
    ipTotal: number;
}
interface Inviter {
    previousSeasonHighestTier: string;
    summonerName: string;
    summonerId: number;
}
interface InvitationRequest {
    invitePayload: string;
    inviter: Inviter;
    inviteType: number;
    gameMetaData: string;
    owner: Player;
    invitationStateAsString: string;
    invitationState: number;
    inviteTypeAsString: string;
    invitationId: string;
}
interface IconType {
    iconTypeId: number;
    dataVersion: number;
    name: string;
    futureData: Object;
}
interface Icon {
    imagePath: Object;
    toolTip: Object;
    tier: Object;
    itemId: number;
    futureData: Object;
    duration: Object;
    gameCode: number;
    itemEffects: Object;
    baseType: string;
    dataVersion: number;
    description: Object;
    name: string;
    uses: Object;
    iconType: IconType;
}
interface SummonerIcon {
    icon: Icon;
    dataVersion: number;
    purchaseDate: Date;
    iconId: number;
    summonerId: number;
    futureData: Object;
}
interface SummonerIconInventoryDTO {
    summonerIcons: SummonerIcon[];
    dataVersion: number;
    dateString: string;
    summonerId: number;
    summonerIconJson: Object;
    futureData: Object;
}
interface ChampionBanInfoDTO {
    dataVersion: number;
    enemyOwned: boolean;
    championId: number;
    owned: boolean;
    futureData: Object;
}
interface PracticeGameSearchResult {
    spectatorCount: number;
    glmGameId: Object;
    glmHost: Object;
    glmPort: number;
    gameModeString: string;
    allowSpectators: string;
    gameMapId: number;
    maxNumPlayers: number;
    glmSecurePort: number;
    gameMode: string;
    id: number;
    gameMutators: string[];
    privateGame: boolean;
    name: string;
    owner: PlayerParticipant;
    team1Count: number;
    team2Count: number;
}
interface MatchMakerParams {
    languages: Object;
    queueIds: number[];
    invitationId: Object;
    teamId: Object;
    lastMaestroMessage: Object;
    team: number[];
    botDifficulty: string;
}
interface QueueInfo {
    waitTime: number;
    queueId: number;
    queueLength: number;
}
interface SearchingForMatchNotification {
    ghostGameSummoners: Object[];
    joinedQueues: QueueInfo[];
    playerJoinFailures: FailedJoinPlayer[];
}
interface StoreFulfillmentNotification {
    rp: number;
    inventoryType: string;
    data: ChampionDTO;
    ip: number;
}
interface StoreAccountBalanceNotification {
    rp: number;
    ip: number;
}
interface PlayerCredentialsDto {
    encryptionKey: string;
    gameId: number;
    serverIp: string;
    lastSelectedSkinIndex: number;
    observer: boolean;
    summonerId: number;
    futureData: Object;
    observerServerIp: string;
    dataVersion: number;
    handshakeToken: string;
    playerId: number;
    serverPort: number;
    observerServerPort: number;
    summonerName: string;
    observerEncryptionKey: string;
    championId: number;
}
interface ClientLoginKickNotification {
    sessionToken: string;
}
interface MiniSeriesDTO {
    progress: string;
    target: number;
    losses: number;
    timeLeftToPlayMillis: number;
    wins: number;
}
interface BannedChampion {
    pickTurn: number;
    dataVersion: number;
    championId: number;
    teamId: number;
    futureData: Object;
}
interface InvitePrivileges {
    canInvite: boolean;
}
interface SummonerGameModeSpells {
    spell2Id: number;
    spell1Id: number;
}
interface GameNotification {
    messageCode: string;
    messageArgument: string;
    type: number;
}
interface TradeContractDTO {
    responderInternalSummonerName: string;
    requesterChampionId: number;
    state: string;
    requesterInternalSummonerName: string;
    responderChampionId: number;
}
interface SimpleDialogMessage {
    titleCode: string;
    accountId: number;
    msgId: string;
    params: Object[];
    type: string;
    bodyCode: string;
}
interface PlatformGameLifecycleDTO {
    gameSpecificLoyaltyRewards: Object;
    reconnectDelay: number;
    lastModifiedDate: Object;
    game: GameDTO;
    playerCredentials: PlayerCredentialsDto;
    gameName: string;
    connectivityStateEnum: string;
}
interface ChampionMasteryDTO {
    championId: number;
    playerId: number;
    championLevel: number;
    championPoints: number;
    lastPlayTime: number;
    championPointsSinceLastLevel: number;
    championPointsUntilNextLevel: number;
}
interface InvitationMetaData {
    mapId: number;
    gameMode: string;
    gameMutators: string[];
    gameType: string;
    rankedTeamName: string;
    rankedTeamId: string;
    queueId: number;
    isRanked: boolean;
    botDifficulty: string;
    gameTypeConfigId: number;
    groupFinderId: string;
    gameId: number;
}
interface LoginQueueDto {
    rate: number;
    token: string;
    reason: string;
    delay: number;
    inGameCredentials: InGameCredentials;
    user: string;
    idToken: string;
    vcap: number;
    status: string;
    gasToken: any;
    node: number;
    tickers: any[];
    backlog: number;
    champ: number;
}
interface InGameCredentials {
    inGame: boolean;
    summonerId: number;
    serverIp: string;
    serverPort: number;
    encryptionKey: string;
    handshakeToken: string;
}

namespace MatchHistory {
    export interface PlayerDeltas {
        originalAccountId: number;
        originalPlatformId: string;
        deltas: GameDeltaInfo[];
    }
    export interface GameDeltaInfo {
        gamePlatformId: string;
        gameId: number;
        platformDelta: Delta;
    }
    export interface Delta {
        gamePlatformId: string;
        gameId: number;
        xpDelta: number;
        ipDelta: number;
        compensationModeEnabled: boolean;
        timestamp: number;
    }
    export interface PlayerHistory {
        platformId: string;
        accountId: number;
        shownQueues: number[];
        games: GameResponseInfo;
    }
    export interface GameResponseInfo {
        gameIndexBegin: number;
        gameIndexEnd: number;
        gameTimestampBegin: number;
        gameTimestampEnd: number;
        gameCount: number;
        games: MatchDetails[];
    }
    export interface MatchDetails {
        gameId: number;
        platformId: string;
        gameCreation: number;
        gameDuration: number;
        queueId: number;
        mapId: number;
        seasonId: number;
        gameVersion: string;
        gameMode: string;
        gameType: string;
        teams: Team[];
        participants: Participant[];
        participantIdentities: ParticipantIdentity[];
    }
    export interface Participant {
        participantId: number;
        teamId: number;
        championId: number;
        spell1Id: number;
        spell2Id: number;
        masteries: any[];
        runes: any[];
        stats: ParticipantStats;
        timeline: any;
        highestAchievedSeasonTier: string;
    }
    export interface ParticipantStats {
        participantId: number;
        win: boolean;
        item0: number;
        item1: number;
        item2: number;
        item3: number;
        item4: number;
        item5: number;
        item6: number;
        kills: number;
        deaths: number;
        assists: number;
        largestKillingSpree: number;
        largestMultiKill: number;
        killingSprees: number;
        longestTimeSpentLiving: number;
        doubleKills: number;
        tripleKills: number;
        quadraKills: number;
        pentaKills: number;
        unrealKills: number;
        totalDamageDealt: number;
        magicDamageDealt: number;
        physicalDamageDealt: number;
        trueDamageDealt: number;
        largestCriticalStrike: number;
        totalDamageDealtToChampions: number;
        magicDamageDealtToChampions: number;
        physicalDamageDealtToChampions: number;
        trueDamageDealtToChampions: number;
        totalHeal: number;
        totalUnitsHealed: number;
        totalDamageTaken: number;
        magicalDamageTaken: number;
        physicalDamageTaken: number;
        trueDamageTaken: number;
        goldEarned: number;
        goldSpent: number;
        turretKills: number;
        inhibitorKills: number;
        totalMinionsKilled: number;
        neutralMinionsKilled: number;
        neutralMinionsKilledTeamJungle: number;
        neutralMinionsKilledEnemyJungle: number;
        totalTimeCrowdControlDealt: number;
        champLevel: number;
        visionWardsBoughtInGame: number;
        sightWardsBoughtInGame: number;
        wardsPlaced: number;
        wardsKilled: number;
        firstBloodKill: boolean;
        firstBloodAssist: boolean;
        firstTowerKill: boolean;
        firstTowerAssist: boolean;
        firstInhibitorKill: boolean;
        firstInhibitorAssist: boolean;
        combatPlayerScore: number;
        objectivePlayerScore: number;
        totalPlayerScore: number;
        totalScoreRank: number;
        wasAfk: boolean;
        leaver: boolean;
        gameEndedInEarlySurrender: boolean;
        gameEndedInSurrender: boolean;
        causedEarlySurrender: boolean;
        earlySurrenderAccomplice: boolean;
        teamEarlySurrendered: boolean;
    }
    export interface Team {
        teamId: number;
        win: string;
        firstBlood: boolean;
        firstTower: boolean;
        firstInhibitor: boolean;
        firstBaron: boolean;
        firstDragon: boolean;
        towerKills: number;
        inhibitorKills: number;
        baronKills: number;
        dragonKills: number;
        vilemawKills: number;
        dominionVictoryScore: number;
        bans: BannedChampion[];
    }
    export interface ParticipantIdentity {
        participantId: number;
        player: Player;
    }
    export interface Player {
        platformId: number;
        accountId: number;
        summonerName: string;
        summonerId: number;
        currentPlatformId: string;
        currentAccountId: number;
        matchHistoryUri: string;
        profileIcon: number;
    }
}

namespace ActiveGame {
    export interface ActiveGameState {
        ingame: boolean;
        launched: boolean;
    }
}

namespace Summoner {
    export interface SummonerKudos {
        friendlies: number;
        helpfuls: number;
        teamworks: number;
        honorables: number;
    }
}

//Patcher Service:
interface PatcherState {
    phase: string;
    current: number;
    total: number;
}

//Collection Service:
interface HextechInventory {
    champShards: { [id: number]: number };
    champs: { [id: number]: number };
    skinShards: { [id: number]: number };
    skins: { [id: number]: number };
    wardSkinShards: { [id: number]: number };
    wardSkins: { [id: number]: number };
    mastery6Tokens: { [id: number]: number };
    mastery7Tokens: { [id: number]: number };
    chests: number;
    keys: number;
    keyFragments: number;
    blueEssence: number;
    orangeEssence: number;
}
interface ChampionInventory {
    owned: number[];
    free: number[];
}
interface Skin {
    id: number;
    selected: boolean;
}

//Chat Service:
interface ChatMessage {
    user: string;
    received: boolean;
    body: string;
    date: Date;
    archived: boolean;
}
interface MucFriend {
    room: string;
    name: string;
}
interface MucMessage {
    room: string;
    from: string;
    body: string;
}

//PlayLoop Service:
interface AvailableQueue {
    config: number;
    name: string;
    id: number;
}
interface RerollState {
    cost: number;
    points: number;
    maxPoints: number;
}
interface CurrentPlayLoopState {
    inPlayLoop: boolean;
    queueId: number;
    queueConfigId: number;
}
interface ChampSelectState {
    phase: string;
    alliedBans: number[];
    enemyBans: number[];
    isBlue: boolean;
    allies: GameMember[];
    enemies: GameMember[];
    me: GameMember;
    remaining: number;
    turn: number;
    inventory: Inventory;
    reroll: RerollState;
    chatroom: string;
}
interface CustomState {
    blueTeam: LobbyMember[];
    redTeam: LobbyMember[];
    owner: LobbyMember;
    me: LobbyMember;
    chatroom: string;
}
interface GameMember {
    name: string;
    champion: number;
    spell1: number;
    spell2: number;
    id: Object;
    active: boolean;
    trade: string;
    reroll: RerollState;
    intent: boolean;
    role: string;
}
interface Inventory {
    pickableChamps: number[];
    bannableChamps: number[];
    availableSpells: number[];
}
interface LobbyMember {
    name: string;
    id: Object;
    champ: number;
    role1: string;
    role2: string;
}
interface LobbyState {
    isCaptain: boolean;
    canInvite: boolean;
    canMatch: boolean;
    members: LobbyMember[];
    me: LobbyMember;
    chatroom: string;
}
interface MatchmakingState {
    estimate: number;
    actual: number;
    afkCheck: AfkCheck;
    chatroom: string;
}
interface AfkCheck {
    accepted: boolean;
    duration: number;
    remaining: number;
}