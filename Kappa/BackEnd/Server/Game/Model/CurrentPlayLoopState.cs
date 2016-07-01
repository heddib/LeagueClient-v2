using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class CurrentPlayLoopState : JSONSerializable {
        [JSONField("inPlayLoop")]
        public bool InPlayLoop { get; }

        [JSONField("queueId")]
        public int QueueId { get; }

        [JSONField("queueConfigId")]
        public int QueueConfigId { get; set; }

        public CurrentPlayLoopState(bool inLoop, int queueId, int config) {
            InPlayLoop = inLoop;
            QueueId = queueId;
            QueueConfigId = config;
        }
    }
}
