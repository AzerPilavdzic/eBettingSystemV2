using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class Competition
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Countryid { get; set; }
        public int? Sportid { get; set; }

        public virtual Country Country { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual Event Event { get; set; }
    }
}
