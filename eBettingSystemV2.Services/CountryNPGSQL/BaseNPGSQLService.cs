﻿using Microsoft.Extensions.Configuration;
using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Services.Extensions;
using eBettingSystemV2.Services.Interface;
using AutoMapper;
using Dapper;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public class BaseNPGSQLService<T, TDb, TSearch, Tless,T1>:
        IBaseNPGSQL<T, TSearch, Tless,T1>
        where T : class
        where TDb : class
        where TSearch : BaseSearchObject//base search service
        where Tless : class
        where T1: class
    {
        public IConfiguration Configuration { get; }
        public IMapper Mapper { get; set; }// ne treba ovdje             
        public string connString { get; set;}
        public string PrimaryKey { get; set; } = "";
        public List<string> ListaAtributa { get; set; } = new List<string>();
        public BaseNPGSQLService(IConfiguration Service1 ,IMapper Service3 )
        {
            Configuration = Service1;          
            Mapper = Service3;

            connString = Configuration.GetSection("ConnectionString").GetSection("DefaultConnection").Value;


        }

        //Funkcije

        //Get Funkcije
        public async virtual Task<IEnumerable<T>> GetNPGSQLGeneric(TSearch search = null)
        {
            try
            {

                string Query = null;
                string typeParameterType = typeof(TDb).Name;
                Query += $@"select *  from ""BettingSystem"".""{typeParameterType}"" ";

                Query = AddFilter(Query, search);

                int page = search.Page.HasValue ? search.Page.Value : 0;
                int PageSize = search.PageSize.HasValue ? search.PageSize.Value : 0;
                int OFFSET = (page - 1) * PageSize;

                Query += $@"LIMIT {PageSize} OFFSET {OFFSET};";


                //konnekcija
                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                var entity = await conn.QueryAsync<T>(Query);


                var quary = entity.ToList().AsQueryable();

                return quary;


                //var list = entity.ToList();

                //return Mapper.Map<IEnumerable<T>>(list);

                
                
            }
            catch (Exception e)
            {

                return null;
            
            }



            

        }
        public async virtual Task<T>GetByIdAsync(int id)
        {
            string Query = null;
            string typeParameterType = typeof(TDb).Name;
            Query += $@"select *  from ""BettingSystem"".""{typeParameterType}"" ";

            Query += $@"where {PrimaryKey} = {id}; ";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var quary = await conn.QueryAsync<T>(Query);

            var entity = quary.FirstOrDefault();

            return entity;



            //var list = entity.ToList();

            //var entity = set.Find(id);

            //return Mapper.Map<T>(entity);

        }
        //Get Extenzije
        public virtual string AddFilter(string query,TSearch search = null)
        {
            return query;
        }


        //query ekstenzije

        



    }
}