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
        private readonly ILogger<CompetitionController> _logger;



        private static readonly string[] TestPodaci = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

        private IMemoryCache _cache { get; set; }


        public CompetitionController(ICompetitionService service, ILogger<CompetitionController> logger, IMemoryCache memoryCache) : base(service)
        {
            ICompetitionService = service;
            _logger = logger;
            _cache = memoryCache;
        }

                         

        [HttpGet]
        [Route("GetAllCompetitions")]
        public override Task<ActionResult<IEnumerable<CompetitionModel>>> Get([FromQuery] CompetitionSearchObject search = null)
        {
            //var cacheKey = "competitionList";

            return base.Get(search);
        }

        [HttpGet]
        [Route("GetCompetitionById/{id}")]
        public override Task<ActionResult<CompetitionModel>> GetById(int id)
        {
            return base.GetById(id);
        }

        [HttpGet]
        [Route("GetCompetitionIdbyNaziv/{Naziv}")]
        public async Task<ActionResult<CompetitionModelLess>> GetCompetitionIdbyNaziv(string Naziv)
        {
            try
            {
                var result = await ICompetitionService.GetIdbyNazivAsync(Naziv);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }



        [HttpPost]
        [Route("InsertCompetition")]
        public override Task<ActionResult<CompetitionModel>> Insert(CompetitionInsertRequest insert)
        {
            return base.Insert(insert);
        }


        [HttpPost]
        [Route("UpsertCompetitionById")]
        public override async Task<ActionResult<CompetitionModelLess>> InsertById(int Id, CompetitionInsertRequest Insert)
        {
            try
            {
                var result = await base.InsertById(Id,Insert);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpPost]
        [Route("UpsertOneOrMoreCompetitions")]
        public override async Task<ActionResult<IEnumerable<CompetitionModelLess>>> InsertOneOrMore(IEnumerable<CompetitionUpsertRequest> insertlist)
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


        [HttpPost]
        [Route("InsertCompetitionUsingNamesOnly")]
        public async Task<ActionResult<List<CompetitionModel>>> InsertCompetitionUsingNamesOnly(List<PodaciSaStranice> lista)
        {
            var result = await ICompetitionService.AddDataAsync(lista);
            return result;


        }



        [HttpPut]
        [Route("UpdateCompetitionById")]
        public override Task<ActionResult<CompetitionModel>> Update(int id, [FromBody] CompetitionUpsertRequest update)
        {
            return base.Update(id, update);
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