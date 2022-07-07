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
using eBettingSystemV2.Services.Linq.Servisi;

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

        //public override upsert
        //public override Task<IEnumerable<EventModelLess>> InsertOneOrMoreAsync(IEnumerable<EventUpsertRequest> List)
        //{
        //    return base.InsertOneOrMoreAsync(List);
        //}


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

        public async Task<EventModelLess> GetIdByNameAsync(string name)
        {
            var entry = await Context.Events
                .Where(X => X.EventName.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            if (entry == null)
            {

                //throw new Exception($"Podatak sa imenom {name} ne postoji u bazi");
                return new EventModelLess { EventId = 0 };

            }


            return Mapper.Map<EventModelLess>(entry);
        }


        public override IEnumerable<Event> AddRange(IEnumerable<EventUpsertRequest> insertlist, DbSet<Event> set)
        {
            List<Event> Result = new List<Event>();
            Event CheckEvent = null;

            foreach (var a in insertlist)
            {
                Event EventUpdate = Context.Events.Where(X => X.EventName == a.EventName || X.EventId == a.EventId).FirstOrDefault();

                if (EventUpdate == null)
                {
                    //CheckEvent = EventUpdate;
                    EventUpdate = Mapper.Map<Event>(a);

                    ////dodaj u bazu
                    set.Add(EventUpdate);
                    Context.SaveChanges();

                    Result.Add(CheckEvent);
                    continue;
                }



                else
                {
                    //update
                    CheckEvent = EventUpdate;

                    CheckEvent.EventId = EventUpdate.EventId;
                    CheckEvent.EventName = a.EventName;
                    CheckEvent.EventStartTime = a.EventStartTime;

                    CheckEvent.EventStatus = a.EventStatus;

                    CheckEvent.EventPeriod = a.EventPeriod;
                    CheckEvent.Result = a.Result;
                    CheckEvent.HomeTeam = a.HomeTeam;
                    CheckEvent.AwayTeam = a.AwayTeam;

                    CheckEvent.RedCardsAwayTeam=a.RedCardsAwayTeam; 
                    CheckEvent.RedCardsHomeTeam=a.RedCardsHomeTeam;

                    CheckEvent.YellowCardsAwayTeam=a.YellowCardsAwayTeam;
                    CheckEvent.YellowCardsHomeTeam=a.YellowCardsHomeTeam;

                    //CheckEvent = Mapper.Map<Event>(a);

                    //SA ILI BEZ JEDNAKO?
                    CheckEvent = Mapper.Map<Event>(CheckEvent);
                    Context.SaveChanges();

                    Result.Add(Mapper.Map<Event>(CheckEvent));



                }
            }


            IEnumerable<Event> entity = Mapper.Map<IEnumerable<Event>>(Result);

            Context.SaveChanges();

            return entity;



        }

        public EventUpsertRequest GetByEventKey(string _eventKey)
        {
            var entry = Context.Events
                .Where(x => x.EventKey == _eventKey)
                .FirstOrDefaultAsync();

            //return Mapper.Map<EventUpsertRequest>(entry);


            //zasto funkcija postoji
            return null;
        }
    }
}
