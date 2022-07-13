using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{

    //naziv mora biti kao u bazi
    public partial class competition
    {
        public competition()
        {
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Countryid { get; set; }
        public int? Sportid { get; set; }

        public virtual Country Country { get; set; }
        public virtual sport Sport { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
