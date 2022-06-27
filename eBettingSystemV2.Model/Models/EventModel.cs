using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Models
{
    public partial class EventModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int ?CompetitionId { get; set; }
        public DateTime? EventStartTime { get; set; }
        public string EventStatus { get; set; }
        public string EventPeriod { get; set; }
        public string Result { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? YellowCardsHomeTeam { get; set; }
        public int? YellowCardsAwayTeam { get; set; }
        public int? RedCardsHomeTeam { get; set; }
        public int? RedCardsAwayTeam { get; set; }
    }
}