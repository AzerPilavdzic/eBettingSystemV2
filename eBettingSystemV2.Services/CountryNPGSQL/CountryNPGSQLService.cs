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
        : base(Service1,Service3) { }

        public override string AddFilter(string query, CountrySearchObject search = null)
        {

            //search.Naziv = "Engleska";
            //search.CountryId = 12;
        
            if (!string.IsNullOrWhiteSpace(search?.Naziv))
            {
                query += $@"where (lower(""CountryName"") LIKE lower('%{search.Naziv}%')) ";
                          
            }
            if (search.CountryId != null && string.IsNullOrWhiteSpace(search?.Naziv))
            {
                query += $@"where ""CountryId"" = {search.CountryId} ";

            }

            if (search.CountryId != null && !string.IsNullOrWhiteSpace(search?.Naziv))
            {
                query += $@"or ""CountryId"" = {search.CountryId} ";

            }

            return query;


        }


        //Insert extensions

        public override string GetAtributes()
        {
           
            return $@"""CountryName""";
        }

        public override string GetValues(CountryInsertRequest insert)
        {
            return $@"'{insert.CountryName}'";
        }

        public override string GetIdName()
        {
            return $@"""CountryId"",""CountryName""";
        }



        public override bool BeforeInsertBool(CountryInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Country"" 
             where (lower(""CountryName"") = lower('{insert.CountryName}'))");
            var entity = List.FirstOrDefault();

            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: DRZAVA SA TIM IMENOM VEC POSTOJI.");
        }






    }
}


