using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{

    
    public class FetchCacheInsertService:IFetchCacheInsert
    {

        private IFetch IFetchService { get; set; }
        private ICache ICacheService { get; set; }


        public FetchCacheInsertService(IFetch Service1,ICache Service2)
        {

            IFetchService = Service1;
            ICacheService = Service2;
        
        
        }

        public async Task FetchStoreCacheCompetition()
        {


            List<PodaciSaStranice> Lista = IFetchService.FetchSportAndData();



            var Lista2 = await ICacheService.SetCacheCompetition(Lista);




        }







    }
}
