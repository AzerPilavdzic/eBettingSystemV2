﻿using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Models
{
    public partial class TeamModel
    {
        public int teamid { get; set; }
        public string TeamName { get; set; }
        public int? FoundedYear { get; set; }
        public string City { get; set; }
        public int countryid { get; set; }

        public int sportid { get; set; }
    }
}
