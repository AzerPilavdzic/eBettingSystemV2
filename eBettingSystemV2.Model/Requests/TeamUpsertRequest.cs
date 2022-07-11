using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBettingSystemV2.Model.SearchObjects
{

    //imena atributa moraju biti ista kao u bazi

    public class TeamUpsertRequest
    {

        public int teamid { get; set; }
        public string teamname { get; set; }
        public string city { get; set; }

        public int countryid { get; set; }

        public int? foundedyear { get; set; }
    }
}