using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class sport
    {
        public sport()
        {
            Competitions = new HashSet<competition>();
            Teams = new HashSet<teams>();
        }

        public int SportsId { get; set; }
        public string name { get; set; }

        public virtual ICollection<competition> Competitions { get; set; }
        public virtual ICollection<teams> Teams { get; set; }
    }
}
