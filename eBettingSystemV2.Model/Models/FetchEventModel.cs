using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Model.Models
{
    public class FetchEventModel
    {
        public string CompetitionName { get; set; }

        List<Podaci> _eventi { get; set; }

        public class Podaci
        {
            public string LinkId { get; set; }
            public string EventName { get; set; }
            public string Result { get; set; }

        }
    }
}
