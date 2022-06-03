//using eBettingSystemV2.ErrorFilters;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace eBettingSystemV2.Controllers
//{
//    //[ApiController]
//    //[Route("[controller]/api/v{version:apiVersion}/weatherforecast")]
//    //[ApiVersion("1.0", Deprecated = true)] //za testiranje api verzije
//    //[ApiVersion("2.0")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<WeatherForecastController> _logger;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        [MapToApiVersion("1.0")]
//        [NotImplExceptionFilter]
//        public async Task<IEnumerable<WeatherForecast>> GetAPIVersion1()
//        {
//            var rng = new Random();

//            return  Enumerable.Range(1, 5).Select(index => new WeatherForecast
//            {
//                Date = DateTime.Now.AddDays(index),
//                TemperatureC = rng.Next(-200, 5500),
//                Summary = Summaries[rng.Next(Summaries.Length)]
//            })
//            .ToArray();

//        }


//        //this action will be implemented in other folder 
//        [HttpGet("{id}")]
//        [MapToApiVersion("2.0")]
//        [NotImplExceptionFilter]
//        public async Task<IEnumerable<WeatherForecast>> GetAPIVersion2()
//        {
//            var rng = new Random();

//            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//            {
//                Date = DateTime.Now.AddDays(index),
//                TemperatureC = rng.Next(-200, 5500),
//                Summary = Summaries[rng.Next(Summaries.Length)]
//            })
//            .ToArray();

//        }
//    }
//}
