using Kappa.Riot.Domain;
using MFroehlich.League.Assets;
using MFroehlich.Parsing.JSON;
using System.Diagnostics;
using System.Linq;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class AvailableQueue {
        private GameQueueConfig config;

        public string Name => config.CacheName;
        public int Config => config.GameTypeConfigId;
        public int Id => config.Id;
        public int Map { get; }

        public AvailableQueue() { }
        public AvailableQueue(GameQueueConfig config) {
            this.config = config;
            Map = config.SupportedMapIds.Single(m => DataDragon.MapData.Value.data.ContainsKey(m.ToString()));
        }
    }
}
