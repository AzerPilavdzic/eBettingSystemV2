using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eProdaja.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eBettingSystemV2.Services.Interface;

namespace eBettingSystemV2.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EventController : BaseCRUDController<EventModel, EventSearchObject, EventInsertRequest, EventUpsertRequest, EventModelLess>
    {
        private IEventService IEventService { get; set; }
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService service, ILogger<EventController> logger) : base(service)
        {
            IEventService = service;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetAllEvents")]
        public override Task<ActionResult<IEnumerable<EventModel>>> Get([FromQuery] EventSearchObject search = null)
        {

            return base.Get(search);
        }


        [HttpGet]
        [Route("GetEventById/{id}")]
        public override Task<ActionResult<EventModel>> GetById(int id)
        {
            return base.GetById(id);
        }

        [HttpGet]
        [Route("GetEventIdByName/{EventName}")]
        public async Task<ActionResult<EventModelLess>> GetEventIdByName(string EventName)
        {
            return BadRequest("Action is not implemented.");

        }


        [HttpPost]
        [Route("InsertEvent")]
        public override Task<ActionResult<EventModel>> Insert(EventInsertRequest insert)
        {
            return base.Insert(insert);
        }

        [HttpPost]
        [Route("UpsertEvents")]
        public override Task<ActionResult<EventModelLess>> InsertById(int Id, EventInsertRequest Insert)
        {
            return base.InsertById(Id, Insert);
        }


        [HttpPost]
        [Route("UpsertOneOrMoreEvents")]
        public override async Task<ActionResult<IEnumerable<EventModelLess>>> InsertOneOrMore(IEnumerable<EventUpsertRequest> insertlist)
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



        [HttpPut]
        [Route("UpdateEvent/{id}")]
        public override Task<ActionResult<EventModel>> Update(int id, [FromBody] EventUpsertRequest update)
        {
            return base.Update(id, update);
        }

        [HttpDelete]
        [Route("DeleteEvent")]
        public async Task<IActionResult> Delete(int EventId)
        {
            try
            {
                var result = await IEventService.DeleteAsync(EventId);

                if (result != 0)
                {
                    return Ok($"Event sa Id {EventId} je uspjesno izbrisan.");
                }
                else
                {
                    //Console.WriteLine("TESTIRANJE ISPISA U KONZOLI");
                    return BadRequest($"Event sa ID {EventId} ne postoji.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
