﻿using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
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
        public override Task<ActionResult<IEnumerable<CountryModel>>> Get([FromQuery] CountrySearchObject search = null)
        {

            return base.Get(search);
        }

       
        [HttpGet]
        [Route("GetCountryById/{id}")]
        public override Task<ActionResult<CountryModel>> GetById(int id)
        {
            return base.GetById(id);
        }

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
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("InsertCountry")]
        public override Task<ActionResult<CountryModel>> Insert(CountryInsertRequest insert)
        {
            return base.Insert(insert);
        }

        [HttpPost]
        [Route("UpsertCountries")]
        public override Task<ActionResult<CountryModelLess>> InsertById(int Id, CountryInsertRequest Insert)
        {
            return base.InsertById(Id, Insert);
        }


        [HttpPost]
        [Route("UpsertOneOrMoreCountries")]
        public override async Task<ActionResult<IEnumerable<CountryModelLess>>> InsertOneOrMore(IEnumerable<CountryUpsertRequest> insertlist)
        {
            try
            {
                var result = await base.InsertOneOrMore(insertlist);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




            
        }



        [HttpPut]
        [Route("UpdateCountry/{id}")]
        public override Task<ActionResult<CountryModel>> Update(int id, [FromBody] CountryUpsertRequest update)
        {
            return base.Update(id, update);
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

                if (result != 0)
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
                return BadRequest(ex.Message);
            }



           
        }
    }
}
