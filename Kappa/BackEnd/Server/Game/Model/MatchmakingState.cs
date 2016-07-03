using System;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class MatchmakingState {
        public long Estimate { get; set; }
        public long Actual { get; set; }
        public AfkCheck AfkCheck { get; set; }
        public Guid Chatroom { get; set; }
    }

    [JSONSerializable]
    public class AfkCheck {
        public bool? Accepted { get; set; }
        public long Duration { get; set; }
        public long Remaining { get; set; }
    }
}
