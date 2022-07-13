
﻿using AutoMapper;
using Dapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.NPGSQL.Interface;
using Microsoft.Extensions.Configuration;

using Npgsql;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.NPGSQL.Service
{
    public class CompetitionNPGSQLService :
        BCrudNPGSQLService<CompetitionModel, competition, CompetitionSearchObject, CompetitionInsertRequest, CompetitionUpsertRequest, CompetitionModelLess>
        , ICompetitionNPGSQL
    {

        public CompetitionNPGSQLService(IConfiguration Service1, IMapper Service3) : base(Service1, Service3)
        {
            PrimaryKey = "id";
            ForeignKey = "countryid";
            Conflictinsert = PrimaryKey;
            ConflictUpsert = PrimaryKey;
        }


        //Get funkcije
        public CompetitionModelLess GetIdbyNazivAsync(string name)
        {
            try
            {

                string Query = null;
                string typeParameterType = typeof(competition).Name;

                string TableName = typeParameterType.Any(char.IsUpper) ? $@"""{typeParameterType}""" : typeParameterType;

                Query += $@"select *  from ""BettingSystem"".{TableName} ";

                Query += $@"where naziv = '{name}'; ";

                using var conn = new NpgsqlConnection(connString);
                conn.OpenAsync();

                var quary = conn.Query<CompetitionModelLess>(Query).FirstOrDefault();


                if (Query == null)
                {

                    return new CompetitionModelLess { Id = 0 };

                }



                //var entity = quary.FirstOrDefault();

                return quary;

            }
            catch (Exception e)
            {

                throw;
            }







        }







        //Get Ekstenzije
        public override string AddFilter(string query, CompetitionSearchObject search = null)
        {

            List<string> ListaSearch = new List<string>();
            List<string> ListaValues = new List<string>();
            List<string> ListaType = new List<string>();

            if (!string.IsNullOrWhiteSpace(search?.naziv))
            {
                ListaSearch.Add("naziv");
                ListaValues.Add(search.naziv);
                ListaType.Add("S");


            }

            if (search?.countryid != null)
            {
                ListaSearch.Add("countryid");
                ListaValues.Add(search.countryid.ToString());
                ListaType.Add("I");


            }

            if (search.sportid != null)
            {
                ListaSearch.Add("sportid");
                ListaValues.Add(search.sportid.ToString());
                ListaType.Add("I");


            }
            if (search.id != null)
            {
                ListaSearch.Add("id");
                ListaValues.Add(search.id.ToString());
                ListaType.Add("I");


            }




            if (ListaSearch.Count() != 0)
            {
                query += "where ";
            }


            for (int i = 0; i < ListaSearch.Count(); i++)
            {
                if (ListaType[i] == "S")
                {
                    query += $@"(lower({ListaSearch[i]}) LIKE lower('%{ListaValues[i]}%')) ";

                }
                else
                {

                    query += $@"{ListaSearch[i]} = {ListaValues[i]} ";

                }
                if ((i + 1) != ListaSearch.Count())
                {

                    query += "or ";

                }


            }

            return query;


        }


        //insert Esktenzije 
        public override void BeforeInsertVoid(CompetitionInsertRequest insert, int Id = 0)
        {



            string QuerySport = null;
            string QueryCountries = null;


            QuerySport += $@"select *  from ""BettingSystem"".sport ";
            QuerySport += $@"where ""SportsId"" = {insert.sportid}; ";

            QueryCountries += $@"select *  from ""BettingSystem"".""Country"" ";
            QueryCountries += $@"where ""CountryId"" = {insert.countryid}; ";

            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();

            var sportpostoji = conn.Query<sport>(QuerySport).FirstOrDefault();
            var countrypostoji = conn.Query<Country>(QueryCountries).FirstOrDefault();


            //ako id posotoji a ne postoje fk
            //id postojati za to 
            //

            // uslov postojanja insert.id , entry u bazi  da bi proslijedio foreign key null
            // uslov postojanja insert.id , entry u bazi i foriegn key koji postoji

            //slucaj u kojem samo naziv             //baca error
            //slucaj u kojem id ,naziv              //prođe edit samo naziv
            //slucaj u kojem id,naziv i foreign key //provjerava foreign keyove       
            //slucaj u kojem foreign key samo       //provjereava 

            if (Id == 0 && insert.countryid == 0 && insert.sportid == 0)
            {

                throw new Exception($"foreign keyovi nisu navedeni ");


            }
            if (Id != 0 && (insert.sportid != 0 || insert.countryid != 0))
            {

                if (sportpostoji == null && insert.sportid != 0)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {insert.sportid} ne postoji ");


                }

                if (countrypostoji == null && insert.countryid != 0)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom Country jer Country sa CountryID {insert.countryid} ne postoji ");


                }

            }
            if (insert.sportid != 0 && insert.countryid != 0)
            {

                if (sportpostoji == null)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {insert.sportid} ne postoji ");


                }

                if (countrypostoji == null)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom Country jer Country sa CountryID {insert.countryid} ne postoji ");


                }



            }


















        }

        //upsert ekstenzije 

        public override List<CompetitionUpsertRequest> BeforeInsertFilterList(IEnumerable<CompetitionUpsertRequest> List)
        {

            //uslovi 
            //ako nije unjet id napravi id uslov da foreign keyovi postoje
            //id postoji ako foreign key postoji provjeri vljanost ako ne postoji uzmi iz baze


                
            List<CompetitionUpsertRequest> OutputList = new List<CompetitionUpsertRequest>();

            foreach (var item in List)
            {
                CompetitionUpsertRequest competition = null;

                FKprovjeriexception(item).Wait();  //provjerava da li je korisnik unjeo pravilno FK

                if (item.id == 0)
                {
               
                    var insert = Mapper.Map<CompetitionInsertRequest>(item);

                    var result = InsertAsync(insert).Result;

                    item.id = result.Id; //dodaj id za upsert

                    OutputList.Add(item);
                }
                else
                {

                    competition = foreignkeyfix(item); //popravlja fk ako korisik nije unjeo fk

                    OutputList.Add(competition);



                }

              
               

            }
           

            return OutputList;
        }

        public async Task FKprovjeriexception(CompetitionUpsertRequest Competition)
        {
            string QuerySport = null;
            string QueryCountries = null;


            QuerySport += $@"select *  from ""BettingSystem"".sport ";
            QuerySport += $@"where ""SportsId"" = {Competition.sportid}; ";

            QueryCountries += $@"select *  from ""BettingSystem"".""Country"" ";
            QueryCountries += $@"where ""CountryId"" = {Competition.countryid}; ";

            using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var sportpostoji = conn.Query<sport>(QuerySport).FirstOrDefault();
            var countrypostoji = conn.Query<Country>(QueryCountries).FirstOrDefault();


            //ako id posotoji a ne postoje fk
            //id postojati za to 
            //

            // uslov postojanja insert.id , entry u bazi  da bi proslijedio foreign key null
            // uslov postojanja insert.id , entry u bazi i foriegn key koji postoji

            //slucaj u kojem samo naziv             //baca error
            //slucaj u kojem id ,naziv              //prođe edit samo naziv
            //slucaj u kojem id,naziv i foreign key //provjerava foreign keyove       
            //slucaj u kojem foreign key samo       //provjereava 

            if (Competition.id == 0 && Competition.countryid == 0 && Competition.sportid == 0)
            {

                throw new Exception($"foreign keyovi nisu navedeni ");


            }
            if (Competition.id != 0 && (Competition.sportid != 0 || Competition.countryid != 0))
            {

                if (sportpostoji == null && Competition.sportid != 0)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {Competition.sportid} ne postoji ");


                }

                if (countrypostoji == null && Competition.countryid != 0)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom Country jer Country sa CountryID {Competition.countryid} ne postoji ");


                }

            }
            if (Competition.sportid != 0 && Competition.countryid != 0)
            {

                if (sportpostoji == null)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {Competition.sportid} ne postoji ");


                }

                if (countrypostoji == null)
                {

                    throw new Exception($"Nije moguce napraviti vezu sa tabelom Country jer Country sa CountryID {Competition.countryid} ne postoji ");


                }



            }

        }


        public override CompetitionInsertRequest foreignkeyfix(int Id, CompetitionInsertRequest insert)
        {

            if (Id == 0)
            {

                return insert;
            
            }

            string QueryCompetition = null;



            QueryCompetition += $@"select *  from ""BettingSystem"".competition ";
            QueryCompetition += $@"where id = {Id}; ";   

            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();

            
            var competition = conn.Query<competition>(QueryCompetition).FirstOrDefault();

            insert.countryid = insert.countryid==0?competition.countryid:insert.countryid;
            insert.sportid =  insert.sportid == 0 ? competition.sportid.Value : insert.sportid;

            return insert;



        }

        public  CompetitionUpsertRequest foreignkeyfix(CompetitionUpsertRequest insert)
        {

            if (insert.id == 0)
            {

                return insert;

            }

            string QueryCompetition = null;



            QueryCompetition += $@"select *  from ""BettingSystem"".competition ";
            QueryCompetition += $@"where id = {insert.id}; ";

            using var conn = new NpgsqlConnection(connString);
            conn.OpenAsync();


            var competition = conn.Query<competition>(QueryCompetition).FirstOrDefault();

            insert.countryid = insert.countryid == 0 ? competition.countryid : insert.countryid;
            insert.sportid = insert.sportid == 0 ? competition.sportid.Value : insert.sportid;

            return insert;



        }













    }
}

