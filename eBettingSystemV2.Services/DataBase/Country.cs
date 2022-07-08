using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class Country
    {
        public Country()
        {
            Competitions = new HashSet<Competition>();
            Teams = new HashSet<teams>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
        public virtual ICollection<teams> Teams { get; set; }
    }
}
