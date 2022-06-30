using Microsoft.Extensions.Configuration;
using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBettingSystemV2.Services.Interface;
using AutoMapper;
using Npgsql;
using Dapper;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public class BCrudNPGSQLService<T, TDb, TSearch, TInsert, TUpdate, Tless,T1>
       :BaseNPGSQLService<T, TDb, TSearch, Tless,T1>,
       ICrudNPGSQL<T, TSearch, TInsert, TUpdate, Tless,T1>
       where T : class
       where TDb : class
       where TSearch : BaseSearchObject
       where TInsert : class
       where TUpdate : class
       where Tless : class
       where T1 : class
    {
        private IConfiguration service1;
        private IMapper service3;

        public BCrudNPGSQLService(IConfiguration Service1 , IMapper Service3)
        : base(Service1,Service3) { }

        public virtual async Task<Tless> InsertAsync(TInsert insert)
        {
            //provjere

            BeforeInsertVoid(insert);

            if (!BeforeInsertBool(insert))
            {
                return null;
            }
            //provjere

            string Query = null;
            string typeParameterType = typeof(TDb).Name;
            Query += $@"insert into ""BettingSystem"".""{typeParameterType}""";
           
            var Atributes = GetAtributes();

            Query +=$@"({Atributes})";

         

            var values = GetValues(insert);

            Query += $@" values({values})";

            var idname = GetIdName();

            Query += $@" returning {idname}";


            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var entity = await conn.QuerySingleAsync<TDb>(
            Query);

           
            BeforeInsert(insert, entity);
         
            return Mapper.Map<Tless>(entity);


        }

        






        // quary extenzije
        public virtual string GetValues(TInsert insert)
        {
            return "";
        }

        public virtual string GetAtributes()
        {
            return "";
        }
        public virtual string GetIdName()
        {
            return "";
        }


        //before Extenzije
        public virtual void BeforeInsertVoid(TInsert insert)
        {

        }
        public virtual bool BeforeInsertBool(TInsert insert)
        {
            return true;
        }
        public virtual void BeforeInsert(TInsert insert, TDb entity)
        {

        }

        

    }
}
