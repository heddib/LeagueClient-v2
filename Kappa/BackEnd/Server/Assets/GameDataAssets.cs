namespace Kappa.BackEnd.Server.Assets {
    public static class GameDataAssets {
        public const string DefaultLocale = "default";

        public static string ChampionSplash(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-splashes/{id / 1000}/{id}.jpg";
        }

        public static string ChampionTile(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-tiles/{id / 1000}/{id}.jpg";
        }

        public static string ChampionCard(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-cards/{id / 1000}/{id}.jpg";
        }

        public static string ChampionIcon(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-icons/{id}.png";
        }

        public static string ChromaImage(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-chroma-images/{id / 1000}/{id}.png";
        }

        public static string SplashVideo(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-splash-videos/{id / 1000}/{id}.webm";
        }

        public static string ChampionQuote(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-sfx-audios/{id}.ogg";
        }

        public static string ChampionSfx(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-vo-audios/{id}.ogg";
        }


        public static string WardSkin(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/ward-skin-images/{id}.png";
        }

        public static string WardSkinShadow(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/ward-skin-images/{id}-shadow.png";
        }


        public static string SummonerSpellIcon(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/summoner-spell-icons/{id}.png";
        }

        public static string ProfileIcon(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/profile-icons/{id}.jpg";
        }

        public static string MasteryIcon(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/summoner-mastery-icons/{id}.png";
        }

        public static string ItemIcon(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/item-icons/{id}.png";
        }


        public static string ChampionDetails(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champions/{id}.json";
        }


        public static string SettingsToPersist(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/SettingsToPersist.json";
        }

        public static string ChampionSummary(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-summary.json";
        }

        public static string SummonerSpells(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/summoner-spells.json";
        }

        public static string Masteries(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/summoner-masteries.json";
        }

        public static string WardSkins(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/ward-skins.json";
        }

        public static string RuneSlot(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/rune-slot.json";
        }

        public static string Runes(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/runes.json";
        }

        public static string Items(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/items.json";
        }

        public static string Maps(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/maps.json";
        }

        public static string MapAssets(string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/map-assets/map-assets.json";
        }


        public static string MapAsset(string mapKey, string type, string name, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/map-assets/{mapKey}/{type}/{name}";
        }

        public static string SplashMetaData(int id, string locale = DefaultLocale) {
            return $@"plugins/rcp-be-lol-game-data/global/{locale}/v1/champion-splashes/{id}/metadata.json";
        }
    }
}
