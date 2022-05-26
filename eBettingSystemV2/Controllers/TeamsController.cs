﻿using eBettingSystemV2.Model.SearchObjects;
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
        public override IActionResult Get([FromQuery] TeamSearchObject search = null)
        {
            return base.Get(search);
        }


        [HttpPost]
        [Route("InsertTeam")]
        public override IActionResult Insert(TeamUpsertRequest insert)
        {

            if (insert.Countryid == 0)
            {

                return BadRequest("Unos nije validan");


            }
            else
            {
                try
                {


                    return base.Insert(insert);
                }
                catch
                {
                    if (insert.Countryid == 0)
                    {


                        _logger.LogInformation("CountryId can not be null");
                    }


                    return null;


                }




            }





           

            //return base.Insert(insert)






            
        }


        [HttpPut]
        [Route("UpdateTeam/{Id}")]
        public override IActionResult Update(int id, [FromBody] TeamUpsertRequest update)
        {
            return base.Update(id, update); 
        }


        [HttpGet]
        [Route("GetTeam/{Id}")]

        public override IActionResult GetById(int id)
        {
            return base.GetById(id);    
        }

        [HttpDelete]
        [Route("DeleteTeam/{TeamId}")]

        public IActionResult Delete(int TeamId)
        {
            


           return Ok(ITeamService.Delete(TeamId));
            
        
        
        }


        [HttpGet]
        [Route("GetTeamByCountryId/{CountryId}")]

        public IEnumerable<TeamModel> GetTeamByCountryId(int CountryId)
        {

            return ITeamService.GetbyForeignKey(CountryId);
        
        
        }





    }
}
