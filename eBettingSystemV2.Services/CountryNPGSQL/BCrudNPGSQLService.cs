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

            //if (!BeforeInsertBool(insert))
            //{
            //    return null;
            //}
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
        public virtual async Task<IEnumerable<T>> UpsertOneOrMoreAsync(IEnumerable<TUpdate> List)
        {
            //for tomorrow;


            var list = BeforeInsertFilterList(List); //ako ime vec postoji u bazi izbaci iz liste


            string Query  = null;
            string AddQuery = null;//dodatak za ako korisnik ne unose id
            string typeParameterType = typeof(TDb).Name;


            if (list.Count() != 0)
            {
                Query += $@"insert into ""BettingSystem"".""{typeParameterType}""";
                AddQuery += $@"insert into ""BettingSystem"".""{typeParameterType}"""; //dodatak za ako korisnik ne unose id


                var Atributes = GetAllAtributes();
                Query += $@"({Atributes})";

                Query += "Values";
                for (int i = 0; i < list.Count(); i++)
                {
                    Query += "(";
                    Query += GetValuesAll(list[i]);
                    Query += ")";

                    if ((i + 1) != list.Count())
                    {
                        Query += ",";

                    }

                }
                Query += $@"ON CONFLICT ({PrimaryKey}) DO ";
                Query += $@"UPDATE SET {GetAtribute1()} = EXCLUDED.{GetAtribute1()}";

                var allatributes = GetAllAtributes();

                Query += $@" returning {allatributes}";

                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                var entity = await conn.QueryAsync<T>(Query);
                //IEnumerable<Tless> OutPut = OutputList;

                conn.Close();

                return entity;
            }

            throw new Exception("Lista nije validna");


        }

        public virtual async Task<IEnumerable<T>> InsertOneOrMoreAsync(IEnumerable<TInsert>List)
        {
            string Query = null;
            string typeParameterType = typeof(TDb).Name;
            var list = List.ToList();
            var Atributes = GetAllAtributesBesidesPrimary(List.FirstOrDefault().GetType());


            Query += $@"insert into ""BettingSystem"".""{typeParameterType}""";
            Query += $@"({Atributes})";
            Query += " Values";
            for (int i = 0; i < list.Count(); i++)
            {
                Query += "(";
                Query += GetValuesAllBesidesPrimary(list[i]);
                Query += ")";

                if ((i + 1) != list.Count())
                {
                    Query += ",";

                }

            }         
            Query += $@" On Conflict ({GetAtribute1()}) DO NOTHING";
            Query += $@" Returning {GetAllAtributes()}";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var entity = await conn.QueryAsync<T>(Query);
            //IEnumerable<Tless> OutPut = OutputList;

            conn.Close();

            return entity;



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
                                ON CONFLICT({PrimaryKey}) 
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
                           {PrimaryKey}={id}
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

        //Delete metode
        public virtual async Task<int> DeleteAsync(int id)
        {

            BeforeDelete(id);

            string Query = null;
            string typeParameterType = typeof(TDb).Name;

            Query += $@"DELETE FROM ""BettingSystem"".""{typeParameterType}""";
            Query += $@"Where {PrimaryKey} = {id} ";
            Query += $@"Returning {GetAllAtributes()}";



            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var entity = await conn.QueryFirstOrDefaultAsync<TDb>(Query);

            conn.Close();

            if(entity == null)
            {
                return -1;
            }

            return id;
            
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
        public virtual string GetValuesAll(TUpdate insert)
        {
            return $@"";
        }
        public virtual string GetValuesAllBesidesPrimary(TInsert Insert)
        {
            return "";
        }
        public virtual string GetAtribute1()
        {
            return "";
        }
        public virtual string GetAllAtributesBesidesPrimary(Type Tip)
        {
            return "";
        }
        public virtual string GetAllAtributes()
        {
            return "";
        }
        public virtual string GetAllAtributes(TInsert insert)
        {

            return "";
        
        }

        


        //before Extenzije

        public virtual List<TUpdate> BeforeInsertFilterList(IEnumerable<TUpdate> List)
        {

            return List.ToList();
        }
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
        public virtual bool BeforeInsertBool(TUpdate Update)
        {
            return true;
        }
        public virtual void BeforeInsert(TInsert insert, TDb entity)
        {

        }
        public virtual void BeforeDelete(int id)
        {


        }



    }
}
