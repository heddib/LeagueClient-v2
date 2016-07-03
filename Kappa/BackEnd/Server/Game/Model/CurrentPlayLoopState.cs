using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class CurrentPlayLoopState {
        public bool InPlayLoop { get; }

        public int QueueId { get; }
        public int QueueConfigId { get; set; }

        public CurrentPlayLoopState(bool inLoop, int queueId, int config) {
            InPlayLoop = inLoop;
            QueueId = queueId;
            QueueConfigId = config;
        }
    }
}
