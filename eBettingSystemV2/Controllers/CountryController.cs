using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using eProdaja.Model.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController
    //public class CountryController : BaseCRUDController<CountryModel, CountrySearchObject, CountryUpsertRequest, CountryUpsertRequest>
    {
        public static List<Country> Test = new List<Country>();
        private static readonly string[] Summaries = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

        private readonly ILogger<CountryController> _logger;
        //public CountryController(ICountryService service) : base(service)
        //{

        //}


        //public CountryController(ILogger<CountryController> logger) : 
        //{
        //    _logger = logger;
        //}

        [HttpGet]
        public IEnumerable<Country> Get()
        {
            Test.Add(new Country { Country1 = "test1", CountryId = 1 });
            return Test.ToArray();
        }
    }
}
