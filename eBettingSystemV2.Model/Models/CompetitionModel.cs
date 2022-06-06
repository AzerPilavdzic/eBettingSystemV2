using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Models
{
    public class CompetitionModel
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public int sportid { get; set; }
        public int countryid { get; set; }
    }
}
