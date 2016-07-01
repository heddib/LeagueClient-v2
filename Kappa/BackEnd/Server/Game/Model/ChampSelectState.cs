using System;
using MFroehlich.Parsing.JSON;
using System.Collections.Generic;

namespace Kappa.BackEnd.Server.Game.Model {
    public class ChampSelectState : JSONSerializable {
        [JSONField("phase")]
        public ChampSelectPhase Phase { get; set; }

        [JSONField("alliedBans")]
        public List<int> AlliedBans { get; set; } = new List<int>();
        [JSONField("enemyBans")]
        public List<int> EnemyBans { get; set; } = new List<int>();

        [JSONField("isBlue")]
        public bool IsBlue { get; set; }
        [JSONField("allies")]
        public List<GameMember> Allies { get; set; }
        [JSONField("enemies")]
        public List<GameMember> Enemies { get; set; }

        [JSONField("me")]
        public GameMember Me { get; set; }

        [JSONField("remaining")]
        public long Remaining { get; set; }
        [JSONField("turn")]
        public int Turn { get; set; }

        [JSONField("inventory")]
        public Inventory Inventory { get; set; }

        [JSONField("chatroom")]
        public Guid Chatroom { get; set; }
    }

    public enum ChampSelectPhase {
        PLANNING,
        BANNING,
        PICKING,
        FINALIZING
    }
}