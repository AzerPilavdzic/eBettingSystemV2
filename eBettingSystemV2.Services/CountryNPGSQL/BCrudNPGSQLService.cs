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
        

        public BCrudNPGSQLService(IConfiguration Service1 , IMapper Service3)
        : base(Service1,Service3) { }

        //Insert Upsert
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
           
            var Atributes = GetAtribute1();

            Query +=$@"({Atributes})";

            var values = GetValue1(insert);

            Query += $@" values({values})";

            var allatributes = GetAllAtributes();

            Query += $@" returning {allatributes}";


            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var entity = await conn.QuerySingleAsync<TDb>(
            Query);

           
            BeforeInsert(insert, entity);
         
            return Mapper.Map<Tless>(entity);


        }
        public virtual async Task<Tless> UpsertbyIdAsync(TInsert Insert, int Id)
        {
            try
            {

           
            BeforeInsertVoid(Insert); 

            string Query = null;

            string typeParameterType = typeof(TDb).Name;

            if (Id!=0)
            {
                Query = $@"INSERT INTO ""BettingSystem"".""{typeParameterType}"" ({GetAllAtributes()})
                                VALUES({GetValuesAll(Insert, Id)}) 
                                ON CONFLICT({GetTheNameOfIdentityColumn()}) 
                                DO 
                                UPDATE SET {GetAtribute1()} = {GetValue1(Insert)}
                                returning {GetAllAtributes()}";

            }

            if (Id==0)
            {
                Query = $@"INSERT INTO ""BettingSystem"".""{typeParameterType}"" ({GetAtribute1()})
                                VALUES({GetValue1(Insert)})                              
                                returning {GetAllAtributes()}";

            }
        
            await using var conn = new NpgsqlConnection(connString);

            await conn.OpenAsync();

            var quary = await conn.QueryAsync<Tless>(Query);

            var entity = quary.FirstOrDefault();
   
            return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Drzava sa tim imenom vec postoji");
            }

        }

        //Update Metode
        public virtual async Task<T> UpdateAsync(int id, TUpdate update)
        {
            try
            {
                BeforeInsertVoid(update);

                string Query = null;

                string typeParameterType = typeof(TDb).Name;
             
                Query = $@"UPDATE ""BettingSystem"".""{typeParameterType}""
                           SET
                           {GetCoalesce(update)}
                           WHERE 
                           {GetTheNameOfIdentityColumn()}={id}
                           returning {GetAllAtributes()}";
                    
                await using var conn = new NpgsqlConnection(connString);

                await conn.OpenAsync();

                var quary = await conn.QueryAsync<T>(Query);

                var entity = quary.FirstOrDefault();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Drzava sa tim imenom vec postoji");
            }
        }

        //Update Upsert extenzije
        public virtual TUpdate Coalesce(TUpdate update, TDb entry)
        {
            return update;

        }

        //Upsert extenzije
        public virtual bool checkIfNameSame(TInsert insert, TDb entry)
        {

            return false;

        }

        //quary extenzije


        public virtual string GetCoalesce(TUpdate Update)
        {
            return "";
        }
        public virtual int GetkeyValue(TUpdate Update)
        {
            return 0;
        }
        public virtual string GetValue1(TInsert insert)
        {
            return "";
        }
        public virtual string GetValue1(TUpdate Update)
        {
            return "";
        }
        public virtual string GetValuesAll(TInsert insert, int id)
        {
            return $@"";
        }
        public virtual string GetAtribute1()
        {
            return "";
        }
        public virtual string GetAllAtributes()
        {
            return "";
        }

        


        //before Extenzije
        public virtual void BeforeInsertVoid(TInsert insert)
        {

        }
        public virtual void BeforeInsertVoid(TUpdate Update)
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
