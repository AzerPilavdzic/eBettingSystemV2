using Microsoft.Extensions.Configuration;
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

        public IMapper Mapper { get; set; }
        
      
        public string connString { get; set;} 
        public BaseNPGSQLService(IConfiguration Service1 ,IMapper Service3 )
        {
            Configuration = Service1;          
            Mapper = Service3;

            connString = Configuration.GetSection("ConnectionString").GetSection("DefaultConnection").Value;


        }

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

                var entity = await conn.QueryAsync<TDb>(Query);


                var quary = entity.ToList().AsQueryable();

                var list = entity.ToList();


                return Mapper.Map<IEnumerable<T>>(list);
                //return list;
            }
            catch (Exception e)
            {

                return null;
            
            }



            

        }


        public virtual string AddFilter(string query,TSearch search = null)
        {
            return query;
        }

       
    }
}
