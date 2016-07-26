using RtmpSharp.IO;
using System;
using System.Collections.Generic;

namespace Kappa.Riot.Domain {
    [Serializable, SerializedName("com.riotgames.platform.login.AuthenticationCredentials")]
    public class AuthenticationCredentials {
        [SerializedName("oldPassword")]
        public string OldPassword { get; set; }
        [SerializedName("clientVersion")]
        public string ClientVersion { get; set; }
        [SerializedName("password")]
        public string Password { get; set; }
        [SerializedName("partnerCredentials")]
        public object PartnerCredentials { get; set; }
        [SerializedName("macAddress")]
        public string MacAddress { get; set; }
        [SerializedName("domain")]
        public string Domain { get; set; }
        [SerializedName("operatingSystem")]
        public string OperatingSystem { get; set; }
        [SerializedName("securityAnswer")]
        public object SecurityAnswer { get; set; }
        [SerializedName("locale")]
        public string Locale { get; set; }
        [SerializedName("authToken")]
        public string AuthToken { get; set; }
        [SerializedName("username")]
        public string Username { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.account.AccountSummary")]
    public class AccountSummary {
        [SerializedName("groupCount")]
        public int GroupCount { get; set; }
        [SerializedName("username")]
        public string Username { get; set; }
        [SerializedName("accountId")]
        public long AccountId { get; set; }
        [SerializedName("summonerInternalName")]
        public string SummonerInternalName { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("admin")]
        public bool Admin { get; set; }
        [SerializedName("hasBetaAccess")]
        public bool HasBetaAccess { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("partnerMode")]
        public bool PartnerMode { get; set; }
        [SerializedName("needsPasswordReset")]
        public bool NeedsPasswordReset { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.login.Session")]
    public class LoginSession {
        [SerializedName("token")]
        public string Token { get; set; }
        [SerializedName("password")]
        public string Password { get; set; }
        [SerializedName("accountSummary")]
        public AccountSummary AccountSummary { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.SummaryAggStat")]
    public class SummaryAggStat {
        [SerializedName("statType")]
        public string StatType { get; set; }
        [SerializedName("count")]
        public int Count { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("value")]
        public int Value { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.SummaryAggStats")]
    public class SummaryAggStats {
        [SerializedName("statsJson")]
        public object StatsJson { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("stats")]
        public List<SummaryAggStat> Stats { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.PlayerStatSummary")]
    public class PlayerStatSummary {
        [SerializedName("maxRating")]
        public int MaxRating { get; set; }
        [SerializedName("playerStatSummaryTypeString")]
        public string PlayerStatSummaryTypeString { get; set; }
        [SerializedName("aggregatedStats")]
        public SummaryAggStats AggregatedStats { get; set; }
        [SerializedName("modifyDate")]
        public DateTime ModifyDate { get; set; }
        [SerializedName("leaves")]
        public int Leaves { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("playerStatSummaryType")]
        public string PlayerStatSummaryType { get; set; }
        [SerializedName("userId")]
        public long UserId { get; set; }
        [SerializedName("losses")]
        public int Losses { get; set; }
        [SerializedName("rating")]
        public int Rating { get; set; }
        [SerializedName("aggregatedStatsJson")]
        public object AggregatedStatsJson { get; set; }
        [SerializedName("wins")]
        public int Wins { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.PlayerStatSummaries")]
    public class PlayerStatSummaries {
        [SerializedName("season")]
        public int Season { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("playerStatSummarySet")]
        public List<PlayerStatSummary> PlayerStatSummarySet { get; set; }
        [SerializedName("userId")]
        public int UserId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.Talent")]
    public class Talent {
        [SerializedName("index")]
        public int Index { get; set; }
        [SerializedName("level5Desc")]
        public string Level5Desc { get; set; }
        [SerializedName("minLevel")]
        public int MinLevel { get; set; }
        [SerializedName("maxRank")]
        public int MaxRank { get; set; }
        [SerializedName("level4Desc")]
        public string Level4Desc { get; set; }
        [SerializedName("tltId")]
        public int TltId { get; set; }
        [SerializedName("level3Desc")]
        public string Level3Desc { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("talentGroupId")]
        public int TalentGroupId { get; set; }
        [SerializedName("gameCode")]
        public int GameCode { get; set; }
        [SerializedName("minTier")]
        public int MinTier { get; set; }
        [SerializedName("prereqTalentGameCode")]
        public object PrereqTalentGameCode { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("level2Desc")]
        public string Level2Desc { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("talentRowId")]
        public int TalentRowId { get; set; }
        [SerializedName("level1Desc")]
        public string Level1Desc { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.TalentRow")]
    public class TalentRow {
        [SerializedName("index")]
        public int Index { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("talents")]
        public List<Talent> Talents { get; set; }
        [SerializedName("tltGroupId")]
        public int TltGroupId { get; set; }
        [SerializedName("maxPointsInRow")]
        public int MaxPointsInRow { get; set; }
        [SerializedName("pointsToActivate")]
        public int PointsToActivate { get; set; }
        [SerializedName("tltRowId")]
        public int TltRowId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.TalentGroup")]
    public class TalentGroup {
        [SerializedName("index")]
        public int Index { get; set; }
        [SerializedName("talentRows")]
        public List<TalentRow> TalentRows { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("tltGroupId")]
        public int TltGroupId { get; set; }
        [SerializedName("version")]
        public int Version { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.runes.RuneType")]
    public class RuneType {
        [SerializedName("runeTypeId")]
        public int RuneTypeId { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.RuneSlot")]
    public class RuneSlot {
        [SerializedName("id")]
        public int Id { get; set; }
        [SerializedName("minLevel")]
        public int MinLevel { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("runeType")]
        public RuneType RuneType { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerCatalog")]
    public class SummonerCatalog {
        [SerializedName("items")]
        public object Items { get; set; }
        [SerializedName("talentTree")]
        public List<TalentGroup> TalentTree { get; set; }
        [SerializedName("spellBookConfig")]
        public List<RuneSlot> SpellBookConfig { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.Effect")]
    public class Effect {
        [SerializedName("effectId")]
        public int EffectId { get; set; }
        [SerializedName("gameCode")]
        public string GameCode { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("categoryId")]
        public object CategoryId { get; set; }
        [SerializedName("runeType")]
        public RuneType RuneType { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.ItemEffect")]
    public class ItemEffect {
        [SerializedName("effectId")]
        public int EffectId { get; set; }
        [SerializedName("itemEffectId")]
        public int ItemEffectId { get; set; }
        [SerializedName("effect")]
        public Effect Effect { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("value")]
        public string Value { get; set; }
        [SerializedName("itemId")]
        public int ItemId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.runes.Rune")]
    public class Rune {
        [SerializedName("imagePath")]
        public object ImagePath { get; set; }
        [SerializedName("toolTip")]
        public object ToolTip { get; set; }
        [SerializedName("tier")]
        public int Tier { get; set; }
        [SerializedName("itemId")]
        public int ItemId { get; set; }
        [SerializedName("runeType")]
        public RuneType RuneType { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("duration")]
        public int Duration { get; set; }
        [SerializedName("gameCode")]
        public int GameCode { get; set; }
        [SerializedName("itemEffects")]
        public List<ItemEffect> ItemEffects { get; set; }
        [SerializedName("baseType")]
        public string BaseType { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("description")]
        public string Description { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("uses")]
        public object Uses { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.spellbook.SlotEntry")]
    public class SlotEntry {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("runeId")]
        public int RuneId { get; set; }
        [SerializedName("runeSlotId")]
        public int RuneSlotId { get; set; }
        [SerializedName("runeSlot")]
        public RuneSlot RuneSlot { get; set; }
        [SerializedName("rune")]
        public Rune Rune { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.spellbook.SpellBookPageDTO")]
    public class SpellBookPageDTO {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("pageId")]
        public long PageId { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("current")]
        public bool Current { get; set; }
        [SerializedName("slotEntries")]
        public List<SlotEntry> SlotEntries { get; set; }
        [SerializedName("createDate")]
        public DateTime CreateDate { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.spellbook.SpellBookDTO")]
    public class SpellBookDTO {
        [SerializedName("bookPagesJson")]
        public object BookPagesJson { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("bookPages")]
        public List<SpellBookPageDTO> BookPages { get; set; }
        [SerializedName("dateString")]
        public string DateString { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerDefaultSpells")]
    public class SummonerDefaultSpells {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("summonerDefaultSpellsJson")]
        public object SummonerDefaultSpellsJson { get; set; }
        [SerializedName("summonerDefaultSpellMap")]
        public AsObject SummonerDefaultSpellMap { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerTalentsAndPoints")]
    public class SummonerTalentsAndPoints {
        [SerializedName("talentPoints")]
        public int TalentPoints { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("modifyDate")]
        public DateTime ModifyDate { get; set; }
        [SerializedName("createDate")]
        public DateTime CreateDate { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.Summoner")]
    public class Summoner {
        [SerializedName("internalName")]
        public string InternalName { get; set; }
        [SerializedName("previousSeasonHighestTier")]
        public string PreviousSeasonHighestTier { get; set; }
        [SerializedName("acctId")]
        public long AccountId { get; set; }
        [SerializedName("helpFlag")]
        public bool HelpFlag { get; set; }
        [SerializedName("sumId")]
        public long SummonerId { get; set; }
        [SerializedName("profileIconId")]
        public int ProfileIconId { get; set; }
        [SerializedName("displayEloQuestionaire")]
        public bool DisplayEloQuestionaire { get; set; }
        [SerializedName("lastGameDate")]
        public DateTime LastGameDate { get; set; }
        [SerializedName("previousSeasonHighestTeamReward")]
        public int PreviousSeasonHighestTeamReward { get; set; }
        [SerializedName("revisionDate")]
        public DateTime RevisionDate { get; set; }
        [SerializedName("advancedTutorialFlag")]
        public bool AdvancedTutorialFlag { get; set; }
        [SerializedName("revisionId")]
        public long RevisionId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("nameChangeFlag")]
        public bool NameChangeFlag { get; set; }
        [SerializedName("tutorialFlag")]
        public bool TutorialFlag { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.masterybook.TalentEntry")]
    public class TalentEntry {
        [SerializedName("rank")]
        public int Rank { get; set; }
        [SerializedName("talentId")]
        public int TalentId { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("talent")]
        public Talent Talent { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.masterybook.MasteryBookPageDTO")]
    public class MasteryBookPageDTO {
        [SerializedName("talentEntries")]
        public List<TalentEntry> TalentEntries { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("pageId")]
        public long PageId { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("current")]
        public bool Current { get; set; }
        [SerializedName("createDate")]
        public object CreateDate { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.masterybook.MasteryBookDTO")]
    public class MasteryBookDTO {
        [SerializedName("bookPagesJson")]
        public object BookPagesJson { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("bookPages")]
        public List<MasteryBookPageDTO> BookPages { get; set; }
        [SerializedName("dateString")]
        public string DateString { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerLevelAndPoints")]
    public class SummonerLevelAndPoints {
        [SerializedName("infPoints")]
        public int InfPoints { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("expPoints")]
        public int ExpPoints { get; set; }
        [SerializedName("summonerLevel")]
        public int SummonerLevel { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerLevel")]
    public class SummonerLevel {
        [SerializedName("expTierMod")]
        public int ExpTierMod { get; set; }
        [SerializedName("grantRp")]
        public int GrantRp { get; set; }
        [SerializedName("expForLoss")]
        public int ExpForLoss { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("summonerTier")]
        public int SummonerTier { get; set; }
        [SerializedName("infTierMod")]
        public int InfTierMod { get; set; }
        [SerializedName("expToNextLevel")]
        public int ExpToNextLevel { get; set; }
        [SerializedName("expForWin")]
        public int ExpForWin { get; set; }
        [SerializedName("summonerLevel")]
        public int Level { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.AllSummonerData")]
    public class AllSummonerData {
        [SerializedName("spellBook")]
        public SpellBookDTO SpellBook { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("summonerDefaultSpells")]
        public SummonerDefaultSpells SummonerDefaultSpells { get; set; }
        [SerializedName("summonerTalentsAndPoints")]
        public SummonerTalentsAndPoints SummonerTalentsAndPoints { get; set; }
        [SerializedName("summoner")]
        public Summoner Summoner { get; set; }
        [SerializedName("masteryBook")]
        public MasteryBookDTO MasteryBook { get; set; }
        [SerializedName("summonerLevelAndPoints")]
        public SummonerLevelAndPoints SummonerLevelAndPoints { get; set; }
        [SerializedName("summonerLevel")]
        public SummonerLevel SummonerLevel { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.broadcast.BroadcastNotification")]
    public partial class BroadcastNotification {
    }

    [Serializable, SerializedName("com.riotgames.platform.systemstate.ClientSystemStatesNotification")]
    public partial class ClientSystemStatesNotification {
        [SerializedName("championTradeThroughLCDS")]
        public bool ChampionTradeThroughLCDS { get; set; }
        [SerializedName("practiceGameEnabled")]
        public bool PracticeGameEnabled { get; set; }
        [SerializedName("advancedTutorialEnabled")]
        public bool AdvancedTutorialEnabled { get; set; }
        [SerializedName("minNumPlayersForPracticeGame")]
        public int MinNumPlayersForPracticeGame { get; set; }
        [SerializedName("practiceGameTypeConfigIdList")]
        public List<int> PracticeGameTypeConfigIdList { get; set; }
        [SerializedName("freeToPlayChampionIdList")]
        public List<int> FreeToPlayChampionIdList { get; set; }
        [SerializedName("freeToPlayChampionForNewPlayersIdList")]
        public List<int> FreeToPlayChampionForNewPlayersIdList { get; set; }
        [SerializedName("freeToPlayChampionsForNewPlayersMaxLevel")]
        public int FreeToPlayChampionsForNewPlayersMaxLevel { get; set; }
        [SerializedName("inactiveChampionIdList")]
        public List<object> InactiveChampionIdList { get; set; }
        [SerializedName("gameModeToInactiveSpellIds")]
        public Dictionary<string, object> GameModeToInactiveSpellIds { get; set; }
        [SerializedName("inactiveSpellIdList")]
        public List<int> InactiveSpellIdList { get; set; }
        [SerializedName("inactiveTutorialSpellIdList")]
        public List<int> InactiveTutorialSpellIdList { get; set; }
        [SerializedName("inactiveClassicSpellIdList")]
        public List<int> InactiveClassicSpellIdList { get; set; }
        [SerializedName("inactiveOdinSpellIdList")]
        public List<int> InactiveOdinSpellIdList { get; set; }
        [SerializedName("inactiveAramSpellIdList")]
        public List<int> InactiveAramSpellIdList { get; set; }
        [SerializedName("enabledQueueIdsList")]
        public List<int> EnabledQueueIdsList { get; set; }
        [SerializedName("unobtainableChampionSkinIDList")]
        public List<int> UnobtainableChampionSkinIdList { get; set; }
        [SerializedName("archivedStatsEnabled")]
        public bool ArchivedStatsEnabled { get; set; }
        [SerializedName("queueThrottleDTO")]
        public Dictionary<string, object> QueueThrottleDTO { get; set; }
        [SerializedName("gameMapEnabledDTOList")]
        public List<Dictionary<string, object>> GameMapEnabledDTOList { get; set; }
        [SerializedName("storeCustomerEnabled")]
        public bool StoreCustomerEnabled { get; set; }
        [SerializedName("runeUniquePerSpellBook")]
        public bool RuneUniquePerSpellBook { get; set; }
        [SerializedName("tribunalEnabled")]
        public bool TribunalEnabled { get; set; }
        [SerializedName("observerModeEnabled")]
        public bool ObserverModeEnabled { get; set; }
        [SerializedName("spectatorSlotLimit")]
        public int SpectatorSlotLimit { get; set; }
        [SerializedName("clientHeartBeatRateSeconds")]
        public int ClientHeartBeatRateSeconds { get; set; }
        [SerializedName("observableGameModes")]
        public List<string> ObservableGameModes { get; set; }
        [SerializedName("observableCustomGameModes")]
        public string ObservableCustomGameModes { get; set; }
        [SerializedName("teamServiceEnabled")]
        public bool TeamServiceEnabled { get; set; }
        [SerializedName("leagueServiceEnabled")]
        public bool LeagueServiceEnabled { get; set; }
        [SerializedName("modularGameModeEnabled")]
        public bool ModularGameModeEnabled { get; set; }
        [SerializedName("riotDataServiceDataSendProbability")]
        public decimal RiotDataServiceDataSendProbability { get; set; }
        [SerializedName("displayPromoGamesPlayedEnabled")]
        public bool DisplayPromoGamesPlayedEnabled { get; set; }
        [SerializedName("masteryPageOnServer")]
        public bool MasteryPageOnServer { get; set; }
        [SerializedName("maxMasteryPagesOnServer")]
        public int MaxMasteryPagesOnServer { get; set; }
        [SerializedName("tournamentSendStatsEnabled")]
        public bool TournamentSendStatsEnabled { get; set; }
        [SerializedName("tournamentShortCodesEnabled")]
        public bool TournamentShortCodesEnabled { get; set; }
        [SerializedName("replayServiceAddress")]
        public string ReplayServiceAddress { get; set; }
        [SerializedName("kudosEnabled")]
        public bool KudosEnabled { get; set; }
        [SerializedName("buddyNotesEnabled")]
        public bool BuddyNotesEnabled { get; set; }
        [SerializedName("localeSpecificChatRoomsEnabled")]
        public bool LocaleSpecificChatRoomsEnabled { get; set; }
        [SerializedName("replaySystemStates")]
        public Dictionary<string, object> ReplaySystemStates { get; set; }
        [SerializedName("sendFeedbackEventsEnabled")]
        public bool SendFeedbackEventsEnabled { get; set; }
        [SerializedName("knownGeographicGameServerRegions")]
        public List<string> KnownGeographicGameServerRegions { get; set; }
        [SerializedName("leaguesDecayMessagingEnabled")]
        public bool LeaguesDecayMessagingEnabled { get; set; }
        [SerializedName("currentSeason")]
        public int CurrentSeason { get; set; }
        [SerializedName("causedEarlySurrender")]
        public bool CausedEarlySurrender { get; set; }
        [SerializedName("gameEndedInEarlySurrender")]
        public bool GameEndedInEarlySurrender { get; set; }
        [SerializedName("teamEarlySurrendered")]
        public bool TeamEarlySurrendered { get; set; }
        [SerializedName("earlySurrenderAccomplice")]
        public bool EarlySurrenderAccomplice { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.kudos.dto.PendingKudosDTO")]
    public class PendingKudosDTO {
        [SerializedName("pendingCounts")]
        public List<int> PendingCounts { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.GameTypeConfigDTO")]
    public class GameTypeConfigDTO {
        [SerializedName("allowTrades")]
        public bool AllowTrades { get; set; }
        [SerializedName("banTimerDuration")]
        public int BanTimerDuration { get; set; }
        [SerializedName("maxAllowableBans")]
        public int MaxAllowableBans { get; set; }
        [SerializedName("crossTeamChampionPool")]
        public bool CrossTeamChampionPool { get; set; }
        [SerializedName("teamChampionPool")]
        public bool TeamChampionPool { get; set; }
        [SerializedName("postPickTimerDuration")]
        public int PostPickTimerDuration { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("id")]
        public int Id { get; set; }
        [SerializedName("duplicatePick")]
        public bool DuplicatePick { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("exclusivePick")]
        public bool ExclusivePick { get; set; }
        [SerializedName("mainPickTimerDuration")]
        public int MainPickTimerDuration { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("pickMode")]
        public string PickMode { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.clientfacade.domain.LoginDataPacket")]
    public class LoginDataPacket {
        [SerializedName("restrictedGamesRemainingForRanked")]
        public int RestrictedGamesRemainingForRanked { get; set; }
        [SerializedName("playerStatSummaries")]
        public PlayerStatSummaries PlayerStatSummaries { get; set; }
        [SerializedName("restrictedChatGamesRemaining")]
        public int RestrictedChatGamesRemaining { get; set; }
        [SerializedName("minutesUntilShutdown")]
        public int MinutesUntilShutdown { get; set; }
        [SerializedName("minor")]
        public bool Minor { get; set; }
        [SerializedName("maxPracticeGameSize")]
        public int MaxPracticeGameSize { get; set; }
        [SerializedName("summonerCatalog")]
        public SummonerCatalog SummonerCatalog { get; set; }
        [SerializedName("ipBalance")]
        public int IpBalance { get; set; }
        [SerializedName("reconnectInfo")]
        public PlatformGameLifecycleDTO ReconnectInfo { get; set; }
        [SerializedName("languages")]
        public List<string> Languages { get; set; }
        [SerializedName("simpleMessages")]
        public List<object> SimpleMessages { get; set; }
        [SerializedName("allSummonerData")]
        public AllSummonerData AllSummonerData { get; set; }
        [SerializedName("customMinutesLeftToday")]
        public int CustomMinutesLeftToday { get; set; }
        [SerializedName("displayPrimeReformCard")]
        public bool DisplayPrimeReformCard { get; set; }
        [SerializedName("platformGameLifecycleDTO")]
        public PlatformGameLifecycleDTO PlatformGameLifecycleDTO { get; set; }
        [SerializedName("coOpVsAiMinutesLeftToday")]
        public int CoOpVsAiMinutesLeftToday { get; set; }
        [SerializedName("bingeData")]
        public object BingeData { get; set; }
        [SerializedName("inGhostGame")]
        public bool InGhostGame { get; set; }
        [SerializedName("bingePreventionSystemEnabledForClient")]
        public bool BingePreventionSystemEnabledForClient { get; set; }
        [SerializedName("pendingBadges")]
        public int PendingBadges { get; set; }
        [SerializedName("bannedUntilDateMillis")]
        public long BannedUntilDateMillis { get; set; }
        [SerializedName("broadcastNotification")]
        public BroadcastNotification BroadcastNotification { get; set; }
        [SerializedName("minutesUntilMidnight")]
        public int MinutesUntilMidnight { get; set; }
        [SerializedName("timeUntilFirstWinOfDay")]
        public long TimeUntilFirstWinOfDay { get; set; }
        [SerializedName("coOpVsAiMsecsUntilReset")]
        public long CoOpVsAiMsecsUntilReset { get; set; }
        [SerializedName("clientSystemStates")]
        public ClientSystemStatesNotification ClientSystemStates { get; set; }
        [SerializedName("bingeMinutesRemaining")]
        public long BingeMinutesRemaining { get; set; }
        [SerializedName("pendingKudosDTO")]
        public PendingKudosDTO PendingKudosDTO { get; set; }
        [SerializedName("leaverBusterPenaltyTime")]
        public int LeaverBusterPenaltyTime { get; set; }
        [SerializedName("platformId")]
        public string PlatformId { get; set; }
        [SerializedName("emailStatus")]
        public string EmailStatus { get; set; }
        [SerializedName("matchMakingEnabled")]
        public bool MatchMakingEnabled { get; set; }
        [SerializedName("minutesUntilShutdownEnabled")]
        public bool MinutesUntilShutdownEnabled { get; set; }
        [SerializedName("rpBalance")]
        public int RpBalance { get; set; }
        [SerializedName("showEmailVerificationPopup")]
        public bool ShowEmailVerificationPopup { get; set; }
        [SerializedName("bingeIsPlayerInBingePreventionWindow")]
        public bool BingeIsPlayerInBingePreventionWindow { get; set; }
        [SerializedName("gameTypeConfigs")]
        public List<GameTypeConfigDTO> GameTypeConfigs { get; set; }
        [SerializedName("minorShutdownEnforced")]
        public bool MinorShutdownEnforced { get; set; }
        [SerializedName("competitiveRegion")]
        public string CompetitiveRegion { get; set; }
        [SerializedName("customMsecsUntilReset")]
        public long CustomMsecsUntilReset { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.matchmaking.MatchingThrottleConfig")]
    public class MatchingThrottleConfig {
        [SerializedName("limit")]
        public int Limit { get; set; }
        [SerializedName("matchingThrottleProperties")]
        public List<object> MatchingThrottleProperties { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("cacheName")]
        public string CacheName { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.matchmaking.GameQueueConfig")]
    public class GameQueueConfig {
        [SerializedName("blockedMinutesThreshold")]
        public int BlockedMinutesThreshold { get; set; }
        [SerializedName("ranked")]
        public bool Ranked { get; set; }
        [SerializedName("minimumParticipantListSize")]
        public int MinimumParticipantListSize { get; set; }
        [SerializedName("maxLevel")]
        public int MaxLevel { get; set; }
        [SerializedName("thresholdEnabled")]
        public bool ThresholdEnabled { get; set; }
        [SerializedName("gameTypeConfigId")]
        public int GameTypeConfigId { get; set; }
        [SerializedName("minLevel")]
        public int MinLevel { get; set; }
        [SerializedName("queueState")]
        public string QueueState { get; set; }
        [SerializedName("type")]
        public string Type { get; set; }
        [SerializedName("cacheName")]
        public string CacheName { get; set; }
        [SerializedName("id")]
        public int Id { get; set; }
        [SerializedName("queueBonusKey")]
        public string QueueBonusKey { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("maxSummonerLevelForFirstWinOfTheDay")]
        public int MaxSummonerLevelForFirstWinOfTheDay { get; set; }
        [SerializedName("queueStateString")]
        public string QueueStateString { get; set; }
        [SerializedName("pointsConfigKey")]
        public string PointsConfigKey { get; set; }
        [SerializedName("teamOnly")]
        public bool TeamOnly { get; set; }
        [SerializedName("minimumQueueDodgeDelayTime")]
        public int MinimumQueueDodgeDelayTime { get; set; }
        [SerializedName("randomizeTeamSides")]
        public bool RandomizeTeamSides { get; set; }
        [SerializedName("supportedMapIds")]
        public List<int> SupportedMapIds { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("typeString")]
        public string TypeString { get; set; }
        [SerializedName("numPlayersPerTeam")]
        public int NumPlayersPerTeam { get; set; }
        [SerializedName("disallowFreeChampions")]
        public bool DisallowFreeChampions { get; set; }
        [SerializedName("maximumParticipantListSize")]
        public int MaximumParticipantListSize { get; set; }
        [SerializedName("mapSelectionAlgorithm")]
        public string MapSelectionAlgorithm { get; set; }
        [SerializedName("botsCanSpawnOnBlueSide")]
        public bool BotsCanSpawnOnBlueSide { get; set; }
        [SerializedName("gameMutators")]
        public List<string> GameMutators { get; set; }
        [SerializedName("thresholdSize")]
        public int ThresholdSize { get; set; }
        [SerializedName("matchingThrottleConfig")]
        public MatchingThrottleConfig MatchingThrottleConfig { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.boost.SummonerActiveBoostsDTO")]
    public class SummonerActiveBoostsDTO {
        [SerializedName("xpBoostEndDate")]
        public long XpBoostEndDate { get; set; }
        [SerializedName("xpBoostPerWinCount")]
        public int XpBoostPerWinCount { get; set; }
        [SerializedName("xpLoyaltyBoost")]
        public int XpLoyaltyBoost { get; set; }
        [SerializedName("ipBoostPerWinCount")]
        public int IpBoostPerWinCount { get; set; }
        [SerializedName("ipLoyaltyBoost")]
        public int IpLoyaltyBoost { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("ipBoostEndDate")]
        public long IpBoostEndDate { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.champion.ChampionSkinDTO")]
    public class ChampionSkinDTO {
        [SerializedName("lastSelected")]
        public bool LastSelected { get; set; }
        [SerializedName("stillObtainable")]
        public bool StillObtainable { get; set; }
        [SerializedName("purchaseDate")]
        public long PurchaseDate { get; set; }
        [SerializedName("winCountRemaining")]
        public int WinCountRemaining { get; set; }
        [SerializedName("endDate")]
        public long EndDate { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("freeToPlayReward")]
        public bool FreeToPlayReward { get; set; }
        [SerializedName("sources")]
        public List<object> Sources { get; set; }
        [SerializedName("skinId")]
        public int SkinId { get; set; }
        [SerializedName("owned")]
        public bool Owned { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.champion.ChampionDTO")]
    public class ChampionDTO {
        [SerializedName("rankedPlayEnabled")]
        public bool RankedPlayEnabled { get; set; }
        [SerializedName("winCountRemaining")]
        public int WinCountRemaining { get; set; }
        [SerializedName("botEnabled")]
        public bool BotEnabled { get; set; }
        [SerializedName("endDate")]
        public long EndDate { get; set; }
        [SerializedName("freeToPlayReward")]
        public bool FreeToPlayReward { get; set; }
        [SerializedName("sources")]
        public List<object> Sources { get; set; }
        [SerializedName("owned")]
        public bool Owned { get; set; }
        [SerializedName("purchased")]
        public long Purchased { get; set; }
        [SerializedName("championSkins")]
        public List<ChampionSkinDTO> ChampionSkins { get; set; }
        [SerializedName("purchaseDate")]
        public long PurchaseDate { get; set; }
        [SerializedName("active")]
        public bool Active { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("freeToPlay")]
        public bool FreeToPlay { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.runes.SummonerRune")]
    public class SummonerRune {
        [SerializedName("purchased")]
        public DateTime Purchased { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }
        [SerializedName("runeId")]
        public int RuneId { get; set; }
        [SerializedName("quantity")]
        public int Quantity { get; set; }
        [SerializedName("rune")]
        public Rune Rune { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.runes.SummonerRuneInventory")]
    public class SummonerRuneInventory {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("summonerRunesJson")]
        public object SummonerRunesJson { get; set; }
        [SerializedName("dateString")]
        public string DateString { get; set; }
        [SerializedName("summonerRunes")]
        public List<SummonerRune> SummonerRunes { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.leagues.pojo.LeagueItemDTO")]
    public class LeagueItemDTO {
        [SerializedName("previousDayLeaguePosition")]
        public int PreviousDayLeaguePosition { get; set; }
        [SerializedName("timeLastDecayMessageShown")]
        public long TimeLastDecayMessageShown { get; set; }
        [SerializedName("seasonEndTier")]
        public string SeasonEndTier { get; set; }
        [SerializedName("seasonEndRank")]
        public string SeasonEndRank { get; set; }
        [SerializedName("hotStreak")]
        public bool HotStreak { get; set; }
        [SerializedName("leagueName")]
        public string LeagueName { get; set; }
        [SerializedName("miniSeries")]
        public object MiniSeries { get; set; }
        [SerializedName("tier")]
        public string Tier { get; set; }
        [SerializedName("freshBlood")]
        public bool FreshBlood { get; set; }
        [SerializedName("lastPlayed")]
        public long LastPlayed { get; set; }
        [SerializedName("timeUntilInactivityStatusChanges")]
        public long TimeUntilInactivityStatusChanges { get; set; }
        [SerializedName("inactivityStatus")]
        public string InactivityStatus { get; set; }
        [SerializedName("playerOrTeamId")]
        public string PlayerOrTeamId { get; set; }
        [SerializedName("leaguePoints")]
        public int LeaguePoints { get; set; }
        [SerializedName("demotionWarning")]
        public int DemotionWarning { get; set; }
        [SerializedName("inactive")]
        public bool Inactive { get; set; }
        [SerializedName("seasonEndApexPosition")]
        public int SeasonEndApexPosition { get; set; }
        [SerializedName("rank")]
        public string Rank { get; set; }
        [SerializedName("veteran")]
        public bool Veteran { get; set; }
        [SerializedName("queueType")]
        public string QueueType { get; set; }
        [SerializedName("losses")]
        public int Losses { get; set; }
        [SerializedName("timeUntilDecay")]
        public long TimeUntilDecay { get; set; }
        [SerializedName("displayDecayWarning")]
        public bool DisplayDecayWarning { get; set; }
        [SerializedName("playerOrTeamName")]
        public string PlayerOrTeamName { get; set; }
        [SerializedName("wins")]
        public int Wins { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.leagues.client.dto.SummonerLeagueItemsDTO")]
    public class SummonerLeagueItemsDTO {
        [SerializedName("summonerLeagues")]
        public List<LeagueItemDTO> SummonerLeagues { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.TeamId")]
    public class TeamId {
        [SerializedName("fullId")]
        public string FullId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.stats.TeamStatDetail")]
    public class TeamStatDetail {
        [SerializedName("inSeasonWins")]
        public int InSeasonWins { get; set; }
        [SerializedName("maxRating")]
        public int MaxRating { get; set; }
        [SerializedName("seedRating")]
        public int SeedRating { get; set; }
        [SerializedName("averageGamesPlayed")]
        public int AverageGamesPlayed { get; set; }
        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("inSeasonLosses")]
        public int InSeasonLosses { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("teamIdString")]
        public string TeamIdString { get; set; }
        [SerializedName("seasonId")]
        public int SeasonId { get; set; }
        [SerializedName("losses")]
        public int Losses { get; set; }
        [SerializedName("rating")]
        public int Rating { get; set; }
        [SerializedName("teamStatTypeString")]
        public string TeamStatTypeString { get; set; }
        [SerializedName("wins")]
        public int Wins { get; set; }
        [SerializedName("teamStatType")]
        public string TeamStatType { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.stats.TeamStatSummary")]
    public class TeamStatSummary {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("teamStatDetails")]
        public List<TeamStatDetail> TeamStatDetails { get; set; }
        [SerializedName("teamIdString")]
        public string TeamIdString { get; set; }
        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.stats.MatchHistorySummary")]
    public class MatchHistorySummary {
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("mapId")]
        public int MapId { get; set; }
        [SerializedName("assists")]
        public int Assists { get; set; }
        [SerializedName("opposingTeamName")]
        public string OpposingTeamName { get; set; }
        [SerializedName("invalid")]
        public bool Invalid { get; set; }
        [SerializedName("deaths")]
        public int Deaths { get; set; }
        [SerializedName("gameId")]
        public long GameId { get; set; }
        [SerializedName("kills")]
        public int Kills { get; set; }
        [SerializedName("win")]
        public bool Win { get; set; }
        [SerializedName("date")]
        public long Date { get; set; }
        [SerializedName("opposingTeamKills")]
        public int OpposingTeamKills { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.dto.TeamMemberInfoDTO")]
    public class TeamMemberInfoDTO {
        [SerializedName("joinDate")]
        public DateTime JoinDate { get; set; }
        [SerializedName("playerName")]
        public object PlayerName { get; set; }
        [SerializedName("inviteDate")]
        public DateTime InviteDate { get; set; }
        [SerializedName("status")]
        public string Status { get; set; }
        [SerializedName("playerId")]
        public long PlayerId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.dto.RosterDTO")]
    public class RosterDTO {
        [SerializedName("ownerId")]
        public long OwnerId { get; set; }
        [SerializedName("memberList")]
        public List<TeamMemberInfoDTO> MemberList { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.dto.TeamDTO")]
    public class TeamDTO {
        [SerializedName("secondsUntilEligibleForDeletion")]
        public long SecondsUntilEligibleForDeletion { get; set; }
        [SerializedName("secondLastJoinDate")]
        public DateTime SecondLastJoinDate { get; set; }
        [SerializedName("lastJoinDate")]
        public DateTime LastJoinDate { get; set; }
        [SerializedName("teamStatSummary")]
        public TeamStatSummary TeamStatSummary { get; set; }
        [SerializedName("matchHistory")]
        public List<MatchHistorySummary> MatchHistory { get; set; }
        [SerializedName("status")]
        public string Status { get; set; }
        [SerializedName("tag")]
        public string Tag { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("thirdLastJoinDate")]
        public DateTime ThirdLastJoinDate { get; set; }
        [SerializedName("roster")]
        public RosterDTO Roster { get; set; }
        [SerializedName("lastGameDate")]
        public DateTime LastGameDate { get; set; }
        [SerializedName("modifyDate")]
        public DateTime ModifyDate { get; set; }
        [SerializedName("messageOfDay")]
        public object MessageOfDay { get; set; }
        [SerializedName("createDate")]
        public DateTime CreateDate { get; set; }
        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.TeamInfo")]
    public class TeamInfo {
        [SerializedName("secondsUntilEligibleForDeletion")]
        public long SecondsUntilEligibleForDeletion { get; set; }
        [SerializedName("memberStatusString")]
        public string MemberStatusString { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("tag")]
        public string Tag { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("memberStatus")]
        public string MemberStatus { get; set; }
        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.team.dto.PlayerDTO")]
    public class PlayerDTO {
        [SerializedName("playerId")]
        public long PlayerId { get; set; }
        [SerializedName("teamsSummary")]
        public List<TeamDTO> TeamsSummary { get; set; }
        [SerializedName("createdTeams")]
        public List<object> CreatedTeams { get; set; }
        [SerializedName("playerTeams")]
        public List<TeamInfo> PlayerTeams { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerSummary")]
    public class SummonerSummary {
        [SerializedName("id")]
        public long Id { get; set; }
        [SerializedName("internalName")]
        public string InternalName { get; set; }
        [SerializedName("level")]
        public int Level { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("losses")]
        public int Losses { get; set; }
        [SerializedName("leaves")]
        public int Leaves { get; set; }
        [SerializedName("wins")]
        public int Wins { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.PublicSummoner")]
    public class PublicSummoner {
        [SerializedName("internalName")]
        public string InternalName { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("acctId")]
        public long AcctId { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("profileIconId")]
        public int ProfileIconId { get; set; }
        [SerializedName("revisionDate")]
        public DateTime RevisionDate { get; set; }
        [SerializedName("revisionId")]
        public long RevisionId { get; set; }
        [SerializedName("summonerLevel")]
        public int SummonerLevel { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.Player")]
    public class Player {
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.Member")]
    public class Member {
        [SerializedName("hasDelegatedInvitePower")]
        public bool HasDelegatedInvitePower { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.Invitee")]
    public class Invitee {
        [SerializedName("inviteeStateAsString")]
        public string InviteeStateAsString { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("inviteeState")]
        public string InviteeState { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.LobbyStatus")]
    public class LobbyStatus {
        [SerializedName("chatKey")]
        public string ChatKey { get; set; }
        [SerializedName("gameMetaData")]
        public string GameMetaData { get; set; }
        [SerializedName("owner")]
        public Player Owner { get; set; }
        [SerializedName("members")]
        public List<Member> Members { get; set; }
        [SerializedName("invitees")]
        public List<Invitee> Invitees { get; set; }
        [SerializedName("invitationId")]
        public string InvitationId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.leagues.pojo.LeagueListDTO")]
    public class LeagueListDTO {
        [SerializedName("queue")]
        public string Queue { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("tier")]
        public string Tier { get; set; }
        [SerializedName("requestorsRank")]
        public string RequestorsRank { get; set; }
        [SerializedName("entries")]
        public List<LeagueItemDTO> Entries { get; set; }
        [SerializedName("nextApexUpdate")]
        public object NextApexUpdate { get; set; }
        [SerializedName("maxLeagueSize")]
        public int MaxLeagueSize { get; set; }
        [SerializedName("requestorsName")]
        public string RequestorsName { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.leagues.client.dto.SummonerLeaguesDTO")]
    public class SummonerLeaguesDTO {
        [SerializedName("summonerLeagues")]
        public List<LeagueListDTO> SummonerLeagues { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.map.GameMap")]
    public class GameMap {
        [SerializedName("displayName")]
        public string DisplayName { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("mapId")]
        public int MapId { get; set; }
        [SerializedName("totalPlayers")]
        public int TotalPlayers { get; set; }
        [SerializedName("description")]
        public string Description { get; set; }
        [SerializedName("minCustomPlayers")]
        public int MinCustomPlayers { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("dataVersion")]
        public object DataVersion { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.PracticeGameConfig")]
    public class PracticeGameConfig {
        [SerializedName("passbackDataPacket")]
        public object PassbackDataPacket { get; set; }
        [SerializedName("gameName")]
        public string GameName { get; set; }
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("allowSpectators")]
        public string AllowSpectators { get; set; }
        [SerializedName("region")]
        public string Region { get; set; }
        [SerializedName("gameTypeConfig")]
        public int GameTypeConfig { get; set; }
        [SerializedName("gamePassword")]
        public string GamePassword { get; set; }
        [SerializedName("maxNumPlayers")]
        public int MaxNumPlayers { get; set; }
        [SerializedName("gameMap")]
        public GameMap GameMap { get; set; }
        [SerializedName("gameMutators")]
        public List<object> GameMutators { get; set; }
        [SerializedName("passbackUrl")]
        public object PassbackUrl { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.FeaturedGameInfo")]
    public class FeaturedGameInfo {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("championVoteInfoList")]
        public List<object> ChampionVoteInfoList { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.PlayerChampionSelectionDTO")]
    public class PlayerChampionSelectionDTO {
        [SerializedName("summonerInternalName")]
        public string SummonerInternalName { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("spell2Id")]
        public int Spell2Id { get; set; }
        [SerializedName("selectedSkinIndex")]
        public int SelectedSkinIndex { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("spell1Id")]
        public int Spell1Id { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.GameDTO")]
    public class GameDTO {
        [SerializedName("spectatorsAllowed")]
        public string SpectatorsAllowed { get; set; }
        [SerializedName("passwordSet")]
        public bool PasswordSet { get; set; }
        [SerializedName("practiceGameRewardsDisabledReasons")]
        public List<string> PracticeGameRewardsDisabledReasons { get; set; }
        [SerializedName("gameType")]
        public string GameType { get; set; }
        [SerializedName("gameTypeConfigId")]
        public int GameTypeConfigId { get; set; }
        [SerializedName("gameState")]
        public GameState GameState { get; set; }
        [SerializedName("observers")]
        public List<object> Observers { get; set; }
        [SerializedName("statusOfParticipants")]
        public string StatusOfParticipants { get; set; }
        [SerializedName("id")]
        public long Id { get; set; }
        [SerializedName("ownerSummary")]
        public PlayerParticipant OwnerSummary { get; set; }
        [SerializedName("teamTwoPickOutcome")]
        public object TeamTwoPickOutcome { get; set; }
        [SerializedName("teamTwo")]
        public List<GameParticipant> TeamTwo { get; set; }
        [SerializedName("bannedChampions")]
        public List<BannedChampion> BannedChampions { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("roomName")]
        public string RoomName { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("spectatorDelay")]
        public int SpectatorDelay { get; set; }
        [SerializedName("teamOne")]
        public List<GameParticipant> TeamOne { get; set; }
        [SerializedName("terminatedCondition")]
        public string TerminatedCondition { get; set; }
        [SerializedName("queueTypeName")]
        public string QueueTypeName { get; set; }
        [SerializedName("featuredGameInfo")]
        public FeaturedGameInfo FeaturedGameInfo { get; set; }
        [SerializedName("passbackUrl")]
        public object PassbackUrl { get; set; }
        [SerializedName("roomPassword")]
        public string RoomPassword { get; set; }
        [SerializedName("optimisticLock")]
        public int OptimisticLock { get; set; }
        [SerializedName("teamOnePickOutcome")]
        public object TeamOnePickOutcome { get; set; }
        [SerializedName("maxNumPlayers")]
        public int MaxNumPlayers { get; set; }
        [SerializedName("queuePosition")]
        public int QueuePosition { get; set; }
        [SerializedName("terminatedConditionString")]
        public string TerminatedConditionString { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("expiryTime")]
        public long ExpiryTime { get; set; }
        [SerializedName("mapId")]
        public int MapId { get; set; }
        [SerializedName("banOrder")]
        public List<int> BanOrder { get; set; }
        [SerializedName("gameStateString")]
        public string GameStateString { get; set; }
        [SerializedName("pickTurn")]
        public int PickTurn { get; set; }
        [SerializedName("playerChampionSelections")]
        public List<PlayerChampionSelectionDTO> PlayerChampionSelections { get; set; }
        [SerializedName("gameMutators")]
        public List<string> GameMutators { get; set; }
        [SerializedName("joinTimerDuration")]
        public int JoinTimerDuration { get; set; }
        [SerializedName("passbackDataPacket")]
        public object PassbackDataPacket { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.StartChampSelectDTO")]
    public class StartChampSelectDTO {
        [SerializedName("invalidPlayers")]
        public List<object> InvalidPlayers { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.GameTimerDTO")]
    public class GameTimerDTO {
        [SerializedName("remainingTimeInMillis")]
        public long RemainingTimeInMillis { get; set; }
        [SerializedName("currentGameState")]
        public GameState CurrentGameState { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.harassment.LcdsResponseString")]
    public class LcdsResponseString {
        [SerializedName("value")]
        public string Value { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.reroll.pojo.PointSummary")]
    public class PointSummary {
        [SerializedName("pointsToNextRoll")]
        public int PointsToNextRoll { get; set; }
        [SerializedName("maxRolls")]
        public int MaxRolls { get; set; }
        [SerializedName("numberOfRolls")]
        public int NumberOfRolls { get; set; }
        [SerializedName("pointsCostToRoll")]
        public int PointsCostToRoll { get; set; }
        [SerializedName("currentPoints")]
        public int CurrentPoints { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.TimeTrackedStat")]
    public class TimeTrackedStat {
        [SerializedName("timestamp")]
        public DateTime Timestamp { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("type")]
        public string Type { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.PlayerStats")]
    public class PlayerStats {
        [SerializedName("timeTrackedStats")]
        public List<TimeTrackedStat> TimeTrackedStats { get; set; }
        [SerializedName("promoGamesPlayed")]
        public int PromoGamesPlayed { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("promoGamesPlayedLastUpdated")]
        public object PromoGamesPlayedLastUpdated { get; set; }
        [SerializedName("lifetimeGamesPlayed")]
        public AsObject LifetimeGamesPlayed { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.PlayerLifetimeStats")]
    public class PlayerLifetimeStats {
        [SerializedName("playerStatSummaries")]
        public PlayerStatSummaries PlayerStatSummaries { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("previousFirstWinOfDay")]
        public DateTime PreviousFirstWinOfDay { get; set; }
        [SerializedName("userId")]
        public long UserId { get; set; }
        [SerializedName("dodgeStreak")]
        public int DodgeStreak { get; set; }
        [SerializedName("dodgePenaltyDate")]
        public object DodgePenaltyDate { get; set; }
        [SerializedName("playerStatsJson")]
        public object PlayerStatsJson { get; set; }
        [SerializedName("playerStats")]
        public PlayerStats PlayerStats { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.AggregatedStat")]
    public class AggregatedStat {
        [SerializedName("statType")]
        public string StatType { get; set; }
        [SerializedName("count")]
        public int Count { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("value")]
        public int Value { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.ChampionStatInfo")]
    public class ChampionStatInfo {
        [SerializedName("totalGamesPlayed")]
        public int TotalGamesPlayed { get; set; }
        [SerializedName("accountId")]
        public long AccountId { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("stats")]
        public List<AggregatedStat> Stats { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.AggregatedStatsKey")]
    public class AggregatedStatsKey {
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("season")]
        public int Season { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("userId")]
        public long UserId { get; set; }
        [SerializedName("gameModeString")]
        public string GameModeString { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.AggregatedStats")]
    public class AggregatedStats {
        [SerializedName("lifetimeStatistics")]
        public List<AggregatedStat> LifetimeStatistics { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("modifyDate")]
        public DateTime ModifyDate { get; set; }
        [SerializedName("key")]
        public AggregatedStatsKey Key { get; set; }
        [SerializedName("aggregatedStatsJson")]
        public object AggregatedStatsJson { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.team.TeamPlayerAggregatedStatsDTO")]
    public class TeamPlayerAggregatedStatsDTO {
        [SerializedName("postSeasonGamesPlayed")]
        public int PostSeasonGamesPlayed { get; set; }
        [SerializedName("playerId")]
        public long PlayerId { get; set; }
        [SerializedName("postSeasonWins")]
        public int PostSeasonWins { get; set; }
        [SerializedName("seasonId")]
        public int SeasonId { get; set; }
        [SerializedName("aggregatedStats")]
        public AggregatedStats AggregatedStats { get; set; }
        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
        [SerializedName("teamStatType")]
        public string TeamStatType { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.team.TeamAggregatedStatsDTO")]
    public class TeamAggregatedStatsDTO {
        [SerializedName("queueType")]
        public string QueueType { get; set; }
        [SerializedName("serializedToJson")]
        public string SerializedToJson { get; set; }
        [SerializedName("playerAggregatedStatsList")]
        public List<TeamPlayerAggregatedStatsDTO> PlayerAggregatedStatsList { get; set; }
        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.RawStatDTO")]
    public class RawStatDTO {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("value")]
        public int Value { get; set; }
        [SerializedName("statTypeName")]
        public string StatTypeName { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.PlayerParticipantStatsSummary")]
    public class PlayerParticipantStatsSummary {
        [SerializedName("skinName")]
        public string SkinName { get; set; }
        [SerializedName("gameId")]
        public long GameId { get; set; }
        [SerializedName("profileIconId")]
        public int ProfileIconId { get; set; }
        [SerializedName("elo")]
        public int Elo { get; set; }
        [SerializedName("leaver")]
        public bool Leaver { get; set; }
        [SerializedName("leaves")]
        public int Leaves { get; set; }
        [SerializedName("teamId")]
        public long TeamId { get; set; }
        [SerializedName("statistics")]
        public List<RawStatDTO> Statistics { get; set; }
        [SerializedName("eloChange")]
        public int EloChange { get; set; }
        [SerializedName("level")]
        public int Level { get; set; }
        [SerializedName("botPlayer")]
        public bool BotPlayer { get; set; }
        [SerializedName("userId")]
        public long UserId { get; set; }
        [SerializedName("spell2Id")]
        public int Spell2Id { get; set; }
        [SerializedName("losses")]
        public int Losses { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("wins")]
        public int Wins { get; set; }
        [SerializedName("spell1Id")]
        public int Spell1Id { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.statistics.EndOfGameStats")]
    public class EndOfGameStats {
        [SerializedName("battleBoostIpEarned")]
        public int BattleBoostIpEarned { get; set; }
        [SerializedName("ranked")]
        public bool Ranked { get; set; }
        [SerializedName("talentPointsGained")]
        public int TalentPointsGained { get; set; }
        [SerializedName("skinIndex")]
        public int SkinIndex { get; set; }
        [SerializedName("basePoints")]
        public int BasePoints { get; set; }
        [SerializedName("teamPlayerParticipantStats")]
        public List<PlayerParticipantStatsSummary> TeamPlayerParticipantStats { get; set; }
        [SerializedName("difficulty")]
        public string Difficulty { get; set; }
        [SerializedName("partyRewardsBonusIpEarned")]
        public int PartyRewardsBonusIpEarned { get; set; }
        [SerializedName("boostXpEarned")]
        public int BoostXpEarned { get; set; }
        [SerializedName("invalid")]
        public bool Invalid { get; set; }
        [SerializedName("roomName")]
        public string RoomName { get; set; }
        [SerializedName("userId")]
        public object UserId { get; set; }
        [SerializedName("rpEarned")]
        public int RpEarned { get; set; }
        [SerializedName("experienceTotal")]
        public int ExperienceTotal { get; set; }
        [SerializedName("gameId")]
        public long GameId { get; set; }
        [SerializedName("loyaltyBoostXpEarned")]
        public int LoyaltyBoostXpEarned { get; set; }
        [SerializedName("elo")]
        public int Elo { get; set; }
        [SerializedName("roomPassword")]
        public string RoomPassword { get; set; }
        [SerializedName("firstWinBonus")]
        public int FirstWinBonus { get; set; }
        [SerializedName("eloChange")]
        public int EloChange { get; set; }
        [SerializedName("myTeamInfo")]
        public TeamInfo MyTeamInfo { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("customMsecsUntilReset")]
        public long CustomMsecsUntilReset { get; set; }
        [SerializedName("leveledUp")]
        public bool LeveledUp { get; set; }
        [SerializedName("gameType")]
        public string GameType { get; set; }
        [SerializedName("queueBonusEarned")]
        public int QueueBonusEarned { get; set; }
        [SerializedName("imbalancedTeamsNoPoints")]
        public bool ImbalancedTeamsNoPoints { get; set; }
        [SerializedName("experienceEarned")]
        public int ExperienceEarned { get; set; }
        [SerializedName("reportGameId")]
        public long ReportGameId { get; set; }
        [SerializedName("gameLength")]
        public long GameLength { get; set; }
        [SerializedName("otherTeamInfo")]
        public TeamInfo OtherTeamInfo { get; set; }
        [SerializedName("customMinutesLeftToday")]
        public int CustomMinutesLeftToday { get; set; }
        [SerializedName("coOpVsAiMinutesLeftToday")]
        public int CoOpVsAiMinutesLeftToday { get; set; }
        [SerializedName("pointsPenalties")]
        public List<object> PointsPenalties { get; set; }
        [SerializedName("otherTeamPlayerParticipantStats")]
        public List<PlayerParticipantStatsSummary> OtherTeamPlayerParticipantStats { get; set; }
        [SerializedName("loyaltyBoostIpEarned")]
        public int LoyaltyBoostIpEarned { get; set; }
        [SerializedName("boostIpEarned")]
        public int BoostIpEarned { get; set; }
        [SerializedName("coOpVsAiMsecsUntilReset")]
        public long CoOpVsAiMsecsUntilReset { get; set; }
        [SerializedName("completionBonusPoints")]
        public int CompletionBonusPoints { get; set; }
        [SerializedName("newSpells")]
        public List<object> NewSpells { get; set; }
        [SerializedName("timeUntilNextFirstWinBonus")]
        public long TimeUntilNextFirstWinBonus { get; set; }
        [SerializedName("ipEarned")]
        public int IpEarned { get; set; }
        [SerializedName("sendStatsToTournamentProvider")]
        public bool SendStatsToTournamentProvider { get; set; }
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("gameMutators")]
        public List<object> GameMutators { get; set; }
        [SerializedName("odinBonusIp")]
        public int OdinBonusIp { get; set; }
        [SerializedName("queueType")]
        public string QueueType { get; set; }
        [SerializedName("myTeamStatus")]
        public string MyTeamStatus { get; set; }
        [SerializedName("ipTotal")]
        public int IpTotal { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.Inviter")]
    public class Inviter {
        [SerializedName("previousSeasonHighestTier")]
        public string PreviousSeasonHighestTier { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.InvitationRequest")]
    public class InvitationRequest {
        [SerializedName("invitePayload")]
        public string InvitePayload { get; set; }
        [SerializedName("inviter")]
        public Inviter Inviter { get; set; }
        [SerializedName("inviteType")]
        public InviteType InviteType { get; set; }
        [SerializedName("gameMetaData")]
        public string GameMetaData { get; set; }
        [SerializedName("owner")]
        public Player Owner { get; set; }
        [SerializedName("invitationStateAsString")]
        public string InvitationStateAsString { get; set; }
        [SerializedName("invitationState")]
        public InvitationState InvitationState { get; set; }
        [SerializedName("inviteTypeAsString")]
        public string InviteTypeAsString { get; set; }
        [SerializedName("invitationId")]
        public string InvitationId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.icon.IconType")]
    public class IconType {
        [SerializedName("iconTypeId")]
        public int IconTypeId { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.catalog.icon.Icon")]
    public class Icon {
        [SerializedName("imagePath")]
        public object ImagePath { get; set; }
        [SerializedName("toolTip")]
        public object ToolTip { get; set; }
        [SerializedName("tier")]
        public object Tier { get; set; }
        [SerializedName("itemId")]
        public int ItemId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("duration")]
        public object Duration { get; set; }
        [SerializedName("gameCode")]
        public int GameCode { get; set; }
        [SerializedName("itemEffects")]
        public object ItemEffects { get; set; }
        [SerializedName("baseType")]
        public string BaseType { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("description")]
        public object Description { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("uses")]
        public object Uses { get; set; }
        [SerializedName("iconType")]
        public IconType IconType { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.icon.SummonerIcon")]
    public class SummonerIcon {
        [SerializedName("icon")]
        public Icon Icon { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }
        [SerializedName("iconId")]
        public int IconId { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.icon.SummonerIconInventoryDTO")]
    public class SummonerIconInventoryDTO {
        [SerializedName("summonerIcons")]
        public List<SummonerIcon> SummonerIcons { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("dateString")]
        public string DateString { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("summonerIconJson")]
        public object SummonerIconJson { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.ChampionBanInfoDTO")]
    public class ChampionBanInfoDTO {
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("enemyOwned")]
        public bool EnemyOwned { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("owned")]
        public bool Owned { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.practice.PracticeGameSearchResult")]
    public class PracticeGameSearchResult {
        [SerializedName("spectatorCount")]
        public int SpectatorCount { get; set; }
        [SerializedName("glmGameId")]
        public object GlmGameId { get; set; }
        [SerializedName("glmHost")]
        public object GlmHost { get; set; }
        [SerializedName("glmPort")]
        public int GlmPort { get; set; }
        [SerializedName("gameModeString")]
        public string GameModeString { get; set; }
        [SerializedName("allowSpectators")]
        public string AllowSpectators { get; set; }
        [SerializedName("gameMapId")]
        public int GameMapId { get; set; }
        [SerializedName("maxNumPlayers")]
        public int MaxNumPlayers { get; set; }
        [SerializedName("glmSecurePort")]
        public int GlmSecurePort { get; set; }
        [SerializedName("gameMode")]
        public string GameMode { get; set; }
        [SerializedName("id")]
        public long Id { get; set; }
        [SerializedName("gameMutators")]
        public List<string> GameMutators { get; set; }
        [SerializedName("privateGame")]
        public bool PrivateGame { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("owner")]
        public PlayerParticipant Owner { get; set; }
        [SerializedName("team1Count")]
        public int Team1Count { get; set; }
        [SerializedName("team2Count")]
        public int Team2Count { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.matchmaking.MatchMakerParams")]
    public class MatchMakerParams {
        [SerializedName("languages")]
        public object Languages { get; set; }
        [SerializedName("queueIds")]
        public List<int> QueueIds { get; set; }
        [SerializedName("invitationId")]
        public object InvitationId { get; set; }
        [SerializedName("teamId")]
        public object TeamId { get; set; }
        [SerializedName("lastMaestroMessage")]
        public object LastMaestroMessage { get; set; }
        [SerializedName("team")]
        public List<long> Team { get; set; }
        [SerializedName("botDifficulty")]
        public string BotDifficulty { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.matchmaking.QueueInfo")]
    public class QueueInfo {
        [SerializedName("waitTime")]
        public long WaitTime { get; set; }
        [SerializedName("queueId")]
        public int QueueId { get; set; }
        [SerializedName("queueLength")]
        public int QueueLength { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.matchmaking.SearchingForMatchNotification")]
    public class SearchingForMatchNotification {
        [SerializedName("ghostGameSummoners")]
        public List<object> GhostGameSummoners { get; set; }
        [SerializedName("joinedQueues")]
        public List<QueueInfo> JoinedQueues { get; set; }
        [SerializedName("playerJoinFailures")]
        public List<FailedJoinPlayer> PlayerJoinFailures { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.messaging.StoreFulfillmentNotification")]
    public class StoreFulfillmentNotification {
        [SerializedName("rp")]
        public int RP { get; set; }
        [SerializedName("inventoryType")]
        public string InventoryType { get; set; }
        [SerializedName("data")]
        public ChampionDTO Data { get; set; }
        [SerializedName("ip")]
        public int IP { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.messaging.StoreAccountBalanceNotification")]
    public class StoreAccountBalanceNotification {
        [SerializedName("rp")]
        public int RP { get; set; }
        [SerializedName("ip")]
        public int IP { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.PlayerCredentialsDto")]
    public class PlayerCredentialsDto {
        [SerializedName("encryptionKey")]
        public string EncryptionKey { get; set; }
        [SerializedName("gameId")]
        public long GameId { get; set; }
        [SerializedName("serverIp")]
        public string ServerIp { get; set; }
        [SerializedName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }
        [SerializedName("observer")]
        public bool Observer { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
        [SerializedName("observerServerIp")]
        public string ObserverServerIp { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("handshakeToken")]
        public string HandshakeToken { get; set; }
        [SerializedName("playerId")]
        public double PlayerId { get; set; }
        [SerializedName("serverPort")]
        public int ServerPort { get; set; }
        [SerializedName("observerServerPort")]
        public int ObserverServerPort { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("observerEncryptionKey")]
        public string ObserverEncryptionKey { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.messaging.ClientLoginKickNotification")]
    public class ClientLoginKickNotification {
        [SerializedName("sessionToken")]
        public string SessionToken { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.leagues.pojo.MiniSeriesDTO")]
    public class MiniSeriesDTO {
        [SerializedName("progress")]
        public string Progress { get; set; }
        [SerializedName("target")]
        public int Target { get; set; }
        [SerializedName("losses")]
        public int Losses { get; set; }
        [SerializedName("timeLeftToPlayMillis")]
        public long TimeLeftToPlayMillis { get; set; }
        [SerializedName("wins")]
        public int Wins { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.BannedChampion")]
    public class BannedChampion {
        [SerializedName("pickTurn")]
        public int PickTurn { get; set; }
        [SerializedName("dataVersion")]
        public int DataVersion { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("teamId")]
        public int TeamId { get; set; }
        [SerializedName("futureData")]
        public object FutureData { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.InvitePrivileges")]
    public class InvitePrivileges {
        [SerializedName("canInvite")]
        public bool CanInvite { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.SummonerGameModeSpells")]
    public class SummonerGameModeSpells {
        [SerializedName("spell2Id")]
        public int Spell2Id { get; set; }
        [SerializedName("spell1Id")]
        public int Spell1Id { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.message.GameNotification")]
    public class GameNotification {
        [SerializedName("messageCode")]
        public string MessageCode { get; set; }
        [SerializedName("messageArgument")]
        public string MessageArgument { get; set; }
        [SerializedName("type")]
        public GameNotificationType Type { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.trade.api.contract.TradeContractDTO")]
    public class TradeContractDTO {
        [SerializedName("responderInternalSummonerName")]
        public string ResponderInternalSummonerName { get; set; }
        [SerializedName("requesterChampionId")]
        public int RequesterChampionId { get; set; }
        [SerializedName("state")]
        public string State { get; set; }
        [SerializedName("requesterInternalSummonerName")]
        public string RequesterInternalSummonerName { get; set; }
        [SerializedName("responderChampionId")]
        public int ResponderChampionId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.messaging.persistence.SimpleDialogMessage")]
    public class SimpleDialogMessage {
        [SerializedName("titleCode")]
        public string TitleCode { get; set; }
        [SerializedName("accountId")]
        public long AccountId { get; set; }
        [SerializedName("msgId")]
        public string MsgId { get; set; }
        [SerializedName("params")]
        public object[] Params { get; set; }
        [SerializedName("type")]
        public string Type { get; set; }
        [SerializedName("bodyCode")]
        public string BodyCode { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.PlatformGameLifecycleDTO")]
    public class PlatformGameLifecycleDTO {
        [SerializedName("gameSpecificLoyaltyRewards")]
        public object GameSpecificLoyaltyRewards { get; set; }
        [SerializedName("reconnectDelay")]
        public int ReconnectDelay { get; set; }
        [SerializedName("lastModifiedDate")]
        public object LastModifiedDate { get; set; }
        [SerializedName("game")]
        public GameDTO Game { get; set; }
        [SerializedName("playerCredentials")]
        public PlayerCredentialsDto PlayerCredentials { get; set; }
        [SerializedName("gameName")]
        public string GameName { get; set; }
        [SerializedName("connectivityStateEnum")]
        public string ConnectivityStateEnum { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.reroll.pojo.EogPointChangeBreakdown")]
    public class EogPointChangeBreakdown {
        [SerializedName("pointChangeFromGameplay")]
        public int PointChangeFromGameplay { get; set; }
        [SerializedName("pointChangeFromChampionsOwned")]
        public int PointChangeFromChampionsOwned { get; set; }
        [SerializedName("previousPoints")]
        public int PreviousPoints { get; set; }
        [SerializedName("pointsUsed")]
        public int PointsUsed { get; set; }
        [SerializedName("endPoints")]
        public int EndPoints { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.trade.api.contract.PotentialTradersDTO")]
    public class PotentialTradersDTO {
        [SerializedName("potentialTraders")]
        public List<string> PotentialTraders { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.TeamSkinRentalDTO")]
    public class TeamSkinRentalDTO {
        [SerializedName("unlocked")]
        public bool Unlocked { get; set; }
        [SerializedName("price")]
        public int Price { get; set; }
        [SerializedName("ipReward")]
        public int IpReward { get; set; }
        [SerializedName("skinUnlockMode")]
        public string SkinUnlockMode { get; set; }
        [SerializedName("availableSkins")]
        public List<object> AvailableSkins { get; set; }
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("ipRewardForPurchaser")]
        public int IpRewardForPurchaser { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.reroll.pojo.RollResult")]
    public class RollResult {
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("pointSummary")]
        public PointSummary PointSummary { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.AllPublicSummonerDataDTO")]
    public class AllPublicSummonerDataDTO {
        [SerializedName("spellBook")]
        public SpellBookDTO SpellBook { get; set; }
        [SerializedName("summonerDefaultSpells")]
        public SummonerDefaultSpells SummonerDefaultSpells { get; set; }
        [SerializedName("summonerTalentsAndPoints")]
        public SummonerTalentsAndPoints SummonerTalentsAndPoints { get; set; }
        [SerializedName("summoner")]
        public BasePublicSummonerDTO Summoner { get; set; }
        [SerializedName("summonerLevelAndPoints")]
        public SummonerLevelAndPoints SummonerLevelAndPoints { get; set; }
        [SerializedName("summonerLevel")]
        public SummonerLevel SummonerLevel { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.summoner.BasePublicSummonerDTO")]
    public class BasePublicSummonerDTO {
        [SerializedName("seasonTwoTier")]
        public string SeasonTwoTier { get; set; }
        [SerializedName("publicName")]
        public string InternalName { get; set; }
        [SerializedName("seasonOneTier")]
        public string SeasonOneTier { get; set; }
        [SerializedName("acctId")]
        public long AccountId { get; set; }
        [SerializedName("name")]
        public string Name { get; set; }
        [SerializedName("sumId")]
        public long SummonerId { get; set; }
        [SerializedName("profileIconId")]
        public int ProfileIconId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.gameinvite.contract.RemovedFromLobbyNotification")]
    public class RemovedFromLobbyNotification {
        [SerializedName("removalReason")]
        public string RemovalReason { get; set; }
        [SerializedName("removalReasonAsString")]
        public string RemovalReasonAsString { get; set; }
    }
}
