using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class teams
    {
        public int teamid { get; set; }
        public string teamname { get; set; }
        public int? foundedyear { get; set; }
        public string city { get; set; }
        public int countryid { get; set; }
        public int? sportid { get; set; }

        public virtual Country Country { get; set; }
        public virtual Sport Sport { get; set; }
    }
}
