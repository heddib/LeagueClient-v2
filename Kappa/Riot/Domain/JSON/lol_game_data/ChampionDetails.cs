using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class ChampionDetails : ChampionSummary {
        [JSONField("title")]
        public string Title { get; set; }

        [JSONField("shortBio")]
        public string ShortBio { get; set; }

        [JSONField("tacticalInfo")]
        public TacticalInfo TacticalInfo { get; set; }

        [JSONField("playstyleInfo")]
        public PlaystyleInfo PlaystyleInfo { get; set; }

        [JSONField("squarePath")]
        public string SquarePath { get; set; }

        [JSONField("portraitPath")]
        public string PortraitPath { get; set; }

        [JSONField("skins")]
        public SkinDetails[] Skins { get; set; }

        [JSONField("passive")]
        public SpellDetails Passive { get; set; }

        [JSONField("spells")]
        public SpellDetails[] Spells { get; set; }
    }

    public class TacticalInfo : JSONSerializable {
        [JSONField("style")]
        public int Style { get; set; }

        [JSONField("difficulty")]
        public int Difficulty { get; set; }

        [JSONField("damageType")]
        public string DamageType { get; set; }
    }

    public class PlaystyleInfo : JSONSerializable {
        [JSONField("damage")]
        public int Damage { get; set; }

        [JSONField("durability")]
        public int Durability { get; set; }

        [JSONField("crowdControl")]
        public int CrowdControl { get; set; }

        [JSONField("mobility")]
        public int Mobility { get; set; }

        [JSONField("utility")]
        public int Utility { get; set; }
    }

    public class SpellDetails : JSONSerializable {
        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("description")]
        public string Description { get; set; }
    }

    public class SkinDetails : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("splashPath")]
        public string SplashPath { get; set; }

        [JSONField("tilePath")]
        public string TilePath { get; set; }

        [JSONField("cardPath")]
        public string CardPath { get; set; }

        [JSONField("splashVideoPath")]
        public string SplashVideoPath { get; set; }

        [JSONField("chromaPath")]
        public string ChromaPath { get; set; }

        [JSONField("chromas")]
        public ChromaDetails[] Chromas { get; set; }
    }

    public class ChromaDetails : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("chromaPath")]
        public string ChromaPath { get; set; }

        [JSONField("cardPath")]
        public string CardPath { get; set; }

        [JSONField("colors")]
        public string[] Colors { get; set; }
    }
}
