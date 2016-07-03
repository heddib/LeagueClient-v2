using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    [JSONSerializable]
    public class Skin {
        public int Id { get; set; }
        public bool Selected { get; set; }

        public Skin(ChampionSkinDTO skin) {
            Id = skin.SkinId;
            Selected = skin.LastSelected;
        }
    }
}
