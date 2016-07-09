using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class WardSkinSummary : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("description")]
        public string Description { get; set; }

        [JSONField("wardImagePath")]
        public string WardImagePath { get; set; }

        [JSONField("wardShadowImagePath")]
        public string WardShadowImagePath { get; set; }
    }
}
