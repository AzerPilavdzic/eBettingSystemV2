using eBettingSystemV2.Controllers;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eProdaja.Controllers
{
    public class BaseCRUDController<T, TSearch, TInsert, TUpdate,Tless> : 
        BaseController<T, TSearch,Tless>
        where T : class 
        where TSearch : class 
        where TInsert : class 
        where TUpdate : class
        where Tless:class
    {
        //private ITeamService service;

        public BaseCRUDController(ICRUDService<T, TSearch, TInsert, TUpdate,Tless> service) : base(service)
        { }

        //public BaseCRUDController(ITeamService service)
        //{
        //    this.service = service;
        //}

        [HttpPost]
        public virtual async Task<ActionResult<T>> Insert(TInsert insert)
        {

            var result = await ((ICRUDService<T, TSearch, TInsert, TUpdate,Tless>)this.Service).InsertAsync(insert);


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
            var result = ((ICRUDService<T, TSearch, TInsert, TUpdate,Tless>)this.Service).Update(id, update);
            return Ok(result);
        }

       

    }
}