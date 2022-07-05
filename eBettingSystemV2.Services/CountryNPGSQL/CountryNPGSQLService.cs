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
using eBettingSystemV2.Services.Interface;
using Npgsql;
using Microsoft.Data.SqlClient;
using Dapper;
using NUnit.Framework;
//using TourismAgency.Services.Database;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public class CountryNPGSQLService :     
        BCrudNPGSQLService
        <
        CountryModel,
        Country,
        CountrySearchObject,
        CountryInsertRequest,
        CountryUpsertRequest,
        CountryModelLess,
        string
        >      
        , ICountryNPGSQL
    {
        public CountryNPGSQLService(IConfiguration Service1, IMapper Service3)
        : base(Service1,Service3) {

            PrimaryKey = $@"""CountryId""";

            var list = typeof(CountryModel).GetProperties();

            foreach (var a in list)
            {
                if (a.Name != "CountryId")
                {
                    var text = a.Name.Any(char.IsUpper) ? $@"""{a.Name}""" : a.Name;

                    ListaAtributa.Add(text);                               
                }

            
            }
          
           
           

        }


        // Get Ekstenzije
        public override string AddFilter(string query, CountrySearchObject search = null)
        {
                  
            
            //foreach (var a in list)
            //{
            //    string name = a.Name;

            //    var objectneki = a.GetCustomAttributes(true);

            //    foreach (object attr in objectneki)
            //    {
            //        //AuthorAttribute authAttr = attr as AuthorAttribute;
            //        string propName = a.Name;
            //        string auth = attr.ToString();


            //    }
            
            //}



            //search.Naziv = "Engleska";
            //search.CountryId = 12;

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
        public override string GetCoalesce(CountryUpsertRequest Update)
        {
            //ako je 0 to je null ,ako je string to je null

            return $@"
               {GetTheNameOfIdentityColumn()}=coalesce({Update.CountryId},""BettingSystem"".""Country"".{GetTheNameOfIdentityColumn()}),
               {GetAtribute1()}=coalesce('{Update.CountryName}',""BettingSystem"".""Country"".{GetAtribute1()})
                    ";       
           

        }
        public override string GetTheNameOfIdentityColumn()
        {
            return $@"""CountryId""";
        }
        public override string GetAtribute1()
        {
           
            return $@"""CountryName""";
        }
        public override string GetAllAtributes()
        {
            return $@"""CountryId"",""CountryName""";
        }
        public override string GetValue1(CountryInsertRequest insert)
        {
            return $@"'{insert.CountryName}'";
        }
        public override string GetValue1(CountryUpsertRequest Update)
        {
            return $@"'{Update.CountryName}'"; ;
        }
        public override string GetValuesAll(CountryInsertRequest insert, int id)
        {
            return $@"{id},'{insert.CountryName}'";
        }
        public override string GetValuesAll(CountryUpsertRequest insert)
        {
            return $@"{insert.CountryId},'{insert.CountryName}'";
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
                        Where {GetAtribute1()} ='{item.CountryName}'";

                var entity = conn.Query<CountryUpsertRequest>(Query).FirstOrDefault();
                
                if (entity != null)
                {
                    continue;
                
                }

                OutputList.Add(item);
            }

           

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






    }
}


