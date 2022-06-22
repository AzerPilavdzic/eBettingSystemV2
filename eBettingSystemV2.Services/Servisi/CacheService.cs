using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Interface;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{
    public class CacheService:ICache
    {
        private IMemoryCache _cache;
        //private ICompetitionService ICompetitionService { get; set; }

        private ILogCompetition ILogCompetitionService { get; set;}


        public CacheService(IMemoryCache memoryCache /*,ICompetitionService service1*/,ILogCompetition service2)
        {

            _cache = memoryCache;

            //ICompetitionService = service1;
            ILogCompetitionService = service2;



        }


        public  async Task<List<PodaciSaStranice>> SetCacheCompetition(List<PodaciSaStranice> podaciSaStranices, Func<Task<List<CompetitionModel>>> AddDataAsync)
        {

            //look for cache that expires in 1 day

            string text = null;

            if (!_cache.TryGetValue(Models.CacheKeys.Expire, out text))
            {
                text = "Expire";

                var cacheEntryOptions2 = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromDays(1)
                };

                _cache.Set(Models.CacheKeys.Expire, "Expire", cacheEntryOptions2);

                //var result = await ICompetitionService.AddDataAsync(podaciSaStranices);
                List<CompetitionModel> result =  AddDataAsync.Invoke().Result;

                ILogCompetitionService.AddEntry("Competition 1 day Update", DateTime.Now,result.Count);


                return podaciSaStranices;


            }
            else //samo dodaj u cache
            {

                List<PodaciSaStranice> podaci2 = null;

                podaci2 = podaciSaStranices;


                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(5),
                    SlidingExpiration = TimeSpan.FromDays(5)
                };

                _cache.Set(Models.CacheKeys.Podaci, podaci2, cacheEntryOptions);

                var cacheEntry = _cache.Get<List<PodaciSaStranice>>(Models.CacheKeys.Podaci);

                return cacheEntry;


            }
















          















        }

        //public 







    }
}
