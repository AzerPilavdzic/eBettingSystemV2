using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RezultatiImporter.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //public class CountryController
    public class DemoController:Controller
    {

        private IDemo IDemoService { get; set; }
        private IMemoryCache _cache { get; set;}

       

        public DemoController(IDemo service, IMemoryCache memoryCache)
        {
            IDemoService = service;
            _cache = memoryCache;
        }


       
     

        [HttpPost]
        [Route("InsertCompetitionUsingNamesOnly")]
        public async Task<ActionResult<List<CompetitionModel>>> InsertCompetitionUsingNamesOnly(List<PodaciSaStranice> lista)
        {
            var result = await IDemoService.AddDataAsync(lista);


            return result;
                  
          
        }

        [HttpGet]
        public string CacheTryGetValueSet()
        {
            DateTime? cacheEntry=null;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            string uslov = cacheEntry == null ? "No cached entry found" : cacheEntry.Value.TimeOfDay.ToString();


            string Text = $"Current Time: {cacheEntry.Value.ToString()} \n" +
                          $"Cache  Time :{uslov} \n";
                


                //$"Current Time: \n"+
                //   $"Cached Time:" +
                //   cacheEntry == null ? "No cached entry found" : cacheEntry.Value.TimeOfDay.ToString() + "\n";



            return Text;



                      



        }







    }
}
