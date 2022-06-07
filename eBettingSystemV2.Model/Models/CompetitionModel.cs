using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Models
{
    public class CompetitionModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Sportid { get; set; }
        public int Countryid { get; set; }
    }
}
