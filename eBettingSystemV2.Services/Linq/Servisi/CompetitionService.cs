﻿using AutoMapper;
using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using eBettingSystemV2.Services.Linq.Servisi;
using eBettingSystemV2.Services.Linq.Interface;

namespace eBettingSystemV2.Services.Linq.Servisi
{
    public class CompetitionService : BaseCRUDService
       <
       CompetitionModel,
       competition,
       CompetitionSearchObject,
       CompetitionInsertRequest,
       CompetitionUpsertRequest,
       CompetitionModelLess
       >,
       ICompetitionService
    {

        static readonly HtmlWeb web = new HtmlWeb();   //avoid hardcode
        static readonly HtmlDocument document = web.Load("https://m.rezultati.com/"); //questionable code
        private ICountryService ICountryService { get; set; }
        private ISportService ISportService { get; set; }
        //private ICompetitionService ICompetitionService { get; set; }
        //private IFetch IFetchService { get; set;}

      




        HtmlNodeCollection categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4"); //questionable code

        public CompetitionService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_,
            ICountryService service1,
            ISportService service2
            //IFetch service3
            ) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;
            ICountryService = service1;
            ISportService = service2;
            //IFetchService = service3;
           



        }

      
        //demo klasa

        public async void AddData(/*List<PodaciSaStranice> Podaci*/)
        {

            List<PodaciSaStranice> Podaci = new List<PodaciSaStranice>();

            Podaci.Add(new PodaciSaStranice
            {
                Competitionname="Kompentecija 1",
                Country="EUROPA",
                Sport="",


            }
            );

            List<competition> competitions = new List<competition>();
            
                foreach (var b in Podaci)
                {
                    var x = new competition
                    {


                    naziv = b.Competitionname,           //competencija1
                    id = GetIdbyName(b.Competitionname),  //45
                    sportid = GetIdbyName(b.Sport),        //kosarka 5  ako ne postoji doda i onda vrati id
                    countryid = GetIdbyName(b.Country)     //country ukraine 5


                    };

                   if (x.countryid == 0)
                   {


                      var insert = new CountryInsertRequest
                      {
                          CountryName = b.Country

                      };

                      var model = await ICountryService.InsertAsync(insert);
                      x.countryid = model.CountryId;

                   }




                    competitions.Add(x);

                }









            var sportoviq = Mapper.Map<IQueryable<CompetitionUpsertRequest>>(competitions);

            await UpsertOneOrMoreAsync(sportoviq);
            
            //45 
            //naci ce competition sa id 45 
            //promjeniti ce naziv i oba foreign key na odgovarajucu vezu bez eceptions




          
        } //demo
        public int GetIdbyName(string name)
        {

            var postoji = Context.Teams.Where(X => X.teamname == name).FirstOrDefault();

            if (postoji != null)
            {

                return postoji.teamid;


            }
            else
            {

                var nova = new teams { teamname = name, teamid = 0 };


                Context.Teams.Add(nova);

                Context.SaveChanges();

                return nova.teamid;
          
            
            }



        
        
        } //demo

        


        //get esktenzije
        public override IQueryable<competition> AddFilter(IQueryable<competition> query, CompetitionSearchObject search = null)
        {
            
            

            //Context.SaveChanges();
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.naziv))
            {
                filterquery = filterquery.Where(x => x.naziv != null)
                    .Where(X => X.naziv.ToLower()
                    .StartsWith(search.naziv.ToLower()));
            }

            if (search.id != null)
            {
                filterquery = filterquery.Where(X => X.id == search.id);

            }

            if (search.countryid != null)
            {
                filterquery = filterquery.Where(X => X.countryid == search.countryid);

            }

            if (search.sportid != null)
            {
                filterquery = filterquery.Where(X => X.sportid == search.sportid);

            }




            return filterquery;





        }

        public CompetitionModelLess GetIdbyNazivAsync(string name)
        {
            try
            {

                var _model = Context.Competitions
                    .Where(x => x.naziv.ToLower() == name.ToLower())
                    .FirstOrDefault();

                if (_model == null)
                {

                    //throw new Exception($"Competition sa imenom {name} ne postoji u bazi");
                    return new CompetitionModelLess { Id = 0 };
                }

                return Mapper.Map<CompetitionModelLess>(_model);

            }
            catch (Exception e)
            {

                throw;
            }


            




        }

        public override Task<CompetitionModel> GetByObjectName(string name)
        {
            var _model = Context.Countries.Where(x => x.CountryName.ToLower() == name.ToLower()).FirstOrDefault();
            if (_model == null)
            {
                Console.WriteLine(name + $" ne postoji u bazi.\n");
                return null;
            }

            return base.GetByObjectName(name);
        } //demo klasa



        //insert Ekstenzije
        public override void BeforeInsertVoid(CompetitionInsertRequest insert)
        {
            var sportpostoji = Context.Sports.Find(insert.sportid);
            var countrypostoji = Context.Countries.Find(insert.countryid);


            if(sportpostoji == null)
            {

                throw new Exception($"Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {insert.sportid} ne postoji ");

                   
            }

            if (countrypostoji == null)
            {

                throw new Exception($"Nije moguce napraviti vezu sa tabelom Country jer Country sa CountryID {insert.countryid} ne postoji ");


            }











        }
        public override IEnumerable<competition> AddRange(IEnumerable<CompetitionUpsertRequest> insertlist, DbSet<competition> set)
        {
            
            
            List<competition> Result = new List<competition>();
            competition aa = null;

            //provjerava exceptione
            foreach (var a in insertlist)                                               
            {
                var sportpostoji = Context.Sports.Find(a.sportid);
                var countrypostoji = Context.Countries.Find(a.countryid);
                var CompetitionNameExist = Context.Competitions.Where(x=>x.naziv ==a.naziv).Select(x=>x.naziv).FirstOrDefault();

                if (sportpostoji == null)
                {
                    throw new Exception($"Competition {a.naziv} Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {a.sportid} ne postoji ");

                }
                if (countrypostoji == null)
                {
                    throw new Exception($"Competition {a.naziv} Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {a.countryid} ne postoji ");
                }                                           

                if (CompetitionNameExist != null && a.id==0)
                {

                    throw new Exception($"Competition vec postoji");
                }

            }



            foreach (var a in insertlist)
            {
                if (a.id == 0)
                {

                    aa = Mapper.Map<competition>(a);
                    //dodaj u bazu
                    set.Add(aa);
                    //dodaj u result
                    Context.SaveChanges();

                    Result.Add(aa);
                    continue;

                }


                var entry = set.Find(a.id);

                if (entry != null)
                {

                    entry.naziv = a.naziv;
                    entry.countryid = a.countryid;
                    entry.sportid = a.sportid;

                    Result.Add(Mapper.Map<competition>(entry));

                }
                else
                {
                    if (a.id != 0)
                    {

                        a.id = 0;

                    }

                    aa = Mapper.Map<competition>(a);

                    set.Add(aa);
                    Context.SaveChanges();


                    Result.Add(Mapper.Map<competition>(aa));

                }





            }


            IEnumerable<competition> entity = Mapper.Map<IEnumerable<competition>>(Result);
            //set.AddRange(entity);

            return entity;




            
        }

        public async Task<List<CompetitionModel>> AddDataAsync(List<PodaciSaStranice> Podaci)
        {
            try
            {



                //lista koja ce biti poslana u InsertOneOrMoreAsync
                List<competition> competitions = new List<competition>();

                foreach (var b in Podaci)
                {
                    //geta id by name
                    

                    var Sport = await ISportService.GetSportIdbyNameAsync(b.Sport);
                    var Competition = GetIdbyNazivAsync(b.Competitionname);
                    var Country = await ICountryService.GetIdByNameAsync(b.Country);

                    //ako id 0 dodaj i uzmi id
                    if (Sport.SportsId == 0)
                    {
                        Sport = await ISportService.InsertAsync(new SportInsertRequest
                        {
                            name = b.Sport
                        });

                    };

                    if (Country.CountryId == 0)
                    {
                        Country = await ICountryService.InsertAsync(new CountryInsertRequest
                        {

                            CountryName = b.Country

                        });
                    }

                    var x = new competition
                    {

                        //kad dobijemo sve id pohranjujemo u competition
                        naziv = b.Competitionname,           //competencija1
                        id = Competition.Id,  //45
                        sportid = Sport.SportsId,        //kosarka 5  ako ne postoji doda i onda vrati id
                        countryid = Country.CountryId,     //country ukraine 5


                    };

                    //dodajemo u listu
                    competitions.Add(x);

                }


                //convertujemo listu tako da je mozemo koristiti u insertoneormore async
                var list = Mapper.Map<IEnumerable<CompetitionUpsertRequest>>(competitions);

                //pokrecemo pohranu podataka
                var result = await UpsertOneOrMoreAsync(list);


                //competitions = Mapper.Map<List<Competition>>(result);

                //ako kompetition sadrzi 0 onda mjenjamo id sa id iz result
                if (competitions.Where(X => X.id == 0).FirstOrDefault() != null)
                {

                    foreach (var a in competitions)
                    {
                        foreach (var b in result)
                        {
                            if (a.id == 0)
                            {
                                a.id = b.Id;

                            }





                        }


                    }
                }






                return Mapper.Map<List<CompetitionModel>>(competitions);



                //convert listu 
                //InsertOneOrMoreAsync(lista);
                //vrati rezultat
                //var sportoviq = Mapper.Map<List<CompetitionUpsertRequest>>(competitions);

            }
            
            catch(Exception ex) 
            {



                return null;
            
            
            }




        }

        //..
        





    }
}
