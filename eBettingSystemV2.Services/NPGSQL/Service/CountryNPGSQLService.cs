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
    public class CountryNPGSQLService :     
        BCrudNPGSQLService
        <
        CountryModel,
        Country,
        CountrySearchObject,
        CountryInsertRequest,
        CountryUpsertRequest,
        CountryModelLess
        
        >      
        ,ICountryNPGSQL
    {
        public CountryNPGSQLService(IConfiguration Service1, IMapper Service3)
        : base(Service1,Service3) {

            PrimaryKey = $@"""CountryId""";
            Conflictinsert = $@"""CountryName""";
            ConflictUpsert = $@"""CountryId""";
            exception = "imena koja su navedena vec postoje u bazi";

                           
        }


        //Get Funkcije
        public async Task<CountryModelLess> GetIdByNameAsync(string name)
        {

            string Query = null;
            string typeParameterType = typeof(Country).Name;
            Query += $@"select *  from ""BettingSystem"".""{typeParameterType}"" ";
            Query += $@"where {GetAtribute1()} = '{name}'; ";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var quary = await conn.QueryAsync<CountryModelLess>(Query);
            var entity = quary.FirstOrDefault();

            if(entity==null)
            {
                return new CountryModelLess { CountryId = 0 };

            }

            conn.Close();
            return entity;
        
        }


        // Get Ekstenzije
        public override string AddFilter(string query, CountrySearchObject search = null)
        {
                  
            
            
            if (!string.IsNullOrWhiteSpace(search?.CountryName))
            {
                query += $@"where (lower(""CountryName"") LIKE lower('%{search.CountryName}%')) ";
                          
            }
            if (search.CountryId != null && string.IsNullOrWhiteSpace(search?.CountryName))
            {
                query += $@"where {PrimaryKey} = {search.CountryId} ";

            }

            if (search.CountryId != null && !string.IsNullOrWhiteSpace(search?.CountryName))
            {
                query += $@"or {PrimaryKey} = {search.CountryId} ";

            }
                   





            return query;


        }

        //Insert extensions

        //query ekstenzije
       
        public override string GetAtribute1()
        {
           
            return $@"""CountryName""";
        }            
      
       
        
        public override string GetValuesAllBesidesPrimary(CountryInsertRequest Insert)
        {
            return $@"'{Insert.CountryName}'";
        }
        public override string ChoseAtribute()
        {
            return PrimaryKey;
        }


        //insert esktenzije
        public override bool BeforeInsertBool(CountryInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Country"" 
             where (lower(""CountryName"") = lower('{insert.CountryName}'))");
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
        public override bool checkIfNameSame(CountryInsertRequest insert, Country entry)
        {
            if (insert.CountryName == entry?.CountryName)
            {

                return true;


            }
            return false;
        }

        public override List<CountryUpsertRequest> BeforeInsertFilterList(IEnumerable<CountryUpsertRequest> List)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();
            var Query = "";
            List<CountryUpsertRequest> OutputList = new List<CountryUpsertRequest>();

            foreach (var item in List)
            {
                //ako korisnik nije unjeo id               
                Query = $@"Select * From ""BettingSystem"".""Country"" 
                        Where lower({GetAtribute1()}) =lower('{item.CountryName}')";

                var entity = conn.Query<CountryUpsertRequest>(Query).FirstOrDefault();
                
                if (entity != null)
                {
                    continue;
                
                }

                OutputList.Add(item);
            }

            foreach (var a in OutputList)
            {
                if (a.CountryId == 0)
                {
                    CountryInsertRequest b=null;

                    var insert = Mapper.Map<CountryInsertRequest>(a);

                    InsertAsync(insert).Wait();
                    a.CountryId = GetIdByNameAsync(a.CountryName).Result.CountryId;

                   
                }
            
            
            }

            conn.CloseAsync();


            return OutputList;






        }
        public override void BeforeInsertVoid(CountryInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Country"" 
             where (lower(""CountryName"") = lower('{insert.CountryName}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: DRZAVA SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();
           
        }
        public override void BeforeInsertVoid(CountryUpsertRequest Update)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Country"" 
             where (lower(""CountryName"") = lower('{Update.CountryName}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: DRZAVA SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();
        }
        public override void BeforeDelete(int id)
        {



            string Query = null;
            string Query2 = null;
            string TableName  = "teams";
            string TableName2 = "competition";
            string foreignkey = "countryid";

            Query  += $@"Select * from ""BettingSystem"".{TableName}";
            Query2 += $@"Select * from ""BettingSystem"".{TableName2}";
            Query  += $@" Where {foreignkey} = {id}";
            Query2 += $@" Where {foreignkey} = {id}";
           
            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();

            var entry = conn.QueryFirstOrDefault<teams>(Query);
            var dalipostojicompetition = conn.QueryFirstOrDefault<competition>(Query2);

            conn.Close();

            if (entry != null)
            {

                throw new Exception("Team got a relation with the Country you want to Delete");


            }

            if (dalipostojicompetition != null)
            {

                throw new Exception("the Country you want to delete got a relation with a entry from the table Competition");

            }
        }
     
    }
}


