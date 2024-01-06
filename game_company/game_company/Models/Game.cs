using System;
using System.Collections.Generic;
using System.Text;

namespace game_company.Models
{
    public  class Game
    {
        public int game_id { get; set; }
        public int cat_id { get; set; }
        public int dev_id { get; set; }
        public string game_name { get; set; }
        public DateTime game_date_release { get; set; }
        public string game_code_unique { get; set; }
        public double game_size { get; set; }
        public double game_price { get; set; }
    }
}
