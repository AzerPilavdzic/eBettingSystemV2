﻿using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class CountryUpsertRequest
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

    }
}