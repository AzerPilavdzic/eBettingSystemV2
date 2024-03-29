﻿using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using eBettingSystemV2.Services.Linq.Interface;
using eBettingSystemV2.Services.NPGSQL.Interface;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    public class CompetitionController : BaseCRUDController<CompetitionModel, CompetitionSearchObject, CompetitionInsertRequest, CompetitionUpsertRequest, CompetitionModelLess>
    {
        //public static List<Country> Test = new List<Country>();
        private ICompetitionService ICompetitionService { get; set; }
        private ICompetitionNPGSQL ICompetitionNPGSQL { get; set; }

        private readonly ILogger<CompetitionController> _logger;



        private static readonly string[] TestPodaci = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

        private IMemoryCache _cache { get; set; }


        public CompetitionController(ICompetitionService service,
            ILogger<CompetitionController> logger,
            IMemoryCache memoryCache,
            ICompetitionNPGSQL service2
            ) : base(service)
        {
            ICompetitionService = service;
            ICompetitionNPGSQL = service2;
            _logger = logger;
            _cache = memoryCache;
        }

                         

        [HttpGet]
        [Route("GetAllCompetitions")]
        public override async Task<ActionResult<IEnumerable<CompetitionModel>>> Get([FromQuery] CompetitionSearchObject search = null)
        {
            try
            {
                var result = await ICompetitionNPGSQL.GetNPGSQLGeneric(search);

                return Ok(result);
               
                //return base.Get(search);


            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
               
            }




            
        }

        [HttpGet]
        [Route("GetCompetitionById/{id}")]
        public override async Task<ActionResult<CompetitionModel>> GetById(int id)
        {
            //return base.GetById(id);

            try
            {
                var result = await ICompetitionNPGSQL.GetByIdAsync(id);

                return Ok(result);

                //return base.Get(search);


            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);

            }

        }

        [HttpGet]
        [Route("GetCompetitionIdbyNaziv/{Naziv}")]
        public async Task<ActionResult<CompetitionModelLess>> GetCompetitionIdbyNaziv(string Naziv)
        {
            try
            {
                var result = ICompetitionNPGSQL.GetIdbyNazivAsync(Naziv);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }



        }



        [HttpPost]
        [Route("InsertCompetition")]
        public override async Task<ActionResult<CompetitionModel>> Insert(CompetitionInsertRequest insert)
        {
            try
            {
                var result = await ICompetitionNPGSQL.InsertAsync(insert);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("UpsertCompetitionById")]
        public override async Task<ActionResult<CompetitionModelLess>> InsertById(int Id, CompetitionInsertRequest Insert)
        {
            try
            {
                //var result = await base.InsertById(Id,Insert);
                var result = await ICompetitionNPGSQL.UpsertbyIdAsync(Insert,Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

            
        }


        [HttpPost]
        [Route("InsertOneOrMoreCompetitions")]
        public override async  Task<ActionResult<IEnumerable<CompetitionModel>>> InsertOneOrMore(IEnumerable<CompetitionInsertRequest> insertlist)
        {
            try
            {
                var result = await ICompetitionNPGSQL.InsertOneOrMoreAsync(insertlist);
                return Ok(result);

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            //return base.InsertOneOrMore(insertlist);
        }

        [HttpPost]
        [Route("UpsertOneOrMoreCompetitions")]
        public override async Task<ActionResult<IEnumerable<CompetitionModel>>> UpsertOneOrMore(IEnumerable<CompetitionUpsertRequest> insertlist)
        {
            try
            {
                //var result = await base.UpsertOneOrMore(insertlist);
                var result = await ICompetitionNPGSQL.UpsertOneOrMoreAsync(insertlist);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

           
        }


        [HttpPost]
        [Route("InsertCompetitionUsingNamesOnly")]
        public async Task<ActionResult<List<CompetitionModel>>> InsertCompetitionUsingNamesOnly(List<PodaciSaStranice> lista)
        {
            //koristi se linq ovdje

            var result = await ICompetitionService.AddDataAsync(lista);
            return result;


        }



        [HttpPut]
        [Route("UpdateCompetitionById")]
        public override async Task<ActionResult<CompetitionModel>> Update(int id, [FromBody] CompetitionInsertRequest update)
        {
            try
            {
                var result = await ICompetitionNPGSQL.UpdateAsync(id,update);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }


            //return base.Update(id, update);
        }

        [HttpDelete]
        [Route("DeleteCompetitionById/{CompId}")]
        public async Task<ActionResult<CompetitionModel>> Delete(int CompId)
        {
            var result = await ICompetitionService.DeleteAsync(CompId);

            if (result != -1)
            {
                return Ok($"id = {CompId};Competition je uspješno izbrisan");
            }
            else
            {
                return NotFound($"Competition sa {CompId} ID ne postoji.");
            }
        }


    }
}