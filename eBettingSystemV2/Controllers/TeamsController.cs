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
    //public class CountryController
    public class TeamsController : BaseCRUDController<TeamModel, TeamSearchObject, TeamUpsertRequest, TeamUpsertRequest,TeamModelLess>
    {
        public static List<Country> Test = new List<Country>();
        //private ITeamService ITeamService { get; set; }
        private ITeamService ITeamService { get; set; }
        private readonly ILogger<TeamsController> _logger;


        private static readonly string[] Summaries = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

       
        public TeamsController(ITeamService service, ILogger<TeamsController> logger) : base(service)
        {
            ITeamService = service;
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
        public override Task<ActionResult<IEnumerable<TeamModel>>> Get([FromQuery] TeamSearchObject search = null)
        {

            return base.Get(search);
        }


        [HttpPost]
        [Route("InsertTeam")]
        public override async Task<ActionResult<TeamModel>> Insert(TeamUpsertRequest insert)
        {
            var result = await base.Insert(insert);

            try
            {
                  return result;
            }
            catch
            {
                 _logger.LogInformation("CountryId can not be null");
                 return NotFound();
            }
        }


        [HttpPut]
        [Route("UpdateTeam/{Id}")]
        public override IActionResult Update(int Id, [FromBody] TeamUpsertRequest update)
        {
            return base.Update(Id, update); 
        }


        [HttpGet]
        [Route("GetTeamById/{id}")]
        public override Task<ActionResult<TeamModel>> GetById(int id)
        {
            return base.GetById(id);    
        }

        [HttpDelete]
        [Route("DeleteTeamById/{TeamId}")]
        public async Task<ActionResult<TeamModel>> Delete(int TeamId)
        {
            var result = await ITeamService.DeleteAsync(TeamId);

            if (result != null)
            {
                return Ok($"id = {TeamId};Tim je uspješno izbrisan") ;
            }
            else
            {
                return Ok($"Team ne postoji ");
            }
        }


        [HttpGet]
        [Route("GetTeamByCountryId/{CountryId}")]

        public async Task<ActionResult<IEnumerable<TeamModel>>> GetTeamByCountryId(int CountryId)
        {

            var result = await ITeamService.GetbyForeignKeyAsync(CountryId);

            if (result.Count() == 0)
            {

                return BadRequest("Podaci ne postoje");

            }
            else
            {
                return Ok(result);
            
            }

            
        }

        [HttpPut]
        [Route("UpdateTeamTest/{Id}")]
        public IActionResult Update(int Id, [FromBody] JsonPatchDocument update)
        {

            var result = ITeamService.UpdateJson(Id, update);
            return Ok(result);





        }


    }
}
