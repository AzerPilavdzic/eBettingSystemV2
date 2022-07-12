using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Linq.Interface;
using eBettingSystemV2.Services.NPGSQL.Interface;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //public class CountryController
    public class CountryController : BaseCRUDNPGSQLController<CountryModel, CountrySearchObject, CountryInsertRequest, CountryUpsertRequest, CountryModelLess>
    {
        public static List<Country> Test = new List<Country>();
        private ICountryNPGSQL ICountryService { get; set; }
        //private ICountryService ICountryService { get; set; }

        private readonly ILogger<CountryController> _logger;

        private static readonly string[] TestPodaci = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };


        public CountryController(ICountryNPGSQL service, ILogger<CountryController> logger) : base(service)
        {
            ICountryService = service;
            _logger = logger;
        }

        public override async Task<ActionResult<IEnumerable<CountryModel>>> Get([FromQuery] CountrySearchObject search = null)
        {
            try
            {
                var List = await ICountryService.GetNPGSQLGeneric(search);
                //var List = await ICountryService.Get(search);

                if (List.Count() == 0)
                    //search.
                    return NotFound("Podaci ne postoje u bazi");
                else
                    return Ok(List);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCountryById/{id}")]
        public override async Task<ActionResult<CountryModel>> GetById(int id)
        {
            try
            {
                var Model = await ICountryService.GetByIdAsync(id);

                if (Model == null)
                {
                    return NotFound("Podatak ne postoji u bazi");
                }
                else
                {
                    return Ok(Model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);


            }
        }

        //[HttpGet]
        //[Route("GetCountryById/{id}")]
        //public override async Task<ActionResult<CountryModel>> GetById(int id)
        //{
        //    try
        //    {
        //        var Model = await ICountryService.GetByIdAsync(id);

        //        if (Model == null)
        //            return NotFound("Podatak ne postoji u bazi");
        //        else
        //            return Ok(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message);
        //        return BadRequest(ex.Message);


        //    }
        //}

        [HttpGet]
        [Route("GetCountryIdByName/{CountryName}")]
        public async Task<ActionResult<CountryModelLess>> GetCountryIdByName(string CountryName)
        {
            try
            {
                var result = await ICountryService.GetIdByNameAsync(CountryName);
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
                var result = await ICountryService.InsertAsync(insert);
                if (result == null)
                    return BadRequest("Ime vec postoji");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
                throw;
            }
        }


        [HttpPost]
        [Route("UpsertCountry")]
        public override async Task<ActionResult<CountryModelLess>> InsertById(int Id, CountryInsertRequest Insert)
        {
            try
            {
                var result = await ICountryService.UpsertbyIdAsync(Insert, Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        //public async override Task<ActionResult<CountryModelLess>> InsertById(int Id, CountryInsertRequest Insert)
        //{
        //    try
        //    {
        //        var result = await CountryNPGSQL.UpsertbyIdAsync(Insert, Id);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message);
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost]
        [Route("UpsertOneOrMoreCountries")]
        public override async Task<ActionResult<IEnumerable<CountryModel>>> UpsertOneOrMore(IEnumerable<CountryUpsertRequest> insertlist)
        {
            try
            {
                var result = await ICountryService.UpsertOneOrMoreAsync(insertlist);
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
                var result = await ICountryService.InsertOneOrMoreAsync(insertlist);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("UpdateCountry/")]
        public override async Task<ActionResult<CountryModel>> Update(int id, [FromBody] CountryUpsertRequest update)
        {
            try
            {
                var result = await ICountryService.UpdateAsync(id, update);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
        [EnableCors("CorsPolicy")]
        [Route("DeleteCountryById/{CountryId}")]
        public async Task<IActionResult> Delete(int CountryId)
        {
            try
            {
                var result = await ICountryService.DeleteAsync(CountryId);

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
