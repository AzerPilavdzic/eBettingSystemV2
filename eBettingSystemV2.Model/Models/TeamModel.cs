using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Models
{
    public partial class TeamModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int? FoundedYear { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}
