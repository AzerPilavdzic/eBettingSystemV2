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
        private static readonly string[] Summaries = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };

        private readonly ILogger<TeamsController> _logger;
        public TeamsController(ITeamService service) : base(service)
        {
            ITeamService = service;
        }

        //public TeamController(ILogger<CountryController> logger) : 
        //{
        //    _logger = logger;
        //}

        //Implementirati za logger
        //Dodati Patch

        [HttpGet]
        [Route("GetAllTeams")]
        public override IEnumerable<TeamModel> Get([FromQuery] TeamSearchObject search = null)
        {
            return base.Get(search);
        }


        [HttpPost]
        [Route("InsertTeam")]
        public override TeamModel Insert(TeamUpsertRequest insert)
        {
            return base.Insert(insert); 
        }


        [HttpPut]
        [Route("UpdateTeam/{Id}")]
        public override TeamModel Update(int id, [FromBody] TeamUpsertRequest update)
        {
            return base.Update(id, update); 
        }


        [HttpGet]
        [Route("GetTeam/{Id}")]

        public override TeamModel GetById(int id)
        {
            return base.GetById(id);    
        }

        [HttpDelete]
        [Route("DeleteTeam/{Id}")]

        public TeamModel Delete(int TeamId)
        {

           return ITeamService.Delete(TeamId);
            
        
        
        }



    }
}
