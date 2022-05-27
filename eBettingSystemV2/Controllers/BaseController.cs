
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Services;
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
    public class BaseController<T, TSearch> : ControllerBase where T : class where TSearch : class
    {
        public IService<T, TSearch> Service { get; set; }
       

        public BaseController(IService<T, TSearch> service)
        {
            Service = service;
        }

        [HttpGet]
        public virtual  IActionResult Get([FromQuery] TSearch search = null)
        {
            if (Service.CheckPage0(search))
            {

                return BadRequest("PageNumber ili PageSize ne smiju biti 0");
            
            }
           


            try
            {

                if (Service.Get(search).Count() == 0)
                {

                    //search.

                    return NotFound("Podaci ne postoje u bazi");

                }
                else
                {

                    return Ok(Service.Get(search));

                }
            }
            catch
            {


                return BadRequest("vrijednost ne moze biti negativna"); 
            
            
            }


           
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            if (Service.GetById(id) == null)
            {

                return NotFound("Podatak ne postoji u bazi");

            }
            else
            {
                return Ok(Service.GetById(id));


            }


          
        }
    }
}
