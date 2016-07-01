using System;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class MatchmakingState : JSONSerializable {
        [JSONField("estimate")]
        public long EstimatedDuration { get; set; }

        [JSONField("actual")]
        public long ActualDuration { get; set; }

        [JSONField("afkCheck")]
        public AfkCheck AfkCheck { get; set; }

        [JSONField("chatroom")]
        public Guid Chatroom { get; set; }
    }

    public class AfkCheck : JSONSerializable {
        [JSONField("accepted")]
        public bool? Accepted { get; set; }
        [JSONField("duration")]
        public long Duration { get; set; }
        [JSONField("remaining")]
        public long Remaining { get; set; }
    }
}
