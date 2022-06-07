using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Model.SearchObjects
{
    public class CompetitionUpsertRequest
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public int sportid { get; set; }
        public int countryid { get; set; }

    }
}
