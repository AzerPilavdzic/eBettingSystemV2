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
        private readonly ILogger<TeamsController> _logger;



        private static readonly string[] TestPodaci = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

       
        public CountryController(ICountryService service,ILogger<TeamsController> logger) : base(service)
        {
            ICountryService = service;
            _logger = logger;
        }

        //public CountryController(ILogger<CountryController> logger) : 
        //{
        //    _logger = logger;
        //}

        //Implementirati za logger
        //Dodati Patch



        [HttpGet]
        [Route("GetAllCountries")]
        public override IEnumerable<CountryModel> Get([FromQuery] CountrySearchObject search = null)
        {
            


            return base.Get(search);
        }

        [HttpPost]
        [Route("InsertCountry")]
        public override CountryModel Insert(CountryUpsertRequest insert)
        {
            return base.Insert(insert);
        }

        [HttpGet]
        [Route("GetCountryById/{id}")]
        public override CountryModel GetById(int id)
        {
            return base.GetById(id);
        }




        [HttpPut]
        [Route("UpdateCountry/{id}")]
        public override CountryModel Update(int id, [FromBody] CountryUpsertRequest update)
        {
            return base.Update(id, update);
        }

    }
}
