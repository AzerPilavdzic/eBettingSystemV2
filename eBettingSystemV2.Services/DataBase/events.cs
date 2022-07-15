using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class events
    {
        public int event_id { get; set; }
        public string event_name { get; set; }
        public int? competition_id { get; set; }
        public DateTime? event_start_time { get; set; }
        public string event_status { get; set; }
        public string event_period { get; set; }
        public string result { get; set; }
        public string home_team { get; set; }
        public string away_team { get; set; }
        public int? yellow_cards_home_team { get; set; }
        public int? yellow_cards_away_team { get; set; }
        public int? red_cards_home_team { get; set; }
        public int? red_cards_away_team { get; set; }
        public string eventkey { get; set; }

        public virtual competition Competition { get; set; }
    }
}
