using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.Database;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.JsonPatch;
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
    public class SportController : BaseCRUDController<SportModel, SportSearchObject, SportUpsertRequest, SportUpsertRequest, SportModelLess>
    {
        public static List<Country> Test = new List<Country>();

        private ISportService ISportService { get; set; }
        private readonly ILogger<SportController> _logger;


        private static readonly string[] Summaries = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };


        public SportController(ISportService service, ILogger<SportController> logger) : base(service)
        {
            ISportService = service;
            _logger = logger;

        }


        [HttpGet]
        [Route("GetAllSport")]
        public override Task<ActionResult<IEnumerable<SportModel>>> Get([FromQuery] SportSearchObject search = null)
        {

            return base.Get(search);


        }


        [HttpGet]
        [Route("GetSportById")]
        public override Task<ActionResult<SportModel>> GetById(int id)
        {
            return base.GetById(id);

        }
      


        [HttpPost]
        [Route("InsertSport")]
        public override async Task<ActionResult<SportModel>> Insert(SportUpsertRequest insert)
        {

            try
            {
                var result = await base.Insert(insert);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }







        

        [HttpDelete]
        [Route("DeleteSportById/{SportsId}")]
        public async Task<IActionResult> Delete(int SportsId)
        {
            var result = await ISportService.DeleteAsync(SportsId);

            if (result != null)
            {
                return Ok($"Sport sa Id {SportsId} je uspjesno obrisan");
            }
            else
            {
                return Ok($"Drzava ne postoji ");
            }
        }




        [HttpGet]
        [Route("GetSportById/{id}")]
        public override Task<ActionResult<SportModel>> GetById(int id)
        {
            return base.GetById(id);
        }


        [HttpPut]
        [Route("UpdateSport/{id}")]
        public override Task<ActionResult<SportModel>> Update(int id, [FromBody] SportUpsertRequest update)

        {
            return base.Update(id, update);
        }



        [HttpPost]
        [Route("AddoneormoreSports")]
        public override Task<ActionResult<IEnumerable<SportModelLess>>> InsertOneOrMore(IEnumerable<SportUpsertRequest> insertlist)
        {
            return base.InsertOneOrMore(insertlist);
        }

        [HttpPost]
        [Route("AddSportById")]
        public override Task<ActionResult<SportModelLess>> InsertById(int Id, SportUpsertRequest Insert)
        {
            return base.InsertById(Id, Insert);
        }


             





    }
}
