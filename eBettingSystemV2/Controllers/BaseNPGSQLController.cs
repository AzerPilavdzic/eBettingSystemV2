
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.Linq.Interface;
using eBettingSystemV2.Services.NPGSQL.Interface;
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
    //[Authorize]
    public class BaseNPGSQLController<T, TSearch, Tless> : ControllerBase
        where T : class
        where TSearch : class
        where Tless : class
    {
        public IBaseNPGSQL<T, TSearch, Tless> Service { get; set; }


        public BaseNPGSQLController(IBaseNPGSQL<T, TSearch, Tless> service)
        {
            Service = service;
        }


        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get([FromQuery] TSearch search = null)
        {
            if (Service.CheckPage0(search))
                return BadRequest("PageNumber ili PageSize ne smiju biti 0");

            if (Service.CheckNegative(search))
                return BadRequest("vrijednost ne moze biti negativna");

            //var broj = Service.Get(search).Result.Count();
            try
            {
                var List = await Service.GetNPGSQLGeneric(search);

                if (List.Count() == 0)
                    //search.
                    return NotFound("Podaci ne postoje u bazi");
                else
                    return Ok(List);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public virtual async Task<ActionResult<T>> GetById(int id)
        {
            var Model = await Service.GetByIdAsync(id);

            if (Model == null)
            {
                return NotFound("Podatak ne postoji u bazi");
            }
            else
            {
                return Ok(Model);
            }
        }
    }
}
