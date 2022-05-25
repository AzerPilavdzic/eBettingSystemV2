using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.Database;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
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
    //public class CountryController
    public class CountryController : BaseCRUDController<CountryModel, CountrySearchObject, CountryUpsertRequest, CountryUpsertRequest>
    {
        public static List<Country> Test = new List<Country>();
        private ICountryService ICountryService { get; set; }
        private static readonly string[] Summaries = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

        private readonly ILogger<CountryController> _logger;
        public CountryController(ICountryService service) : base(service)
        {
            ICountryService = service;
        }

        //public CountryController(ILogger<CountryController> logger) : 
        //{
        //    _logger = logger;
        //}

        //Implementirati za logger
        //Dodati Patch


        //[HttpGet]
        //public IEnumerable<CountryModel> GetCountry([FromQuery] CountrySearchObject search = null)
        //{
        //    //return base.Get(search);
        //    //

        //    return ICountryService.Get(search);

        //}



    }
}
