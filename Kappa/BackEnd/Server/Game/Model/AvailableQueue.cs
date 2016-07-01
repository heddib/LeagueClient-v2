using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;
using System.Diagnostics;

namespace Kappa.BackEnd.Server.Game.Model {
    public class AvailableQueue : JSONValuable {
        private GameQueueConfig config;

        public int ConfigId => config.GameTypeConfigId;
        public string Name => config.CacheName;
        public int Id => config.Id;

        public AvailableQueue() { }
        public AvailableQueue(GameQueueConfig config) {
            Debug.WriteLine($"{config.CacheName}: {config.GameTypeConfigId}");
            this.config = config;
        }

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["config"] = config.GameTypeConfigId,
                ["name"] = Name,
                ["id"] = Id,
            };
        }
    }
}
