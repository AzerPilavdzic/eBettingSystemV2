using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class EventUpsertRequest
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        //public int ?CompetitionId { get; set; }
        public DateTime EventStartTime { get; set; }
        public string EventStatus { get; set; }
        public string EventPeriod { get; set; }
        public string Result { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? YellowCardsHomeTeam { get; set; }
        public int? YellowCardsAwayTeam { get; set; }
        public int? RedCardsHomeTeam { get; set; }
        public int? RedCardsAwayTeam { get; set; }
        public string EventKey { get; set; }

    }
}
