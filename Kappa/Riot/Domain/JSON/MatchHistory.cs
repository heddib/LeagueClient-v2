using MFroehlich.League.RiotAPI;
using MFroehlich.Parsing.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.Riot.Domain.JSON {
    public class PlayerDeltas : JSONSerializable {
        [JSONField("originalAccountId")]
        public long OriginalAccountId { get; set; }

        [JSONField("originalPlatformId")]
        public string OriginalPlatform { get; set; }

        [JSONField("deltas")]
        public List<GameDeltaInfo> Deltas { get; set; }
    }

    public class GameDeltaInfo : JSONSerializable {
        [JSONField("gamePlatformId")]
        public string Platform { get; set; }

        [JSONField("gameId")]
        public long GameId { get; set; }

        [JSONField("platformDelta")]
        public Delta Delta { get; set; }
    }

    public class Delta : JSONSerializable {
        [JSONField("gamePlatformId")]
        public string Platform { get; set; }

        [JSONField("gameId")]
        public long GameId { get; set; }

        [JSONField("xpDelta")]
        public int XP { get; set; }

        [JSONField("ipDelta")]
        public int IP { get; set; }

        [JSONField("compensationModeEnabled")]
        public bool IsCompensationModeEnabled { get; set; }

        [JSONField("timestamp")]
        public long Timestamp { get; set; }
    }


    public class PlayerHistory : JSONSerializable {
        [JSONField("platformId")]
        public string Platform { get; set; }

        [JSONField("accountId")]
        public long AccountId { get; set; }

        [JSONField("shownQueues")]
        public List<int> ShownQueues { get; set; }

        [JSONField("games")]
        public GameResponseInfo Games { get; set; }
    }

    public class GameResponseInfo : JSONSerializable {
        [JSONField("gameIndexBegin")]
        public int GameIndexBegin { get; set; }

        [JSONField("gameIndexEnd")]
        public int GameIndexEnd { get; set; }

        [JSONField("gameTimestampBegin")]
        public int GameTimestampBegin { get; set; }

        [JSONField("gameTimestampEnd")]
        public int GameTimestampEnd { get; set; }

        [JSONField("gameCount")]
        public int GameCount { get; set; }

        [JSONField("games")]
        public List<MatchDetails> Games { get; set; }
    }

    public class MatchDetails : JSONSerializable {
        [JSONField("gameId")]
        public long GameId { get; set; }

        [JSONField("platformId")]
        public string Platform { get; set; }

        [JSONField("gameCreation")]
        public long GameCreation { get; set; }

        [JSONField("gameDuration")]
        public int GameDuration { get; set; }

        [JSONField("queueId")]
        public int QueueId { get; set; }

        [JSONField("mapId")]
        public int MapId { get; set; }

        [JSONField("seasonId")]
        public int SeasonId { get; set; }

        [JSONField("gameVersion")]
        public string GameVersion { get; set; }

        [JSONField("gameMode")]
        public string GameMode { get; set; }

        [JSONField("gameType")]
        public string GameType { get; set; }

        [JSONField("teams")]
        public List<Team> Teams { get; set; }

        [JSONField("participants")]
        public List<Participant> Participants { get; set; }

        [JSONField("participantIdentities")]
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }
    }

    public class Participant : JSONSerializable {
        [JSONField("participantId")]
        public int ParticipantId { get; set; }

        [JSONField("teamId")]
        public int TeamId { get; set; }

        [JSONField("championId")]
        public int ChampionId { get; set; }

        [JSONField("spell1Id")]
        public int Spell1Id { get; set; }

        [JSONField("spell2Id")]
        public int Spell2Id { get; set; }

        [JSONField("masteries")]
        public List<MatchAPI.Mastery> Masteries { get; set; }

        [JSONField("runes")]
        public List<MatchAPI.Rune> Runes { get; set; }

        [JSONField("stats")]
        public ParticipantStats Stats { get; set; }

        [JSONField("timeline")]
        public JSONObject Timeline { get; set; }

        [JSONField("highestAchievedSeasonTier")]
        public string HighestAchievedSeasonTier { get; set; }
    }

    public class ParticipantStats : JSONSerializable {
        [JSONField("participantId")]
        public int ParticipantId { get; set; }

        [JSONField("win")]
        public bool Win { get; set; }

        [JSONField("item0")]
        public int Item0 { get; set; }

        [JSONField("item1")]
        public int Item1 { get; set; }

        [JSONField("item2")]
        public int Item2 { get; set; }

        [JSONField("item3")]
        public int Item3 { get; set; }

        [JSONField("item4")]
        public int Item4 { get; set; }

        [JSONField("item5")]
        public int Item5 { get; set; }

        [JSONField("item6")]
        public int Item6 { get; set; }

        [JSONField("kills")]
        public int Kills { get; set; }

        [JSONField("deaths")]
        public int Deaths { get; set; }

        [JSONField("assists")]
        public int Assists { get; set; }

        [JSONField("largestKillingSpree")]
        public int LargestKillingSpree { get; set; }

        [JSONField("largestMultiKill")]
        public int LargestMultiKill { get; set; }

        [JSONField("killingSprees")]
        public int KillingSprees { get; set; }

        [JSONField("longestTimeSpentLiving")]
        public int LongestTimeSpentLiving { get; set; }

        [JSONField("doubleKills")]
        public int DoubleKills { get; set; }

        [JSONField("tripleKills")]
        public int TripleKills { get; set; }

        [JSONField("quadraKills")]
        public int QuadraKills { get; set; }

        [JSONField("pentaKills")]
        public int PentaKills { get; set; }

        [JSONField("unrealKills")]
        public int UnrealKills { get; set; }

        [JSONField("totalDamageDealt")]
        public int TotalDamageDealt { get; set; }

        [JSONField("magicDamageDealt")]
        public int MagicDamageDealt { get; set; }

        [JSONField("physicalDamageDealt")]
        public int PhysicalDamageDealt { get; set; }

        [JSONField("trueDamageDealt")]
        public int TrueDamageDealt { get; set; }

        [JSONField("largestCriticalStrike")]
        public int LargestCriticalStrike { get; set; }

        [JSONField("totalDamageDealtToChampions")]
        public int TotalDamageDealtToChampions { get; set; }

        [JSONField("magicDamageDealtToChampions")]
        public int MagicDamageDealtToChampions { get; set; }

        [JSONField("physicalDamageDealtToChampions")]
        public int PhysicalDamageDealtToChampions { get; set; }

        [JSONField("trueDamageDealtToChampions")]
        public int TrueDamageDealtToChampions { get; set; }

        [JSONField("totalHeal")]
        public int TotalHeal { get; set; }

        [JSONField("totalUnitsHealed")]
        public int TotalUnitsHealed { get; set; }

        [JSONField("totalDamageTaken")]
        public int TotalDamageTaken { get; set; }

        [JSONField("magicalDamageTaken")]
        public int MagicalDamageTaken { get; set; }

        [JSONField("physicalDamageTaken")]
        public int PhysicalDamageTaken { get; set; }

        [JSONField("trueDamageTaken")]
        public int TrueDamageTaken { get; set; }

        [JSONField("goldEarned")]
        public int GoldEarned { get; set; }

        [JSONField("goldSpent")]
        public int GoldSpent { get; set; }

        [JSONField("turretKills")]
        public int TurretKills { get; set; }

        [JSONField("inhibitorKills")]
        public int InhibitorKills { get; set; }

        [JSONField("totalMinionsKilled")]
        public int TotalMinionsKilled { get; set; }

        [JSONField("neutralMinionsKilled")]
        public int NeutralMinionsKilled { get; set; }

        [JSONField("neutralMinionsKilledTeamJungle")]
        public int NeutralMinionsKilledTeamJungle { get; set; }

        [JSONField("neutralMinionsKilledEnemyJungle")]
        public int NeutralMinionsKilledEnemyJungle { get; set; }

        [JSONField("totalTimeCrowdControlDealt")]
        public int TotalTimeCrowdControlDealt { get; set; }

        [JSONField("champLevel")]
        public int ChampLevel { get; set; }

        [JSONField("visionWardsBoughtInGame")]
        public int VisionWardsBoughtInGame { get; set; }

        [JSONField("sightWardsBoughtInGame")]
        public int SightWardsBoughtInGame { get; set; }

        [JSONField("wardsPlaced")]
        public int WardsPlaced { get; set; }

        [JSONField("wardsKilled")]
        public int WardsKilled { get; set; }

        [JSONField("firstBloodKill")]
        public bool FirstBloodKill { get; set; }

        [JSONField("firstBloodAssist")]
        public bool FirstBloodAssist { get; set; }

        [JSONField("firstTowerKill")]
        public bool FirstTowerKill { get; set; }

        [JSONField("firstTowerAssist")]
        public bool FirstTowerAssist { get; set; }

        [JSONField("firstInhibitorKill")]
        public bool FirstInhibitorKill { get; set; }

        [JSONField("firstInhibitorAssist")]
        public bool FirstInhibitorAssist { get; set; }

        [JSONField("combatPlayerScore")]
        public int CombatPlayerScore { get; set; }

        [JSONField("objectivePlayerScore")]
        public int ObjectivePlayerScore { get; set; }

        [JSONField("totalPlayerScore")]
        public int TotalPlayerScore { get; set; }

        [JSONField("totalScoreRank")]
        public int TotalScoreRank { get; set; }

        [JSONField("wasAfk")]
        public bool WasAFk { get; set; }

        [JSONField("leaver")]
        public bool Leaver { get; set; }

        [JSONField("gameEndedInEarlySurrender")]
        public bool GameEndedInEarlySurrender { get; set; }

        [JSONField("gameEndedInSurrender")]
        public bool GameEndedInSurrender { get; set; }

        [JSONField("causedEarlySurrender")]
        public bool CausedEarlySurrender { get; set; }

        [JSONField("earlySurrenderAccomplice")]
        public bool EarlySurrenderAccomplice { get; set; }

        [JSONField("teamEarlySurrendered")]
        public bool TeamEarlySurrendered { get; set; }
    }

    public class Team : JSONSerializable {
        [JSONField("teamId")]
        public int TeamId { get; set; }

        [JSONField("win")]
        public string Win { get; set; }

        [JSONField("firstBlood")]
        public bool FirstBlood { get; set; }

        [JSONField("firstTower")]
        public bool FirstTower { get; set; }

        [JSONField("firstInhibitor")]
        public bool FirstInhibitor { get; set; }

        [JSONField("firstBaron")]
        public bool FirstBaron { get; set; }

        [JSONField("firstDragon")]
        public bool FirstDragon { get; set; }

        [JSONField("towerKills")]
        public int TowerKills { get; set; }

        [JSONField("inhibitorKills")]
        public int InhibitorKills { get; set; }

        [JSONField("baronKills")]
        public int BaronKills { get; set; }

        [JSONField("dragonKills")]
        public int DragonKills { get; set; }

        [JSONField("vilemawKills")]
        public int VilemawKills { get; set; }

        [JSONField("dominionVictoryScore")]
        public int DominionVictoryScore { get; set; }

        [JSONField("bans")]
        public List<MatchAPI.BannedChampion> Bans { get; set; }
    }

    public class ParticipantIdentity : JSONSerializable {
        [JSONField("participantId")]
        public int ParticipantId { get; set; }

        [JSONField("player")]
        public Player Player { get; set; }
    }

    public class Player : JSONSerializable {
        [JSONField("platformId")]
        public int ParticipantId { get; set; }

        [JSONField("accountId")]
        public long AccountId { get; set; }

        [JSONField("summonerName")]
        public string SummonerName { get; set; }

        [JSONField("summonerId")]
        public long SummonerId { get; set; }

        [JSONField("currentPlatformId")]
        public string CurrentPlatform { get; set; }

        [JSONField("currentAccountId")]
        public long CurrentAccountId { get; set; }

        [JSONField("matchHistoryUri")]
        public string MatchHistoryURI { get; set; }

        [JSONField("profileIcon")]
        public int ProfileIconId { get; set; }
    }
}
