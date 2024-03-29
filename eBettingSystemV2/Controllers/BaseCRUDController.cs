﻿using eBettingSystemV2.Controllers;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.Linq.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eProdaja.Controllers
{
    public class BaseCRUDController<T, TSearch, TInsert, TUpdate, Tless> :
        BaseController<T, TSearch, Tless>
        where T : class
        where TSearch : class
        where TInsert : class
        where TUpdate : class
        where Tless : class
    {
        private ITeamService service;

        public BaseCRUDController(ICRUDService<T, TSearch, TInsert, TUpdate, Tless> service) : base(service)
        { }

      



        //public BaseCRUDController(ITeamService service)
        //{
        //    this.service = service;
        //}

        [HttpPost]
        //[Route("BASE3")]
        public virtual async Task<ActionResult<T>> Insert(TInsert insert)
        {

            try
            {
                var result = await ((ICRUDService<T, TSearch, TInsert, TUpdate, Tless>)this.Service).InsertAsync(insert);

                if (result == null)
                {
                    return BadRequest("Ime vec postoji");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }


        }

        [HttpPost]
        //[Route("BASE2")]
        public virtual async Task<ActionResult<IEnumerable<T>>> UpsertOneOrMore(IEnumerable<TUpdate> insertlist)
        {
            var result = await ((ICRUDService<T, TSearch, TInsert, TUpdate, Tless>)this.Service).UpsertOneOrMoreAsync(insertlist);



            if (result == null)
            {
                return BadRequest("Null");

            }
            else
            {

                return Ok(result);

            }



        }

        [HttpPost]
        [Route("2")]
        public virtual async Task<ActionResult<IEnumerable<T>>> InsertOneOrMore(IEnumerable<TInsert> insertlist)
        {
            //var result = await ((ICRUDService<T, TSearch, TInsert, TUpdate, Tless>)this.Service).InsertOneOrMoreAsync(insertlist);



            //if (result == null)
            //{
            //    return BadRequest("Null");

            //}
            //else
            //{

            return Ok();

            //}



        }



        [HttpPut("{id}")]
        public virtual async Task<ActionResult<T>> Update(int id, [FromBody] TInsert update)
        {
            var result = await ((ICRUDService<T, TSearch, TInsert, TUpdate, Tless>)this.Service).UpdateAsync(id, update);
            return Ok(result);
        }

        [HttpPost("{Id}")]
        //[Route("BASE1")]
        //treba ovdje insert request
        public virtual async Task<ActionResult<Tless>> InsertById(int Id, TInsert Insert)
        {

            var result = await ((ICRUDService<T, TSearch, TInsert, TUpdate, Tless>)this.Service).InsertById(Insert, Id);

            return Ok(result);

        }

        
    }
}