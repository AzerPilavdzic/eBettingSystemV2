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

        public ISportsNPGSQL SportsNPGSQL { get; set; }


        private readonly ILogger<SportController> _logger;


        public SportController(ISportService service, ISportsNPGSQL service2, ILogger<SportController> logger) : base(service)
        {
            ISportService = service;
            SportsNPGSQL = service2;
            _logger = logger;

        }


        [HttpGet]
        [Route("GetAllSport")]
        public override async Task<ActionResult<IEnumerable<SportModel>>> Get([FromQuery] SportSearchObject search = null)
        {
            try
            {

                //CHECK PAGE
                //CHECK NEGATIVE


                var List = await SportsNPGSQL.GetNPGSQLGeneric(search);

                if (List.Count() == 0)
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
        [Route("GetSportById/{id}")]
        public override async Task<ActionResult<SportModel>> GetById(int id)
        {
            try
            {
                var Model = await SportsNPGSQL.GetByIdAsync(id);

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

        [HttpGet]
        [Route("GetSportIdByName/{name}")]
        public async Task<ActionResult<SportModelLess>> GetSportIdByName(string name)
        {
            try
            {
                var result = await SportsNPGSQL.GetIdByNameAsync(name);
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
                var result = await SportsNPGSQL.InsertAsync(insert);

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
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
                throw;
            }

        }

        [HttpPost]
        [Route("UpsertOneOrMoreSports")]
        public override async Task<ActionResult<IEnumerable<SportModel>>> UpsertOneOrMore(IEnumerable<SportUpsertRequest> insertlist)
        {
            try
            {
                var result = await SportsNPGSQL.UpsertOneOrMoreAsync(insertlist);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("UpsertSport")]
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
                var result = await SportsNPGSQL.DeleteAsync(SportsId);

                if (result != SportsId)
                {
                    return BadRequest($"Sport ne postoji ");
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

        public override async Task<ActionResult<SportModel>> Update(int id, [FromBody] SportInsertRequest update)
        {
            var result = await SportsNPGSQL.UpdateAsync(id, update);
            return Ok(result);
        }












    }
}
