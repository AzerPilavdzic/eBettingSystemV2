using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class TeamUpsertRequest
    {
        public string TeamName { get; set; }
        public string City { get; set; }

        public int Countryid { get; set; }

        public int? Foundedyear { get; set; }
    }
}