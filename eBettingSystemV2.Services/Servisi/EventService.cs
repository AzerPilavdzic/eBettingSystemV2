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
       events,
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
            //ne koristi se.
            var entity = await Context.Events.Where(x => x.event_name.ToLower() == insert.event_name.ToLower()).FirstOrDefaultAsync();
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
                .Where(X => X.event_name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            if (entry == null)
            {

                //throw new Exception($"Podatak sa imenom {name} ne postoji u bazi");
                return new EventModelLess { event_id = 0 };

            }


            return Mapper.Map<EventModelLess>(entry);
        }


        public override IEnumerable<events> AddRange(IEnumerable<EventUpsertRequest> insertlist, DbSet<events> set)
        {
            List<events> Result = new List<events>();
            events CheckEvent = new();

            foreach (var a in insertlist)
            {
                events EventUpdate = Context.Events.Where(X => X.event_name == a.EventName || X.event_id == a.EventId).FirstOrDefault();

                if (EventUpdate == null)
                {
                    //CheckEvent = EventUpdate;
                    EventUpdate = Mapper.Map<events>(a);

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
                    CheckEvent.competition_id = a.CompetitionId;
                    CheckEvent.event_id = EventUpdate.event_id;
                    CheckEvent.event_name = a.EventName;
                    CheckEvent.event_start_time = a.EventStartTime;

                    CheckEvent.event_status = a.EventStatus;

                    CheckEvent.event_period = a.EventPeriod;
                    CheckEvent.result = a.Result;
                    CheckEvent.home_team = a.HomeTeam;
                    CheckEvent.away_team = a.AwayTeam;

                    CheckEvent.red_cards_away_team=a.RedCardsAwayTeam; 
                    CheckEvent.red_cards_home_team=a.RedCardsHomeTeam;

                    CheckEvent.yellow_cards_away_team=a.YellowCardsAwayTeam;
                    CheckEvent.yellow_cards_home_team=a.YellowCardsHomeTeam;

                    CheckEvent = Mapper.Map<events>(a);

                    Result.Add(Mapper.Map<events>(CheckEvent));

                }
            }


            IEnumerable<events> entity = Mapper.Map<IEnumerable<events>>(Result);
            Context.SaveChanges();

            return entity;



        }

        public EventUpsertRequest GetByEventKey(string _eventKey)
        {
            var entry = Context.Events
                .Where(x => x.eventkey == _eventKey)
                .FirstOrDefaultAsync();

            //return Mapper.Map<EventUpsertRequest>(entry);


            //zasto funkcija postoji
            return null;
        }
    }
}
