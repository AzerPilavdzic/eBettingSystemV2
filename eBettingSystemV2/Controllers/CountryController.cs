using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.CountryNPGSQL;
using eBettingSystemV2.Services.DataBase;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.Cors;
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
    public class CountryController : BaseCRUDController<CountryModel, CountrySearchObject, CountryInsertRequest, CountryUpsertRequest, CountryModelLess>
    {
        public static List<Country> Test = new List<Country>();
        private ICountryService ICountryService { get; set; }
        private ICountryNPGSQL CountryNPGSQL { get; set;}
        private readonly ILogger<CountryController> _logger;



        private static readonly string[] TestPodaci = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

       
        public CountryController(ICountryService service, ICountryNPGSQL service2,ILogger<CountryController> logger) : base(service)
        {
            ICountryService = service;
            CountryNPGSQL = service2;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("GetAllCountries")]
        public override async Task<ActionResult<IEnumerable<CountryModel>>> Get([FromQuery] CountrySearchObject search = null)
        {
            if (Service.CheckPage0(search))
                return BadRequest("PageNumber ili PageSize ne smiju biti 0");

            if (Service.CheckNegative(search))
                return BadRequest("vrijednost ne moze biti negativna");

            //var broj = Service.Get(search).Result.Count();
            try
            {
                var List = await CountryNPGSQL.GetNPGSQLGeneric(search);

                if (List.Count() == 0)
                    //search.
                    return NotFound("Podaci ne postoje u bazi");
                else
                    return Ok(List);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }

       
        [HttpGet]
        [Route("GetCountryById/{id}")]
        public override async Task<ActionResult<CountryModel>> GetById(int id)
        {
            var Model = await CountryNPGSQL.GetByIdAsync(id);

            if (Model == null)
            {
                return NotFound("Podatak ne postoji u bazi");
            }
            else
            {
                return Ok(Model);
            }
        }

        [HttpGet]
        [Route("GetCountryIdByName/{CountryName}")]
        public async Task<ActionResult<CountryModelLess>> GetCountryIdByName(string CountryName)
        {
            try
            {
                var result = await CountryNPGSQL.GetIdByNameAsync(CountryName);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("InsertCountry")]
        public override async Task<ActionResult<CountryModel>> Insert(CountryInsertRequest insert)
        {
            try
            {
                var result = await CountryNPGSQL.InsertAsync(insert);

                if (result == null)
                {
                    return BadRequest("Ime vec postoji");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("UpsertCountries")]
        public async override Task<ActionResult<CountryModelLess>> InsertById(int Id, CountryInsertRequest Insert)
        {
            try
            {
                var result = await CountryNPGSQL.UpsertbyIdAsync(Insert, Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }


        [HttpPost]
        [Route("UpsertOneOrMoreCountries")]
        public override async Task<ActionResult<IEnumerable<CountryModel>>> UpsertOneOrMore(IEnumerable<CountryUpsertRequest> insertlist)
        {
            try
            {
                var result = await CountryNPGSQL.UpsertOneOrMoreAsync(insertlist);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }




            
        }

        [HttpPost]
        [Route("InsertOneOrMoreCountries")]
        public override async Task<ActionResult<IEnumerable<CountryModel>>> InsertOneOrMore(IEnumerable<CountryInsertRequest> insertlist)
        {
            try
            {
                var result = await CountryNPGSQL.InsertOneOrMoreAsync(insertlist);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("UpdateCountry/{id}")]
        public override async Task<ActionResult<CountryModel>> Update(int id, [FromBody] CountryUpsertRequest update)
        {
            var result = await CountryNPGSQL.UpdateAsync(id, update);
            return Ok(result);
        }

        [HttpDelete]
        //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
        [EnableCors("CorsPolicy")]
        [Route("DeleteCountryById/{CountryId}")]
        public async Task<IActionResult> Delete(int CountryId)
        {
            try
            {
                var result = await CountryNPGSQL.DeleteAsync(CountryId);

                if (result >= 0)
                {
                    return Ok($"Drzava sa Id {CountryId} je uspjesno obrisana");
                }
                else
                {
                    //Console.WriteLine("TESTIRANJE ISPISA U KONZOLI");
                    return BadRequest($"Drzava ne postoji ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }



           
        }
    }
}
