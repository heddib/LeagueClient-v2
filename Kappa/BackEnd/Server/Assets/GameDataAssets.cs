using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa.BackEnd.Server.Assets {
    public static class GameDataAssets {
        public static string ChampionSplash(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/champion-splashes/{id / 1000}/{id}.jpg";
        }

        public static string ChampionIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/{id}.png";
        }

        public static string ChromaImage(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/champion-chroma-images/{id / 1000}/{id}.png";
        }

        public static string SplashVideo(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/champion-splash-videos/{id / 1000}/{id}.webm";
        }

        public static string WardSkin(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/ward-skin-images/{id}.png";
        }

        public static string WardSkinShadow(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/ward-skin-images/{id}-shadow.png";
        }

        public static string SummonerSpellIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/summoner-spell-icons/{id}.png";
        }

        public static string ChampionQuote(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/summoner-sfx-audios/{id}.ogg";
        }

        public static string ChampionSfx(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/summoner-vo-audios/{id}.ogg";
        }

        public static string ProfileIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/profile-icons/{id}.jpg";
        }

        public static string MasteryIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/summoner-mastery-icons/{id}.png";
        }

        public static string ItemIcon(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/item-icons/{id}.png";
        }


        public static string ChampionDetails(int id) {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/champions/{id}.json";
        }

        public static string ChampionSummary() {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/champion-summary.json";
        }

        public static string SummonerSpells() {
            return $@"plugins/rcp-be-lol-game-data/global/default/v1/summoner-spells.json";
        }
    }
}
