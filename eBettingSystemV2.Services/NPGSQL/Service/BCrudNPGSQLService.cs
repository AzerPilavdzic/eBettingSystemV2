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
using eBettingSystemV2.Services.NPGSQL.Service;
using eBettingSystemV2.Services.NPGSQL.Interface;

namespace eBettingSystemV2.Services.NPGSQL.Service

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
            var Atributes = GetAllAtributesFromModel(insert.GetType());
            var values = GetAllValuesFromModel(typeof(TInsert), insert);
            var ReturnAtributes = GetAllAtributesFromModel(typeof(TDb));

            Query += $@"insert into ""BettingSystem"".""{typeParameterType}""";                     
            Query += $@"({Atributes})";          
            Query += $@" values({values})";            
            Query += $@" returning {ReturnAtributes}";


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
            var stringtext = GetAllValuesFromModel(typeof(TUpdate),List.FirstOrDefault());

            var list = BeforeInsertFilterList(List); //ako ime vec postoji u bazi izbaci iz liste


            string Query  = null;
            string AddQuery = null;//dodatak za ako korisnik ne unose id
            string typeParameterType = typeof(TDb).Name;


            if (list.Count() != 0)
            {
                Query += $@"insert into ""BettingSystem"".""{typeParameterType}""";
                AddQuery += $@"insert into ""BettingSystem"".""{typeParameterType}"""; //dodatak za ako korisnik ne unose id


                var Atributes = GetAllAtributesFromModel(typeof(TUpdate));
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

                var allatributes = GetAllAtributesFromModel(typeof(TDb));

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
            var Atributes = GetAllAtributesFromModel(typeof(TInsert));


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
            Query += $@" Returning {GetAllAtributesFromModel(typeof(TDb))}";

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
                Query = $@"INSERT INTO ""BettingSystem"".""{typeParameterType}"" ({GetAllAtributesFromModel(typeof(TDb))})
                                VALUES({GetValuesAll(Insert, Id)}) 
                                ON CONFLICT({PrimaryKey}) 
                                DO 
                                UPDATE SET {GetAtribute1()} = {GetValue1(Insert)}
                                returning {GetAllAtributesFromModel(typeof(TDb))}";

            }

            if (Id==0)
            {
                Query = $@"INSERT INTO ""BettingSystem"".""{typeParameterType}"" ({GetAllAtributesFromModel(typeof(TInsert))})
                                VALUES({GetValue1(Insert)})                              
                                returning {GetAllAtributesFromModel(typeof(TDb))}";

            }
        
            await using var conn = new NpgsqlConnection(connString);

            await conn.OpenAsync();

            var quary = await conn.QueryAsync<Tless>(Query);

            var entity = quary.FirstOrDefault();

            conn.Close();

   
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
                           returning {GetAllAtributesFromModel(typeof(TDb))}";
                    
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
            Query += $@"Returning {GetAllAtributesFromModel(typeof(TDb))}";



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
        public virtual string GetValue1(TInsert insert)
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
        public string GetAllAtributesFromModel(Type Tip)
        {
            ListaAtributa.Clear();

            var list = Tip.GetProperties();

            foreach (var a in list)
            {
                if (a.PropertyType.Name != typeof(string).Name &&
                    a.PropertyType.Name != typeof(int).Name &&
                    a.PropertyType.Name != typeof(int?).Name 
                    )
                {

                    continue;
                
                }


                var text = a.Name.Any(char.IsUpper) ? $@"""{a.Name}""" : a.Name;
                ListaAtributa.Add(text);

            }

            var returnstring = "";



            for (int i = 0; i < ListaAtributa.Count(); i++)
            {

                returnstring += ListaAtributa[i];

                if ((i + 1) != ListaAtributa.Count())
                {
                    returnstring += ",";


                }

            }

            ListaAtributa.Clear();

            return returnstring;


        }
        public string GetAllValuesFromModel(Type Tip, object objekt)
        {
            ListaAtributa.Clear();

            var list = Tip.GetProperties();

           
            var ListaValues = new List<string>();
            foreach (var b in list)
            {
                if (b.PropertyType.Name != typeof(string).Name &&
                    b.PropertyType.Name != typeof(int).Name &&
                    b.PropertyType.Name != typeof(int?).Name)
                {

                    continue;

                }
                var nameOfProperty = b.Name;
                var propertyInfo = objekt.GetType().GetProperty(nameOfProperty);
                var value = propertyInfo.GetValue(objekt, null);
                ListaValues.Add(value.GetType() == typeof(string) ? $@"'{value}'" : value.ToString());

            }

            var returnstring = "";

            for (int i = 0; i < ListaValues.Count(); i++)
            {

                returnstring += ListaValues[i];

                if ((i + 1) != ListaValues.Count())
                {
                    returnstring += ",";


                }

            }

            ListaAtributa.Clear();

            return returnstring;



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
