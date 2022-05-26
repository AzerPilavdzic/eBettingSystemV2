
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
        public virtual IActionResult Get([FromQuery] TSearch search = null)
        {

            if (Service.Get(search) == null)
            {

                return NotFound("Podaci ne postoje u bazi");

            }
            else
            {

                return Ok(Service.Get(search));

            }



           
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            return Ok(Service.GetById(id));
        }
    }
}
