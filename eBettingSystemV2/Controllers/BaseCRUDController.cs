using eBettingSystemV2.Controllers;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eProdaja.Controllers
{
    public class BaseCRUDController<T, TSearch, TInsert, TUpdate> : BaseController<T, TSearch>
        where T : class where TSearch : class where TInsert : class where TUpdate : class
    {
        //private ITeamService service;

        public BaseCRUDController(ICRUDService<T, TSearch, TInsert, TUpdate> service) : base(service)
        { }

        //public BaseCRUDController(ITeamService service)
        //{
        //    this.service = service;
        //}

        [HttpPost]
        public virtual IActionResult Insert(TInsert insert)
        {

            var result = ((ICRUDService<T, TSearch, TInsert, TUpdate>)this.Service).Insert(insert);

            if (result == null)
            {

                return BadRequest("Ime vec postoji");


            }
            else
            {

                return Ok(result);
            }




          
        }

        [HttpPut("{id}")]
        public virtual IActionResult Update(int id, [FromBody] TUpdate update)
        {
            var result = ((ICRUDService<T, TSearch, TInsert, TUpdate>)this.Service).Update(id, update);
            return Ok(result);
        }

       

    }
}