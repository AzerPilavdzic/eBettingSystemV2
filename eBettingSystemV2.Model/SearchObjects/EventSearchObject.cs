using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class EventSearchObject : BaseSearchObject
    {
        public int? EventId { get; set; }
        public string EventName { get; set; }
        public int? CompetitionId { get; set; }

    }
}