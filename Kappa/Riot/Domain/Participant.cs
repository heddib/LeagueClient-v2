using RtmpSharp.IO;
using System;

namespace Kappa.Riot.Domain {
    [Serializable, SerializedName("com.riotgames.platform.game.GameParticipant")]
    public class GameParticipant {
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }
        [SerializedName("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [SerializedName("pickMode")]
        public int PickMode { get; set; }
        [SerializedName("pickTurn")]
        public int PickTurn { get; set; }
        [SerializedName("badges")]
        public int Badges { get; set; }
        [SerializedName("isMe")]
        public bool IsMe { get; set; }
        [SerializedName("isGameOwner")]
        public bool IsGameOwner { get; set; }

        [SerializedName("team")]
        public int Team { get; set; }
        [SerializedName("teamName")]
        public string TeamName { get; set; }
        [SerializedName("isFriendly")]
        public bool IsFriendly { get; set; }

        [SerializedName("clubTag")]
        public string ClubTag { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.ObfuscatedParticipant")]
    public class ObfuscatedParticipant : GameParticipant {
        [SerializedName("index")]
        public int Index { get; set; }
        [SerializedName("clientInSynch")]
        public bool ClientInSynch { get; set; }
        [SerializedName("gameUniqueId")]
        public int GameUniqueId { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.reroll.pojo.AramPlayerParticipant")]
    public class ARAMPlayerParticipant : PlayerParticipant {
        [SerializedName("pointSummary")]
        public PointSummary PointSummary { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.BotParticipant")]
    public class BotParticipant : GameParticipant {
        [SerializedName("botSkillLevel")]
        public int BotSkillLevel { get; set; }
        [SerializedName("botSkillLevelName")]
        public string BotSkillLevelName { get; set; }
        [SerializedName("champion")]
        public ChampionDTO Champion { get; set; }
        [SerializedName("teamId")]
        public string TeamId { get; set; }

        [SerializedName("botDifficulty")]
        public string BotDifficulty { get; set; }
        [SerializedName("locale")]
        public string Locale { get; set; }
        [SerializedName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }
        [SerializedName("spell2Id")]
        public int Spell2Id { get; set; }
        [SerializedName("role")]
        public string Role { get; set; }
        [SerializedName("championId")]
        public int ChampionId { get; set; }
        [SerializedName("spell1Id")]
        public int Spell1Id { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.game.PlayerParticipant")]
    public class PlayerParticipant : GameParticipant {
        [SerializedName("accountId")]
        public long AccountId { get; set; }
        [SerializedName("queueRating")]
        public int QueueRating { get; set; }
        [SerializedName("botDifficulty")]
        public string BotDifficulty { get; set; }
        [SerializedName("minor")]
        public bool Minor { get; set; }
        [SerializedName("locale")]
        public object Locale { get; set; }
        [SerializedName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }
        [SerializedName("partnerId")]
        public string PartnerId { get; set; }
        [SerializedName("profileIconId")]
        public int ProfileIconId { get; set; }
        [SerializedName("rankedTeamGuest")]
        public bool RankedTeamGuest { get; set; }
        [SerializedName("summonerId")]
        public long SummonerId { get; set; }
        [SerializedName("voterRating")]
        public int VoterRating { get; set; }
        [SerializedName("selectedRole")]
        public object SelectedRole { get; set; }
        [SerializedName("teamParticipantId")]
        public object TeamParticipantId { get; set; }
        [SerializedName("timeAddedToQueue")]
        public object TimeAddedToQueue { get; set; }
        [SerializedName("index")]
        public int Index { get; set; }
        [SerializedName("originalAccountNumber")]
        public long OriginalAccountNumber { get; set; }
        [SerializedName("adjustmentFlags")]
        public double AdjustmentFlags { get; set; }
        [SerializedName("teamOwner")]
        public bool TeamOwner { get; set; }
        [SerializedName("teamRating")]
        public int TeamRating { get; set; }
        [SerializedName("clientInSynch")]
        public bool ClientInSynch { get; set; }
        [SerializedName("originalPlatformId")]
        public string OriginalPlatformId { get; set; }
        [SerializedName("selectedPosition")]
        public object SelectedPosition { get; set; }
    }
}
