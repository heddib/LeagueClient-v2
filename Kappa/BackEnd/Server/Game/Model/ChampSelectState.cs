using System;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    [JSONSerializable]
    public class ChampSelectState {
        public ChampSelectPhase Phase { get; set; }

        public List<int> AlliedBans { get; set; } = new List<int>();
        public List<int> EnemyBans { get; set; } = new List<int>();

        public bool IsBlue { get; set; }
        public List<GameMember> Allies { get; set; }
        public List<GameMember> Enemies { get; set; }

        public GameMember Me { get; set; }

        public long Remaining { get; set; }
        public int Turn { get; set; }

        public Inventory Inventory { get; set; }

        public Guid Chatroom { get; set; }
    }

    public enum ChampSelectPhase {
        PLANNING,
        BANNING,
        PICKING,
        FINALIZING
    }
}