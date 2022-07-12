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
    public class SportsNPGSQLService :     
        BCrudNPGSQLService
        <
        SportModel,
        sport,
        SportSearchObject,
        SportInsertRequest,
        SportUpsertRequest,
        SportModelLess
        >      
        ,ISportsNPGSQL
    {
        public SportsNPGSQLService(IConfiguration Service1, IMapper Service3)
        : base(Service1,Service3) {

            PrimaryKey = $@"""SportsId""";           
            var list = typeof(SportModel).GetProperties();

            foreach (var a in list)
            {
                if (a.Name != "SportsId")
                {
                    var text = a.Name.Any(char.IsUpper) ? $@"""{a.Name}""" : a.Name;
                    ListaAtributa.Add(text);                               
                }         
            }                         
        }


        //Get Funkcije
        public async Task<SportModelLess> GetIdByNameAsync(string name)
        {

            string Query = null;
            string typeParameterType = typeof(sport).Name;
            Query += $@"select *  from ""BettingSystem"".""{typeParameterType}"" ";
            Query += $@"where {GetAtribute1()} = '{name}'; ";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var quary = await conn.QueryAsync<SportModelLess>(Query);
            var entity = quary.FirstOrDefault();

            if(entity==null)
            {
                return new SportModelLess { SportsId = 0 };

            }

            conn.Close();
            return entity;
        
        }




        // Get Ekstenzije
        public override string AddFilter(string query, SportSearchObject search = null)
        {
                  
            if (!string.IsNullOrWhiteSpace(search?.SportName))
            {
                query += $@"where (lower(""name"") LIKE lower('%{search.SportName}%')) ";
                          
            }
            if (search.SportId != null && string.IsNullOrWhiteSpace(search?.SportName))
            {
                query += $@"where {PrimaryKey} = {search.SportId} ";

            }

            if (search.SportId != null && !string.IsNullOrWhiteSpace(search?.SportName))
            {
                query += $@"or {PrimaryKey} = {search.SportId} ";

            }
                   

            return query;


        }

        //Insert extensions

        //query ekstenzije
        public override string GetCoalesce(SportUpsertRequest Update)
        {
            //ako je 0 to je null ,ako je string to je null

            return $@"
               {PrimaryKey}=coalesce({Update.SportsId},""BettingSystem"".""sport"".{PrimaryKey}),
               {GetAtribute1()}=coalesce('{Update.name}',""BettingSystem"".""sport"".{GetAtribute1()})
                    ";


        }
        public override string GetAtribute1()
        {
            return $@"""name""";
        }
        public override string GetValue1(SportInsertRequest insert)
        {
            return $@"'{insert.name}'";
        }
        public override string GetValuesAll(SportInsertRequest insert, int id)
        {
            return $@"{id},'{insert.name}'";
        }
        public override string GetValuesAll(SportUpsertRequest insert)
        {
            return $@"{insert.SportsId},'{insert.name}'";
        }
        public override string GetValuesAllBesidesPrimary(SportInsertRequest Insert)
        {
            return $@"'{Insert.name}'";
        }


        //insert esktenzije
        public override bool BeforeInsertBool(SportInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""sport"" 
             where (lower(""name"") = lower('{insert.name}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                return true;
            }
            else
            {

                return false;
            
            }
        }



        //Upsert Extenzije
        public override bool checkIfNameSame(SportInsertRequest insert, sport entry)
        {
            if (insert.name== entry?.name)
            {

                return true;


            }
            return false;
        }

        public override List<SportUpsertRequest> BeforeInsertFilterList(IEnumerable<SportUpsertRequest> List)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();
            var Query = "";
            List<SportUpsertRequest> OutputList = new List<SportUpsertRequest>();

            foreach (var item in List)
            {
                //ako korisnik nije unjeo id               
                Query = $@"Select * From ""BettingSystem"".""sport"" 
                        Where {GetAtribute1()} ='{item.name}'";

                var entity = conn.Query<SportUpsertRequest>(Query).FirstOrDefault();
                
                if (entity != null)
                {
                    continue;
                
                }

                OutputList.Add(item);
            }

           

            return OutputList;






        }
        public override void BeforeInsertVoid(SportInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""sport"" 
             where (lower(""name"") = lower('{insert.name}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: SPORT SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();
           
        }
        public override void BeforeInsertVoid(SportUpsertRequest Update)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""sport"" 
             where (lower(""name"") = lower('{Update.name}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: SPORT SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();
        }
        public override void BeforeDelete(int id)
        {

            string Query = null;
            string Query2 = null;
            string TableName  = "teams";
            string TableName2 = "competition";
            string foreignkey = "SportId";

            Query  += $@"Select * from ""BettingSystem"".{TableName}";
            Query2 += $@"Select * from ""BettingSystem"".{TableName2}";
            Query  += $@" Where {foreignkey} = {id}";
            Query2 += $@" Where {foreignkey} = {id}";
           
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var entry = conn.QueryFirstOrDefault<teams>(Query);
            var dalipostojicompetition = conn.QueryFirstOrDefault<Competition>(Query2);

            conn.Close();

            if (entry != null)
            {

                throw new Exception("EXCEPTION SportsNPGSQLService line 281");


            }

            if (dalipostojicompetition != null)
            {

                throw new Exception("EXCEPTION SportsNPGSQLService line 289");

            }
        }
     
    }
}


