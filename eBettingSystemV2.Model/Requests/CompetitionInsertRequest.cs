using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Model.SearchObjects

{

    //imena moraju biti ista kao kolone u bazi
    public class CompetitionInsertRequest
    {
        public string naziv { get; set; }
        public int sportid { get; set; }
        public int countryid { get; set; }
    }
}
