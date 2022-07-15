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
            Events = new HashSet<events>();
        }

        public int id { get; set; }
        public string naziv { get; set; }
        public int countryid { get; set; }
        public int? sportid { get; set; }

        public virtual Country Country { get; set; }
        public virtual sport Sport { get; set; }
        public virtual ICollection<events> Events { get; set; }
    }
}
