using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.Database
{
    public partial class Team
    {
        public int Teamid { get; set; }
        public string Teamname { get; set; }
        public int? Foundedyear { get; set; }
        public string City { get; set; }
        public int Countryid { get; set; }

        public virtual Country Country { get; set; }
    }
}
