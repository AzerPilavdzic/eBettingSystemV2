using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Model.SearchObjects

{
    public class CompetitionInsertRequest
    {
       public string Naziv { get; set; }
        public int Sportid { get; set; }
        public int Countryid { get; set; }
    }
}
