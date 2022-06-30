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



            int i = 0;

            if (!string.IsNullOrWhiteSpace(search?.Naziv))
            {
                query += $@"where (lower(""CountryName"") LIKE lower('%{search.Naziv}%')) ";

                i++;

                
            }

            if (search.CountryId != null && i==0)
            {
                query += $@"where ""CountryId"" = {search.CountryId} ";

            }

            if (search.CountryId != null && i == 1)
            {
                query += $@"or ""CountryId"" = {search.CountryId} ";

            }

            return query;


        }






    }
}


