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


        public CacheService(IMemoryCache memoryCache)
        {

            _cache = memoryCache;





        }


        public  List<PodaciSaStranice> SetCache(List<PodaciSaStranice> podaciSaStranices)
        {
            List<PodaciSaStranice> podaci2 = null;


            // Look for cache key.
            if (!_cache.TryGetValue(podaciSaStranices[0].Competitionname, out podaci2))
            {
                podaci2 = podaciSaStranices;


                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(6),
                    SlidingExpiration = TimeSpan.FromSeconds(6)
                };


                // Save data in cache and set the relative expiration time to one day



                _cache.Set(Models.CacheKeys.Podaci, podaci2, cacheEntryOptions);

                //var result = await IDemoService.AddDataAsync(podaci2);

                return podaci2;

            }
            else
            {


                //uporediti sa stranicom


                var cacheEntry =  _cache.Get<List<PodaciSaStranice>>(Models.CacheKeys.Podaci);


                //cacheEntry[0].Competitionname = "cache";


                return cacheEntry;

            }





        }


    }
}
