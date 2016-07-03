using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON.lol_game_data {
    public class ItemDetails : JSONSerializable {
        [JSONField("id")]
        public int Id { get; set; }

        [JSONField("name")]
        public string Name { get; set; }

        [JSONField("description")]
        public string Description { get; set; }

        [JSONField("price")]
        public int Price { get; set; }

        [JSONField("priceTotal")]
        public int PriceTotal { get; set; }
    }
}
