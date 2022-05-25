using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class CountrySearchObject : BaseSearchObject
    {
        public string  Naziv { get; set; }
        public int? CountryId { get; set; }

    }
}