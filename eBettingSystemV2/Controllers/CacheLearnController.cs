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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace eBettingSystemV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //public class CountryController
    public class CacheLearn:Controller
    {

        private IMemoryCache _cache;
        private IDemo IDemoService { get; set; }



        public CacheLearn(IDemo service, IMemoryCache memoryCache)
        {
            IDemoService = service;
            _cache = memoryCache;
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
                          $"Cache  Time : {uslov} \n";
                


                //$"Current Time: \n"+
                //   $"Cached Time:" +
                //   cacheEntry == null ? "No cached entry found" : cacheEntry.Value.TimeOfDay.ToString() + "\n";



            return Text;



                      



        }

        [HttpGet]
        [Route("SetCacheRelativeExpiration")]
        public string SetCacheRelativeExpiration()
        {
            DateTime? cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Save data in cache and set the relative expiration time to one day
                _cache.Set(CacheKeys.Entry, cacheEntry, TimeSpan.FromDays(1));
            }

           

            string uslov = cacheEntry == null ? "No cached entry found" : cacheEntry.Value.ToString();

            string Text = $"Current Time: {DateTime.Now.ToString()} \n" +
                           $"Cache  Time : {uslov} \n"; 
                       


            return Text;

        }


        //uzima cache ili ga pravi cache and current ntime trebaju da budu isti
        [HttpGet]
        [Route("CacheGetOrCreate")]
        public string CacheGetOrCreate()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return DateTime.Now;
            });

            //string uslov = cacheEntry == null ? "No cached entry found" : cacheEntry.Value.ToString();

            string Text =  $"Current Time: {DateTime.Now.ToString()} \n" +
                           $"Cache  Time : {cacheEntry.ToString()} \n"  +
                           $"Trebaju da budu isti";


            return Text;
        }

        [HttpGet]
        [Route("CacheGetOrCreate async")]
        public async Task<string> CacheGetOrCreateAsynchronous()
        {

            var cacheEntry = await
                _cache.GetOrCreateAsync(CacheKeys.Entry, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                    return Task.FromResult(DateTime.Now);
                });

            string Text = $"Current Time: {DateTime.Now.ToString()} \n" +
                          $"Cache  Time : {cacheEntry.ToString()} \n" +
                          $"Trebaju da budu isti";

            return Text;
        }

        [HttpGet]
        [Route("CacheGet")]
        public string CacheGet()
        {
            var cacheEntry = _cache.Get<DateTime?>(CacheKeys.Entry);
            
            string Text = $"Current Time: {DateTime.Now.ToString()} \n" +
                          $"Cache  Time : {cacheEntry.Value.ToString()} \n" +
                          $"ne trebaju da budu isti";

            return Text;
        }


        [HttpGet]
        [Route("CacheGetOrCreateAbs")]
        public string CacheGetOrCreateAbs()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return DateTime.Now;
            });

            string Text = $"Current Time: {DateTime.Now.ToString()} \n" +
                         $"Cache  Time : {cacheEntry.ToString()} \n" +
                         $"ne trebaju da budu isti";

            return Text;
        }


        [HttpGet]
        [Route("CacheGetOrCreateAbsSliding")]
        public string CacheGetOrCreateAbsSliding()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(3));
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return DateTime.Now;
            });

            string Text = $"Current Time: {DateTime.Now.ToString()} \n" +
                           $"Cache  Time : {cacheEntry.ToString()} \n" +
                           $"ne trebaju da budu isti";
            return Text;
        }


        [HttpPost]
        [Route("TakeDataFromCache")]
        public async Task<List<CompetitionModel>> TakeDataFromCache (List<PodaciSaStranice> podaciSaStranices)
        {
            
            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Podaci, out podaciSaStranices))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromDays(1)
                };

                // Save data in cache and set the relative expiration time to one day
                _cache.Set(CacheKeys.Podaci, podaciSaStranices, cacheEntryOptions);

                var result = await IDemoService.AddDataAsync(podaciSaStranices);
                return result;

            }
            else
            {
                //uporediti sa stranicom

                return null;

            }
            //return Text;
        }



    }
}
