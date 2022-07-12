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
    public class CompetitionNPGSQLService :
        BCrudNPGSQLService
        <
        CompetitionModel,
        Competition,
        CompetitionSearchObject,
        CompetitionInsertRequest,
        CompetitionUpsertRequest,
        CompetitionModelLess
        >
        , ICompetitionNPGSQL
    {
        public CompetitionNPGSQLService(IConfiguration Service1, IMapper Service3)
        : base(Service1, Service3)
        {

            PrimaryKey = $@"""CompetitionsId""";
            var list = typeof(CompetitionModel).GetProperties();

            foreach (var a in list)
            {
                if (a.Name != "CompetitionsId")
                {
                    var text = a.Name.Any(char.IsUpper) ? $@"""{a.Name}""" : a.Name;
                    ListaAtributa.Add(text);
                }
            }
        }


        //Get Funkcije
        public async Task<CompetitionModelLess> GetIdByNameAsync(string name)
        {

            string Query = null;
            string typeParameterType = typeof(Competition).Name;
            Query += $@"select *  from ""BettingSystem"".""{typeParameterType}"" ";
            Query += $@"where {GetAtribute1()} = '{name}'; ";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var quary = await conn.QueryAsync<CompetitionModelLess>(Query);
            var entity = quary.FirstOrDefault();

            if (entity == null)
            {
                return new CompetitionModelLess { Id = 0 };

            }

            conn.Close();
            return entity;

        }




        // Get Ekstenzije
        public override string AddFilter(string query, CompetitionSearchObject search = null)
        {

            if (!string.IsNullOrWhiteSpace(search?.naziv))
            {
                query += $@"where (lower(""name"") LIKE lower('%{search.naziv}%')) ";

            }
            if (search.id != null && string.IsNullOrWhiteSpace(search?.naziv))
            {
                query += $@"where {PrimaryKey} = {search.id} ";

            }

            if (search.id != null && !string.IsNullOrWhiteSpace(search?.naziv))
            {
                query += $@"or {PrimaryKey} = {search.id} ";

            }


            return query;


        }

        //Insert extensions

        //query ekstenzije
        public override string GetCoalesce(CompetitionUpsertRequest Update)
        {
            //ako je 0 to je null ,ako je string to je null

            return $@"
               {PrimaryKey}=coalesce({Update.id},""BettingSystem"".""Competition"".{PrimaryKey}),
               {GetAtribute1()}=coalesce('{Update.naziv}',""BettingSystem"".""Competition"".{GetAtribute1()})
                    ";


        }
        public override string GetAtribute1()
        {
            return $@"""name""";
        }
        public override string GetValue1(CompetitionInsertRequest insert)
        {
            return $@"'{insert.Naziv}'";
        }
        public override string GetValuesAll(CompetitionInsertRequest insert, int id)
        {
            return $@"{id},'{insert.Naziv}'";
        }
        public override string GetValuesAll(CompetitionUpsertRequest insert)
        {
            return $@"{insert.id},'{insert.naziv}'";
        }
        public override string GetValuesAllBesidesPrimary(CompetitionInsertRequest Insert)
        {
            return $@"'{Insert.Naziv}'";
        }


        //insert esktenzije
        public override bool BeforeInsertBool(CompetitionInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Competition"" 
             where (lower(""name"") = lower('{insert.Naziv}'))");
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
        public override bool checkIfNameSame(CompetitionInsertRequest insert, Competition entry)
        {
            if (insert.Naziv == entry?.Naziv)
            {

                return true;


            }
            return false;
        }

        public override List<CompetitionUpsertRequest> BeforeInsertFilterList(IEnumerable<CompetitionUpsertRequest> List)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();
            var Query = "";
            List<CompetitionUpsertRequest> OutputList = new List<CompetitionUpsertRequest>();

            foreach (var item in List)
            {
                Query = $@"Select * From ""BettingSystem"".""competition"" 
                        Where {GetAtribute1()} ='{item.naziv}'";

                var entity = conn.Query<CompetitionUpsertRequest>(Query).FirstOrDefault();

                if (entity != null)
                {
                    continue;

                }

                OutputList.Add(item);
            }



            return OutputList;






        }
        public override void BeforeInsertVoid(CompetitionInsertRequest insert)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""competition"" 
             where (lower(""name"") = lower('{insert.Naziv}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: Competition SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();

        }
        public override void BeforeInsertVoid(CompetitionUpsertRequest Update)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var List = conn.Query($@"Select * from ""BettingSystem"".""Competition"" 
             where (lower(""name"") = lower('{Update.naziv}'))");
            var entity = List.FirstOrDefault();

            if (entity != null)
            {
                throw new Exception("EXCEPTION: Competition SA TIM IMENOM VEC POSTOJI.");
            }
            conn.Close();
        }
        public override void BeforeDelete(int id)
        {

            string Query = null;
            string Query2 = null;
            string TableName = "teams";
            string TableName2 = "competition";
            string foreignkey = "CompetitionId";

            Query += $@"Select * from ""BettingSystem"".{TableName}";
            Query2 += $@"Select * from ""BettingSystem"".{TableName2}";
            Query += $@" Where {foreignkey} = {id}";
            Query2 += $@" Where {foreignkey} = {id}";

            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();

            var entry = conn.QueryFirstOrDefault<teams>(Query);
            var dalipostojicompetition = conn.QueryFirstOrDefault<Competition>(Query2);

            conn.Close();

            if (entry != null)
            {

                throw new Exception("EXCEPTION CompetitionsNPGSQLService line 281");


            }

            if (dalipostojicompetition != null)
            {

                throw new Exception("EXCEPTION CompetitionsNPGSQLService line 289");

            }
        }

    }
}


