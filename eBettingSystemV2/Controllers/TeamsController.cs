using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Linq.Interface;
using eBettingSystemV2.Services.NPGSQL.Interface;
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
    //public class CountryController
    public class TeamsController : BaseCRUDController<TeamModel, TeamSearchObject, TeamUpsertRequest, TeamUpsertRequest,TeamModelLess>
    {
        public static List<Country> Test = new List<Country>();
        //private ITeamService ITeamService { get; set; }
        private ITeamService ITeamService { get; set; }
        private ITeamNPGSQL ITeamNPGSQL { get; set; }

        private readonly ILogger<TeamsController> _logger;


        private static readonly string[] Summaries = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

       
        public TeamsController(ITeamService service,ITeamNPGSQL service2,ILogger<TeamsController> logger) : base(service)
        {
            ITeamService = service;
            ITeamNPGSQL  = service2;
           _logger = logger;

        }


        



        //public TeamController(ILogger<CountryController> logger) : 
        //{
        //    _logger = logger;
        //}

        //Implementirati za logger
        //Dodati Patch

        [HttpGet]
        [Route("GetAllTeams")]
        public override async Task<ActionResult<IEnumerable<TeamModel>>> Get([FromQuery] TeamSearchObject search = null)
        {

            if (ITeamNPGSQL.CheckPage0(search))
                return BadRequest("PageNumber ili PageSize ne smiju biti 0");

            if (ITeamNPGSQL.CheckNegative(search))
                return BadRequest("vrijednost ne moze biti negativna");

            //var broj = Service.Get(search).Result.Count();
            try
            {
                var List = await ITeamNPGSQL.GetNPGSQLGeneric(search);

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
            //return base.Get(search);
        }


        [HttpGet]
        [Route("GetTeamById/{id}")]
        public override async Task<ActionResult<TeamModel>> GetById(int id)
        {
            try
            {
                var Model = await ITeamNPGSQL.GetByIdAsync(id);

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
        [Route("GetTeamByCountryId/{CountryId}")]
        public async Task<ActionResult<IEnumerable<TeamModel>>> GetTeamByCountryId(int CountryId)
        {
            try
            {
                var result = await ITeamNPGSQL.GetbyForeignKeyAsync(CountryId);

                if (result.Count() == 0)
                {

                    return BadRequest("Podaci ne postoje");

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


            }




        }


        [HttpPost]
        [Route("InsertTeam")]
        public override async Task<ActionResult<TeamModel>> Insert(TeamUpsertRequest insert)
        {

            //try
            //{
            //    var result = await base.Insert(insert);
            //    return result;
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogInformation("CountryId can not be null");
            //     return BadRequest(ex.Message);
            //}

            try
            {
                var result = await ITeamNPGSQL.InsertAsync(insert);
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
        [Route("UpsertTeam")]
        public override async Task<ActionResult<TeamModelLess>> InsertById(int Id, TeamUpsertRequest Insert)
        {
            try
            {
                var result = await ITeamNPGSQL.UpsertbyIdAsync(Insert, Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

        }


        [HttpPut]
        [Route("UpdateTeam/{Id}")]
        public override Task<ActionResult<TeamModel>> Update(int Id, [FromBody] TeamUpsertRequest update)
        {
            return base.Update(Id, update); 
        }


        [HttpPut]
        [Route("UpdateTeam_!!!DONT_USE_THIS!!!/{Id}")]
        public IActionResult Update(int Id, [FromBody] JsonPatchDocument update)
        {

            var result = ITeamService.UpdateJson(Id, update);
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteTeamById/{TeamId}")]
        public async Task<ActionResult<TeamModel>> Delete(int TeamId)
        {
            var result = await ITeamService.DeleteAsync(TeamId);

            if (result != -1)
            {
                return Ok($"id = {TeamId};Tim je uspješno izbrisan");
            }
            else
            {
                return NotFound($"Team sa {TeamId} ID ne postoji.");
            }
        }

        

    }
}
