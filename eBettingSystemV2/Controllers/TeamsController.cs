using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.Database;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
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
    public class TeamsController : BaseCRUDController<TeamModel, TeamSearchObject, TeamUpsertRequest, TeamUpsertRequest>
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
        public override IActionResult Insert(TeamUpsertRequest insert)
        {
            try
            {
                  return base.Insert(insert);
            }
            catch
            {
                 _logger.LogInformation("CountryId can not be null");
                 return BadRequest("Unos nije validan");
            }
        }


        [HttpPut]
        [Route("UpdateTeam/{Id}")]
        public override IActionResult Update(int Id, [FromBody] TeamUpsertRequest update)
        {
            return base.Update(Id, update); 
        }


        [HttpGet]
        [Route("GetTeamById/{TeamId}")]

        public override IActionResult GetById(int id)
        {
            return base.GetById(id);    
        }

        [HttpDelete]
        [Route("DeleteTeamById/{TeamId}")]

        public async Task<IActionResult> Delete(int TeamId)
        {
            if (ITeamService.Delete(TeamId) != null)
            {
                return Ok($"Team sa Id {TeamId} je uspjesno obrisan");
            }
            else
            {
                return Ok($"Team ne postoji ");
            }
        }


        [HttpGet]
        [Route("GetTeamByCountryId/{CountryId}")]

        public IEnumerable<TeamModel> GetTeamByCountryId(int CountryId)
        {
            return ITeamService.GetbyForeignKey(CountryId);
        }
    }
}
