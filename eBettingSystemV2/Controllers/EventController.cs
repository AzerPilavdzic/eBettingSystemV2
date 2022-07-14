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
using eBettingSystemV2.Services.NPGSQL.Interface;

namespace eBettingSystemV2.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EventController : BaseCRUDNPGSQLController<EventModel, EventSearchObject, EventInsertRequest, EventUpsertRequest, EventModelLess>
    {
        //private IEventService IEventService { get; set; }
        private IEventsNPGSQL IEventService { get; set; }
        
        private readonly ILogger<EventController> _logger;

        public EventController(IEventsNPGSQL service, ILogger<EventController> logger) : base(service)
        {
            IEventService = service;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetAllEvents")]
        public override async Task<ActionResult<IEnumerable<EventModel>>> Get([FromQuery] EventSearchObject search = null)
        {
            try
            {
                var List = await IEventService.GetNPGSQLGeneric(search);
                //var List = await ICountryService.Get(search);

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
        }


        [HttpGet]
        [Route("GetEventById/{id}")]
        public override async Task<ActionResult<EventModel>> GetById(int id)
        {
            try { 
            var Model = await IEventService.GetByIdAsync(id);
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
        [Route("GetEventIdByName/{EventName}")]
        public async Task<ActionResult<EventModelLess>> GetEventIdByName(string EventName)
        {
            try
            {
                var result = await IEventService.GetIdByNameAsync(EventName);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        [HttpPost]
        [Route("InsertEvent")]
        public override async Task<ActionResult<EventModel>> Insert(EventInsertRequest insert)
        {
            try
            {
                var result = await IEventService.InsertAsync(insert);
                if (result == null)
                    return BadRequest("Ime vec postoji");
                else
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
        [Route("UpsertEvents")]
        public override async Task<ActionResult<EventModelLess>> InsertById(int Id, EventInsertRequest Insert)
        {
            try
            {
                var result = await IEventService.UpsertbyIdAsync(Insert, Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("UpsertOneOrMoreEvents")]
        public override async Task<ActionResult<IEnumerable<EventModel>>> UpsertOneOrMore(IEnumerable<EventUpsertRequest> insertlist)
        {
            try
            {
                var result = await base.UpsertOneOrMore(insertlist);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }





        }



        [HttpPut]
        [Route("UpdateEvent/{id}")]
        public override async Task<ActionResult<EventModel>> Update(int id, [FromBody] EventInsertRequest update)
        {
            try
            {
                var result = await IEventService.UpdateAsync(id, update);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
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
