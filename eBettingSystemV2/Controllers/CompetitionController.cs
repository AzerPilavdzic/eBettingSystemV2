using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.Cors;
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
    public class CompetitionController : BaseCRUDController<CompetitionModel, CompetitionSearchObject, CompetitionInsertRequest, CompetitionUpsertRequest, CompetitionModelLess>
    {
        //public static List<Country> Test = new List<Country>();
        private ICompetitionService ICompetitionService { get; set; }
        private readonly ILogger<CompetitionController> _logger;



        private static readonly string[] TestPodaci = new[]
        {
            "BIH", "CRO", "SLO", "SRB"
        };



        public CompetitionController(ICompetitionService service, ILogger<CompetitionController> logger) : base(service)
        {
            ICompetitionService = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("InsertCompetition")]
        public override Task<ActionResult<CompetitionModel>> Insert(CompetitionInsertRequest insert)
        {
            return base.Insert(insert);
        }           

        [HttpGet]
        [Route("GetAllCompetitions")]
        public override Task<ActionResult<IEnumerable<CompetitionModel>>> Get([FromQuery] CompetitionSearchObject search = null)
        {
            return base.Get(search);
        }

        [HttpPost]
        [Route("UpsertCompetitionById")]
        public override Task<ActionResult<CompetitionModelLess>> InsertById(int Id, CompetitionInsertRequest Insert)
        {
            return base.InsertById(Id, Insert);
        }

        [HttpPut]
        [Route("UpdateCompetitionById")]
        public override Task<ActionResult<CompetitionModel>> Update(int id, [FromBody] CompetitionUpsertRequest update)
        {
            return base.Update(id, update);
        }

        [HttpPost]
        [Route("UpsertOneOrMoreCompetitions")]
        public override Task<ActionResult<IEnumerable<CompetitionModelLess>>> InsertOneOrMore(IEnumerable<CompetitionUpsertRequest> insertlist)
        {
            return base.InsertOneOrMore(insertlist);
        }

        [HttpGet]
        [Route("GetCompetitionById/{id}")]
        public override Task<ActionResult<CompetitionModel>> GetById(int id)
        {
            return base.GetById(id);
        }



    }
}