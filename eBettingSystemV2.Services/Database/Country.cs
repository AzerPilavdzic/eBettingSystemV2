using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.Database
{
    public partial class Country
    {
        public Country()
        {
            Teams = new HashSet<Team>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
