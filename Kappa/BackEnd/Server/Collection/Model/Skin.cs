using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Collection.Model {
    public class Skin : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("selected")]
        public bool Selected { get; set; }

        public Skin(ChampionSkinDTO skin) {
            Id = skin.SkinId;
            Selected = skin.LastSelected;
        }
    }
}
