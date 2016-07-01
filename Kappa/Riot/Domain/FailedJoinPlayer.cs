using RtmpSharp.IO;
using System;

namespace Kappa.Riot.Domain {
    [Serializable, SerializedName("com.riotgames.platform.game.FailedJoinPlayer")]
    public class FailedJoinPlayer {
        [SerializedName("reasonFailed")]
        public string ReasonFailed { get; set; }
        [SerializedName("summoner")]
        public Summoner Summoner { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.matchmaking.QueueDodger")]
    public class QueueDodger : FailedJoinPlayer {
        [SerializedName("dodgePenaltyRemainingTime")]
        public long DodgePenaltyRemainingTime { get; set; }
    }
}
