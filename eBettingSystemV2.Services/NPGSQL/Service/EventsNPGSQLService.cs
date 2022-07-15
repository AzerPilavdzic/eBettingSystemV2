//using AutoMapper;
//using eBettingSystemV2.Model;
//using RS2_Tourism_Agency.Model.Request;
//using RS2_Tourism_Agency.Model.SearchObjects;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBettingSystemV2.Model.Models;
//using eBettingSystemV2.Services.Interface;
using Npgsql;
using Microsoft.Data.SqlClient;
using Dapper;
using NUnit.Framework;
using eBettingSystemV2.Services.NPGSQL.Service;
using eBettingSystemV2.Services.NPGSQL.Interface;
//using TourismAgency.Services.Database;

namespace eBettingSystemV2.Services.NPGSQL.Service
{
    public class EventsNPGSQLService :     
        BCrudNPGSQLService
        <
        EventModel,
        events,
        EventSearchObject,
        EventInsertRequest,
        EventUpsertRequest,
        EventModelLess

        >      
        ,IEventsNPGSQL
    {
        public EventsNPGSQLService(IConfiguration Service1, IMapper Service3)
        : base(Service1,Service3) {


            PrimaryKey = $@"""event_id""";
            Conflictinsert = PrimaryKey;
            ConflictUpsert = PrimaryKey;
            
            var list = typeof(EventModel).GetProperties();

            foreach (var a in list)
            {
                if (a.Name != "EventsId")
                {
                    var text = a.Name.Any(char.IsUpper) ? $@"""{a.Name}""" : a.Name;
                    ListaAtributa.Add(text);                               
                }         
            }                         
        }


        //Get Funkcije
        public async Task<EventModelLess> GetIdByNameAsync(string name)
        {

            string Query = null;
            string typeParameterType = typeof(events).Name;
            Query += $@"select *  from ""BettingSystem"".""{typeParameterType}"" ";
            Query += $@"where {GetAtribute1()} = '{name}'; ";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var quary = await conn.QueryAsync<EventModelLess>(Query);
            var entity = quary.FirstOrDefault();

            if(entity==null)
            {
                return new EventModelLess { event_id = 0 };

            }

            conn.Close();
            return entity;
        
        }




        // Get Ekstenzije
        public override string AddFilter(string query, EventSearchObject search = null)
        {
            if (!string.IsNullOrWhiteSpace(search?.EventName))
            {
                query += $@"where (lower(""name"") LIKE lower('%{search.EventName}%')) ";

            }
            if (search.EventId != null && string.IsNullOrWhiteSpace(search?.EventName))
            {
                query += $@"where {PrimaryKey} = {search.EventId} ";

            }


            if (search.EventId != null && !string.IsNullOrWhiteSpace(search?.EventName))
            {
                query += $@"or {PrimaryKey} = {search.EventId} ";

            }


            return query;
        }

      
       



        //Insert extensions

        public override string GetAtribute1()
        {
            return $@"""event_name""";
        }

       

                  
       



        //insert esktenzije
        public override bool BeforeInsertBool(EventInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""event"" 
             where (lower(""event_name"") = lower('{insert.event_name}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
                return true;
            else
                return false;
        }



        //Upsert Extenzije
        public override bool checkIfNameSame(EventInsertRequest insert, events entry)
        {
            if (insert.event_name == entry?.event_name)
            {

                return true;


            }
            return false;
        }

        public override List<EventUpsertRequest> BeforeInsertFilterList(IEnumerable<EventUpsertRequest> List)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();
            var Query = "";
            List<EventUpsertRequest> OutputList = new List<EventUpsertRequest>();

            foreach (var item in List)
            {
                //ako korisnik nije unjeo id               
                Query = $@"Select * From ""BettingSystem"".""Event"" 
                        Where {GetAtribute1()} ='{item.EventName}'";

                var entity = conn.Query<EventUpsertRequest>(Query).FirstOrDefault();
                
                if (entity != null)
                {
                    continue;
                
                }

                OutputList.Add(item);
            }

           

            return OutputList;






        }

        public override void BeforeInsertVoid(EventUpsertRequest Update)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Event"" 
             where (lower(""name"") = lower('{Update.EventName}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: Event SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();
        }
        public override void BeforeDelete(int id)
        {

            string Query = null;
            string Query2 = null;
            string TableName  = "teams";
            string TableName2 = "competition";
            string foreignkey = "EventId";

            Query  += $@"Select * from ""BettingSystem"".{TableName}";
            Query2 += $@"Select * from ""BettingSystem"".{TableName2}";
            Query  += $@" Where {foreignkey} = {id}";
            Query2 += $@" Where {foreignkey} = {id}";
           
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var entry = conn.QueryFirstOrDefault<teams>(Query);
            var dalipostojicompetition = conn.QueryFirstOrDefault<competition>(Query2);

            conn.Close();

            if (entry != null)
            {

                throw new Exception("EXCEPTION EventsNPGSQLService line 281");


            }

            if (dalipostojicompetition != null)
            {

                throw new Exception("EXCEPTION EventsNPGSQLService line 289");

            }
        }

        public EventUpsertRequest GetByEventKey(string eventKey)
        {
            throw new NotImplementedException();
        }
    }
}


