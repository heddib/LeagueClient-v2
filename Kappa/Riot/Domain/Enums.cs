namespace Kappa.Riot.Domain {
    public enum GameNotificationType {
        PLAYER_BANNED_FROM_GAME,
        PLAYER_REMOVED,
        TEAM_REMOVED,
        PLAYER_QUIT
    }

    public enum InviteType {
        DEFAULT,
        TB_PLAY_AGAIN,
        VICTORIOUS_COMRADE,
        HONORED_PLAYER,
        FRIEND_OF_FRIEND
    }

    public enum InvitationState {
        ACTIVE,
        ON_HOLD,
        REVOKED
    }

    public enum GameState {
        ERROR = 0,
        TEAM_SELECT = 1,
        JOINING_CHAMP_SELECT = 1 << 1,
        CHAMP_SELECT = 1 << 2,
        PRE_CHAMP_SELECT = 1 << 3 | CHAMP_SELECT,
        POST_CHAMP_SELECT = 1 << 4 | CHAMP_SELECT,
        START_REQUESTED = 1 << 5,
        IN_PROGRESS = 1 << 6,
        TERMINATED = 1 << 7,
        TERMINATED_IN_ERROR = 1 << 8 | TERMINATED,
    }

    public enum PlayerSkill {
        BEGINNER,
        VETERAN,
        RTS_PLAYER
    }

    public class GameMaps {
        public static GameMap SummonersRift = new GameMap {
            Description =
                "The oldest and most venerated Field of Justice is known as Summoner's Rift.  This battleground is known for the constant conflicts fought between two opposing groups of Summoners.  Traverse down one of three different paths in order to attack your enemy at their weakest point.  Work with your allies to siege the enemy base and destroy their Headquarters!",
            MapId = 11,
            DisplayName = "Summoner's Rift",
            MinCustomPlayers = 1,
            Name = "SummonersRift",
            TotalPlayers = 10
        };

        public static GameMap TheCrystalScar = new GameMap {
            Description =
                "The Crystal Scar was once known as the mining village of Kalamanda, until open war between Demacia and Noxus broke out over control of its vast underground riches. Settle your disputes on this Field of Justice by working with your allies to seize capture points and declare dominion over your enemies!",
            MapId = 8,
            DisplayName = "The Crystal Scar",
            MinCustomPlayers = 1,
            Name = "CrystalScar",
            TotalPlayers = 10
        };

        public static GameMap TheTwistedTreeline = new GameMap {
            Description =
                "Deep in the Shadow Isles lies a ruined city shattered by magical disaster. Those who venture inside the ruins and wander through the Twisted Treeline seldom return, but those who do tell tales of horrific creatures and the vengeful dead.",
            MapId = 10,
            DisplayName = "The Twisted Treeline",
            MinCustomPlayers = 1,
            Name = "TwistedTreeline",
            TotalPlayers = 6
        };

        public static GameMap HowlingAbyss = new GameMap {
            Description =
                "The Howling Abyss is a bottomless crevasse located in the coldest, cruelest, part of the Freljord. Legends say that, long ago, a great battle took place here on the narrow bridge spanning this chasm. No one remembers who fought here, or why, but it is said that if you listen carefully to the wind you can still hear the cries of the vanquished tossed howling into the Abyss.",
            MapId = 12,
            DisplayName = "Howling Abyss",
            MinCustomPlayers = 1,
            Name = "HowlingAbyss",
            TotalPlayers = 10
        };

        public static GameMap ButchersBridge = new GameMap {
            MapId = 14,
            DisplayName = "Butcher's Bridge",
            MinCustomPlayers = 1,
            Name = "ButchersBridge",
            TotalPlayers = 10
        };

        //public static readonly Dictionary<GameMap, Uri> Images = new Dictionary<GameMap, Uri> {
        //  [SummonersRift] = new Uri("pack://application:,,,/RiotAPI;component/Resources/SRiftImage.png"),
        //  [TheCrystalScar] = new Uri("pack://application:,,,/RiotAPI;component/Resources/CScarImage.png"),
        //  [TheTwistedTreeline] = new Uri("pack://application:,,,/RiotAPI;component/Resources/TTImage.png"),
        //  [HowlingAbyss] = new Uri("pack://application:,,,/RiotAPI;component/Resources/HAbyssImage.png"),
        //  [ButchersBridge] = new Uri("pack://application:,,,/RiotAPI;component/Resources/BilgewaterImage.png")
        //}; 

        public static readonly GameMap[] Maps = { SummonersRift, ButchersBridge, HowlingAbyss, TheCrystalScar, TheTwistedTreeline };
    }
}
