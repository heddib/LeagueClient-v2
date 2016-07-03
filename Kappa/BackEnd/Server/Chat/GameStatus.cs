using System;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Chat {
    public class GameStatus : IComparable<GameStatus> {
        public string Key { get; }
        public string Display { get; }
        public int Value { get; }

        private GameStatus(string value, string display) {
            Key = value;
            Display = display;
            Value = counter++;
            Values[Key] = this;
        }

        public int CompareTo(GameStatus other) {
            return Value.CompareTo(other.Value);
        }

        public static Dictionary<string, GameStatus> Values { get; } = new Dictionary<string, GameStatus>();

        // ReSharper disable InconsistentNaming
        public static readonly GameStatus
          outOfGame = new GameStatus("outOfGame", "Out of Game"),
          inTeamBuilder = new GameStatus("inTeamBuilder", "In Team Builder"),
          hostingPracticeGame = new GameStatus("hostingPracticeGame", "Creating Custom Game"),
          hostingRankedGame = new GameStatus("hostingRankedGame", "Creating Ranked Game"),
          hostingCoopVsAIGame = new GameStatus("hostingCoopVsAIGame", "Creating Bot Game"),
          hostingNormalGame = new GameStatus("hostingNormalGame", "Creating Normal Game"),
          teamSelect = new GameStatus("teamSelect", "In Team Select"),
          spectating = new GameStatus("spectating", "Spectating"),
          inQueue = new GameStatus("inQueue", "In Queue"),
          inGame = new GameStatus("inGame", "In Game"),
          tutorial = new GameStatus("tutorial", "Tutorial"),
          championSelect = new GameStatus("championSelect", "In Champion Select");
        // ReSharper restore InconsistentNaming

        public static implicit operator string(GameStatus status) {
            return status.Key;
        }

        private static int counter = 1;
    }
}
