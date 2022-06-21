using eBettingSystemV2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Interface
{
    public interface IFetch
    {
        List<PodaciSaStranice> FetchSportAndData();
        List<string> FetchAllSports();

        List<PodaciSaStranice> FetchDataBySport(string sport, bool ispis = true);



    }
}
