using AutoMapper;
using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{
    public class EventService : BaseCRUDService
       <
       EventModel,
       Event,
       EventSearchObject,
       EventInsertRequest,
       EventUpsertRequest,
       EventModelLess
       >,
       IEventService
    {

        public EventService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;

        }

        public async Task<EventModelLess> BeforeInsertBoolAsync(EventInsertRequest insert)
        {
            var entity = await Context.Events.Where(x => x.EventName.ToLower() == insert.EventName.ToLower()).FirstOrDefaultAsync();
            if (entity == null)
            {
                return null;
            }
            throw new Exception("EXCEPTION: EVENT SA TIM IMENOM VEC POSTOJI.");
        }

        public override Task<EventModelLess> InsertAsync(EventInsertRequest insert)
        {
            if (!BeforeInsertBool(insert))
            {
                throw new Exception("Event sa tim imenom vec postoji.");
            }
            return base.InsertAsync(insert);
        }

        public override EventModelLess Insert(EventInsertRequest insert)
        {
            //Context = new praksa_dbContext();
            return base.Insert(insert);
        }

    }
}
