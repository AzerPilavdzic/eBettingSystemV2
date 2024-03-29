﻿using Microsoft.Extensions.Configuration;
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
    public class BCrudNPGSQLService<T, TDb, TSearch, TInsert, TUpdate, Tless>
       :BaseNPGSQLService<T, TDb, TSearch, Tless>,
       ICrudNPGSQL<T, TSearch, TInsert, TUpdate, Tless>
       where T : class
       where TDb : class
       where TSearch : BaseSearchObject
       where TInsert : class
       where TUpdate : class
       where Tless : class

    {


        public BCrudNPGSQLService(IConfiguration Service1, IMapper Service3)
        : base(Service1, Service3) { }

        //Insert Upsert
        public virtual async Task<Tless> InsertAsync(TInsert insert)
        {
            //provjere
            try
            {


           
           
            BeforeInsertVoid(insert,0);


            //if (!BeforeInsertBool(insert))
            //{
            //    return null;
            //}
            //provjere

            string Query = null;
            string typeParameterType = typeof(TDb).Name;

            string TableName = typeParameterType.Any(char.IsUpper) ? $@"""{typeParameterType}""" : typeParameterType;
            var Atributes = GetAllAtributesFromModel(insert.GetType());
            var values = GetAllValuesFromModel(typeof(TInsert), insert);
            var ReturnAtributes = GetAllAtributesFromModel(typeof(TDb));



            Query += $@"insert into ""BettingSystem"".{TableName}";                     
            Query += $@"({Atributes})";          
            Query += $@" values({values})";            

            Query += $@" returning {ReturnAtributes}";




            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var entity = await conn.QuerySingleAsync<Tless>(
            Query);

                return entity;
            

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public virtual async Task<IEnumerable<T>> UpsertOneOrMoreAsync(IEnumerable<TUpdate> List)
        {
            //ako se ne proslijedi fk daje exception  



           

            var list = BeforeInsertFilterList(List); //ako ime vec postoji u bazi izbaci iz liste

           


            string Query  = null;
            

            string typeParameterType = typeof(TDb).Name;

            string TableName = typeParameterType.Any(char.IsUpper) ? $@"""{typeParameterType}""" : typeParameterType;


            if (list.Count() != 0)
            {
                Query += $@"insert into ""BettingSystem"".{TableName}";

                var Atributes = GetAllAtributesFromModel(typeof(TUpdate));
                Query += $@"({Atributes})";

                Query += "Values";
                for (int i = 0; i < list.Count(); i++)
                {
                    Query += "(";

                    

                    Query += GetAllValuesFromModel(typeof(TUpdate),list[i]);

                    Query += ")";

                    if ((i + 1) != list.Count())
                    {
                        Query += ",";

                    }

                }

             

                Query += $@"ON CONFLICT ({ConflictUpsert}) DO ";
                Query += $@"UPDATE SET {GetCoalesce2conflict(typeof(TUpdate),TableName)}";


                var allatributes = GetAllAtributesFromModel(typeof(TDb));

                Query += $@" returning {allatributes}";

                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                var entity = await conn.QueryAsync<T>(Query);
                //IEnumerable<Tless> OutPut = OutputList;

                conn.Close();

                return entity;
            }

            throw new Exception("Upsert nije izvrsen");


        }
        public virtual async Task<IEnumerable<T>> InsertOneOrMoreAsync(IEnumerable<TInsert> List)
        {

            checkforexceptions(List);


            string Query = null;

            string typeParameterType = typeof(TDb).Name;


            string TableName = typeParameterType.Any(char.IsUpper) ? $@"""{typeParameterType}""" : typeParameterType;

            


            var list = List.ToList();
            var Atributes = GetAllAtributesFromModel(typeof(TInsert));

            

            Query += $@"insert into ""BettingSystem"".""{TableName}""";
            Query += $@"({Atributes})";
            Query += " Values";
            for (int i = 0; i < list.Count(); i++)
            {
                Query += "(";


                Query += GetAllValuesFromModel(typeof(TInsert),list[i]);

                Query += ")";

                if ((i + 1) != list.Count())
                {
                    Query += ",";

                }


                    
            }         
            Query += $@" On Conflict ({Conflictinsert}) DO NOTHING";

            Query += $@" Returning {GetAllAtributesFromModel(typeof(TDb))}";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var entity = await conn.QueryAsync<T>(Query);
            //IEnumerable<Tless> OutPut = OutputList;

            conn.Close();

            if (entity.Count() == 0)
            {

                throw new Exception(exception);
            
            }


            return entity;



        }
        public virtual async Task<Tless> UpsertbyIdAsync(TInsert Insert, int Id)
        {
            //UPDATE SET { GetAtribute1()} = { GetValue1(Insert)}


            try
            {

            BeforeInsertVoid(Insert,Id);
            Insert = foreignkeyfix(Id, Insert);




                string Query = null;


                string typeParameterType = typeof(TDb).Name;



                string TableName = typeParameterType.Any(char.IsUpper) ? $@"""{typeParameterType}""" : typeParameterType;
                if (Id!=0)
            {
                Query = $@"INSERT INTO ""BettingSystem"".{TableName} ({PrimaryKey},{GetAllAtributesFromModel(typeof(TInsert))})
                                VALUES({Id},{GetAllValuesFromModel(typeof(TInsert),Insert)}) 
                                ON CONFLICT({ConflictUpsert}) 
                                DO 
                                UPDATE SET {GetCoalesce2conflict(typeof(TUpdate),TableName)}
                                returning {GetAllAtributesFromModel(typeof(TDb))}";

                }



            if (Id==0)
            {
                Query = $@"INSERT INTO ""BettingSystem"".{TableName} ({GetAllAtributesFromModel(typeof(TInsert))})
                                VALUES({GetAllValuesFromModel(typeof(TInsert), Insert)})                              
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
                throw new Exception(ex.ToString());
            }

        }
        //Update Metode
        public virtual async Task<T> UpdateAsync(int id, TInsert update)
        {
            try
            {
                BeforeInsertVoid(update,id);
                //update =foreignkeyfix(id, update);

                string Query = null;

                string typeParameterType = typeof(TDb).Name;

                string TableName = typeParameterType.Any(char.IsUpper) ? $@"""{typeParameterType}""" : typeParameterType;

                Query = $@"UPDATE ""BettingSystem"".{TableName}
                           SET                         
                           {GetCoalesce2(typeof(TInsert),update,TableName)}
                           WHERE 
                           {PrimaryKey}={id}
                           returning {GetAllAtributesFromModel(typeof(TDb))}";

                await using var conn = new NpgsqlConnection(connString);

                await conn.OpenAsync();

                var quary = await conn.QueryAsync<T>(Query);

               await conn.CloseAsync();

                var entity = quary.FirstOrDefault();

                return entity;
            }
            catch (Exception ex)
            {
                //throw new Exception("Drzava sa tim imenom vec postoji");
                throw new Exception(ex.Message);
            }
        }
        //Delete metode
        public virtual async Task<int> DeleteAsync(int id)
        {

            BeforeDelete(id);
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            string Query = null;
            string typeParameterType = typeof(TDb).Name;

            Query += $@"DELETE FROM ""BettingSystem"".""{typeParameterType}""";
            Query += $@"Where {PrimaryKey} = {id} ";
            Query += $@"Returning {GetAllAtributesFromModel(typeof(TDb))}";




            var entity = await conn.QueryFirstOrDefaultAsync<TDb>(Query);

            await conn.CloseAsync();

            if (entity == null)
            {
                return -1;
            }

            return id;

        }
        
        //Upsert extenzije
        public virtual bool checkIfNameSame(TInsert insert, TDb entry)
        {

            return false;

        } //not used



        //quary extenzije            
        
        
        
        public virtual string GetAtribute1()
        {
            return "";
        } //used
       
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


        } //used
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

                if (value == null || value.ToString() == "0")
                {

                    ListaValues.Add("null");
                }
                else
                {
                    ListaValues.Add(value.GetType() == typeof(string) || value.GetType()==typeof(DateTime) ? $@"'{value}'" : value.ToString());
                }


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



        } //used
        public string UpdateSet(Type Tip, object objekt)
        {
            List<string> ListaAtributa = new List<string>();
            List<string> ListaValues = new List<string>();
            var list = Tip.GetProperties();

            foreach (var b in list)
            {
                if (b.PropertyType.Name != typeof(string).Name &&
                    b.PropertyType.Name != typeof(int).Name &&
                    b.PropertyType.Name != typeof(int?).Name)
                {

                    continue;

                }

                var nameOfProperty = b.Name;

                var nameOfProperty2 = nameOfProperty.Any(char.IsUpper) ? $@"""{nameOfProperty}""" : nameOfProperty;

                if (nameOfProperty == PrimaryKey)
                {
                    continue;

                }

                ListaAtributa.Add(nameOfProperty2);

                var propertyInfo = objekt.GetType().GetProperty(nameOfProperty);

                var value = propertyInfo.GetValue(objekt, null);

                if (value == null || value.ToString() == "0")
                {

                    ListaValues.Add("null");
                }
                else
                {

                    ListaValues.Add(value.GetType() == typeof(string) ? $@"'{value}'" : value.ToString());
                }
            }

            string query = " Update Set ";


            //drugi dio 
            for (int i = 0; i < ListaAtributa.Count; i++)
            {

                query += $@"{ListaAtributa[i]} = {ListaValues[i]}";

                if ((i + 1) != ListaAtributa.Count())
                {

                    query += ",";

                }


            }

            return query;





        } //not used 
        public virtual string GetCoalesce2(Type Tip, object objekt,string TableName)
        {
            List<string> ListaAtributa = new List<string>();
            List<string> ListaValues = new List<string>();

            var list = Tip.GetProperties();

            foreach (var b in list)
            {
                if (b.PropertyType.Name != typeof(string).Name &&
                    b.PropertyType.Name != typeof(int).Name &&
                    b.PropertyType.Name != typeof(int?).Name)
                {

                    continue;

                }

                var nameOfProperty = b.Name;

                var nameOfProperty2 = nameOfProperty.Any(char.IsUpper) ? $@"""{nameOfProperty}""" : nameOfProperty;

                if (nameOfProperty == PrimaryKey)
                {
                    continue;

                }

                ListaAtributa.Add(nameOfProperty2);

                var propertyInfo = objekt.GetType().GetProperty(nameOfProperty);

                var value = propertyInfo.GetValue(objekt, null);

                if (value == null || value.ToString() == "0")
                {

                    ListaValues.Add("null");
                }
                else
                {
                    ListaValues.Add(value.GetType() == typeof(string) ? $@"'{value}'" : value.ToString());
                }

            }

            string query = "";


            //drugi dio 
            for (int i = 0; i < ListaAtributa.Count; i++)
            {

                query += $@"{ListaAtributa[i]} = coalesce({ListaValues[i]},""BettingSystem"".{TableName}.{ListaAtributa[i]})";

                if ((i + 1) != ListaAtributa.Count())
                {

                    query += ",";

                }


            }

            return query;




        } //used
        public virtual string GetCoalesce2conflict(Type Tip, string TableName)
        {
            List<string> ListaAtributa = new List<string>();


            var list = Tip.GetProperties();

            foreach (var b in list)
            {
                if (b.PropertyType.Name != typeof(string).Name &&
                    b.PropertyType.Name != typeof(int).Name &&
                    b.PropertyType.Name != typeof(int?).Name)
                {



                    continue;

                }

                var nameOfProperty = b.Name;

                var nameOfProperty2 = nameOfProperty.Any(char.IsUpper) ? $@"""{nameOfProperty}""" : nameOfProperty;

                if (nameOfProperty == PrimaryKey)
                {
                    continue;

                }

                ListaAtributa.Add(nameOfProperty2);

               

            }

            string query = "";


            //drugi dio 
            for (int i = 0; i < ListaAtributa.Count; i++)
            {

                query += $@"{ListaAtributa[i]} = coalesce(Excluded.{ListaAtributa[i]},""BettingSystem"".{TableName}.{ListaAtributa[i]})";

                if ((i + 1) != ListaAtributa.Count())
                {

                    query += ",";

                }


            }

            return query;


        } //used



        //before Extenzije


        public virtual TInsert foreignkeyfix(int Id,TInsert insert)
        {
            //za foriegn key coalesce


            return insert;
        
        }
        public virtual List<TUpdate> BeforeInsertFilterList(IEnumerable<TUpdate> List)
        {

            return List.ToList();

        

        }
        public virtual void BeforeInsertVoid(TInsert insert,int Id)
        {

        }    //used ok
        public virtual void BeforeInsertVoid(TUpdate Update)
        {

        }   //not used in reference
        public virtual bool BeforeInsertBool(TInsert insert)
        {
            return true;
        }  //not used in reference
        public virtual void BeforeInsert(TInsert insert, TDb entity)
        {

        } //ok
        public virtual void BeforeDelete(int id)
        {



        }
        public virtual void checkforexceptions(IEnumerable<TInsert> lista)
        { 
        
        
        
        }




    }
}