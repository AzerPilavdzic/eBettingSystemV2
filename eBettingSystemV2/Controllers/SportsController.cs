using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eBettingSystemV2.Services.Linq.Interface;
using eBettingSystemV2.Services.NPGSQL.Interface;

namespace eBettingSystemV2.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SportController : BaseCRUDController<SportModel, SportSearchObject, SportInsertRequest, SportUpsertRequest, SportModelLess>
    {
        public static List<Country> Test = new List<Country>();

        private ISportService ISportService { get; set; }

        public ISportsNPGSQL ISportsNPGSQL { get; set; }


        private readonly ILogger<SportController> _logger;


        public SportController(ISportService service, ISportsNPGSQL service2, ILogger<SportController> logger) : base(service)
        {
            ISportService = service;
            ISportsNPGSQL = service2;
            _logger = logger;

        }


        [HttpGet]
        [Route("GetAllSport")]
        public override Task<ActionResult<IEnumerable<SportModel>>> Get([FromQuery] SportSearchObject search = null)
        {

            return base.Get(search);


        }

        [HttpGet]
        [Route("GetSportById/{id}")]
        public override Task<ActionResult<SportModel>> GetById(int id)
        {
            return base.GetById(id);
        }

        [HttpGet]
        [Route("GetSportIdByName/{name}")]
        public async Task<ActionResult<SportModelLess>> GetSportIdByName(string name)
        {
            try
            {
                var result = await ISportService.GetSportIdbyNameAsync(name);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }





        [HttpPost]
        [Route("InsertSport")]
        public override async Task<ActionResult<SportModel>> Insert(SportInsertRequest insert)
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

        [HttpPost]
        [Route("UpsertOneOrMoreSports")]
        public override Task<ActionResult<IEnumerable<SportModel>>> UpsertOneOrMore(IEnumerable<SportUpsertRequest> insertlist)
        {
            return base.UpsertOneOrMore(insertlist);
        }


        [HttpPost]
        [Route("UpsertSports")]
        public override Task<ActionResult<SportModelLess>> InsertById(int Id, SportInsertRequest Insert)
        {
            return base.InsertById(Id, Insert);
        }

        [HttpDelete]
        [Route("DeleteSportById/{SportsId}")]
        public async Task<IActionResult> Delete(int SportsId)
        {

            try
            {
                var result = await ISportService.DeleteAsync(SportsId);

                if (result != SportsId)
                {
                    return BadRequest($"Drzava ne postoji ");
                }
                else
                {
                    return Ok($"Sport sa Id {SportsId} je uspjesno obrisan");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




           

          
        }


        [HttpPut]
        [Route("UpdateSport/{id}")]
        public override Task<ActionResult<SportModel>> Update(int id, [FromBody] SportInsertRequest update)

        {
            return base.Update(id, update);
        }












    }
}
