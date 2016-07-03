using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;
using System.Diagnostics;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class AvailableQueue {
        private GameQueueConfig config;

        public int Config => config.GameTypeConfigId;
        public string Name => config.CacheName;
        public int Id => config.Id;

        public AvailableQueue() { }
        public AvailableQueue(GameQueueConfig config) {
            this.config = config;
        }
    }
}
