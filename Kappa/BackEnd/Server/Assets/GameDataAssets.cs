using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.BackEnd.Server.Assets {
    public static class GameDataAssets {
        public static string Locale { get; set; } = "default";

        public static string ChampionSplash(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-splashes/{id / 1000}/{id}.jpg";
        }

        public static string ChampionTile(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-tiles/{id / 1000}/{id}.jpg";
        }

        public static string ChampionCard(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-cards/{id / 1000}/{id}.jpg";
        }

        public static string ChampionIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-icons/{id}.png";
        }

        public static string ChromaImage(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-chroma-images/{id / 1000}/{id}.png";
        }

        public static string SplashVideo(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-splash-videos/{id / 1000}/{id}.webm";
        }

        public static string ChampionQuote(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-sfx-audios/{id}.ogg";
        }

        public static string ChampionSfx(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-vo-audios/{id}.ogg";
        }


        public static string WardSkin(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/ward-skin-images/{id}.png";
        }

        public static string WardSkinShadow(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/ward-skin-images/{id}-shadow.png";
        }


        public static string SummonerSpellIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/summoner-spell-icons/{id}.png";
        }

        public static string ProfileIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/profile-icons/{id}.jpg";
        }

        public static string MasteryIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/summoner-mastery-icons/{id}.png";
        }

        public static string ItemIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/item-icons/{id}.png";
        }


        public static string ChampionDetails(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champions/{id}.json";
        }

        public static string ChampionSummary => $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/champion-summary.json";
        public static string SummonerSpells => $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/summoner-spells.json";
        public static string Masteries => $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/summoner-masteries.json";
        public static string RuneSlot => $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/rune-slot.json";
        public static string Runes => $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/runes.json";
        public static string Items => $@"plugins/rcp-be-lol-game-data/global/{Locale}/v1/items.json";
    }
}
