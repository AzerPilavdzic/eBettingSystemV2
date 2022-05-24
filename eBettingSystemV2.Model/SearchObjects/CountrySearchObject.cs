using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eProdaja.Model.SearchObjects
{
    public class CountrySearchObject : BaseSearchObject
    {
        public string  Naziv { get; set; }
        public int? JediniceMjereId { get; set; }

    }
}