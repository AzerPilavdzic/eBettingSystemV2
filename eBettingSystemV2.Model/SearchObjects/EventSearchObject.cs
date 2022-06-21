using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class EventSearchObject : BaseSearchObject
    {
        public string EventName { get; set; }
        public int? CompetitionId { get; set; }

    }
}