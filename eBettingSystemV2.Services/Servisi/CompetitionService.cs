using AutoMapper;
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

namespace eBettingSystemV2.Services.Servisi
{
    public class CompetitionService : BaseCRUDService
       <
       CompetitionModel,
       Competition,
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
        private IFetch IFetchService { get; set;}

        private ICache ICacheService { get; set;}




        HtmlNodeCollection categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4"); //questionable code

        public CompetitionService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_,
            ICountryService service1,
            ISportService service2,
            IFetch service3,
            ICache service4
            ) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;
            ICountryService = service1;
            ISportService = service2;
            IFetchService = service3;
            ICacheService = service4;



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

            List<Competition> competitions = new List<Competition>();
            
                foreach (var b in Podaci)
                {
                    var x = new Competition
                    {


                    Naziv = b.Competitionname,           //competencija1
                    Id = GetIdbyName(b.Competitionname),  //45
                    Sportid = GetIdbyName(b.Sport),        //kosarka 5  ako ne postoji doda i onda vrati id
                    Countryid = GetIdbyName(b.Country)     //country ukraine 5


                    };

                   if (x.Countryid == 0)
                   {


                      var insert = new CountryInsertRequest
                      {
                          CountryName = b.Country

                      };

                      var model = await ICountryService.InsertAsync(insert);
                      x.Countryid = model.CountryId;

                   }




                    competitions.Add(x);

                }









            var sportoviq = Mapper.Map<IQueryable<CompetitionUpsertRequest>>(competitions);

            InsertOneOrMoreAsync(sportoviq);
            
            //45 
            //naci ce competition sa id 45 
            //promjeniti ce naziv i oba foreign key na odgovarajucu vezu bez eceptions




          
        } //demo
        public int GetIdbyName(string name)
        {

            var postoji = Context.Teams.Where(X => X.Teamname == name).FirstOrDefault();

            if (postoji != null)
            {

                return postoji.Teamid;


            }
            else
            {

                var nova = new Team { Teamname = name, Teamid = 0 };


                Context.Teams.Add(nova);

                Context.SaveChanges();

                return nova.Teamid;
          
            
            }



        
        
        } //demo

        


        //get esktenzije
        public override IQueryable<Competition> AddFilter(IQueryable<Competition> query, CompetitionSearchObject search = null)
        {
            //uzmi html od competitiona(unutar njega je drzava i naziv competitiona);
            //dodao je sve drzave prije toga
            //lokalna varijabla koja cuva drzavaID, mijenja se svakom narednom iteracijom
            //sportID property
            var _categories = categories.ToList();
            List<string> listCompetition = new List<string>();


            categories.ToList().ForEach(i => listCompetition.Add(i.InnerText));

            for (int i = 0; i < listCompetition.Count(); i++)
            {

                int drzavaID = 69;
                var _competitionName = Regex.Match(listCompetition[i], @"(?<=: )(?:(?! -).)*");
                listCompetition[i] = _competitionName.ToString();


                Context.Competitions.Add(new Competition()
                {
                    //Sportid = _sportID,
                    Countryid = drzavaID,
                    Naziv = listCompetition[i],
                });


                Console.WriteLine("Drzava ID " + drzavaID + " natjecanje: " + listCompetition[i]);

            }

            //Context.SaveChanges();
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.naziv))
            {
                filterquery = filterquery.Where(x => x.Naziv != null)
                    .Where(X => X.Naziv.ToLower()
                    .StartsWith(search.naziv.ToLower()));
            }

            if (search.id != null)
            {
                filterquery = filterquery.Where(X => X.Id == search.id);

            }

            if (search.countryid != null)
            {
                filterquery = filterquery.Where(X => X.Countryid == search.countryid);

            }

            if (search.sportid != null)
            {
                filterquery = filterquery.Where(X => X.Sportid == search.sportid);

            }




            return filterquery;





        }

        public async Task<CompetitionModelLess> GetIdbyNazivAsync(string name)
        {
            var _model = await Context.Competitions
                .Where(x => x.Naziv.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            if (_model == null)
            {

                //throw new Exception($"Competition sa imenom {name} ne postoji u bazi");
                return new CompetitionModelLess { Id = 0 };
            }

            return Mapper.Map<CompetitionModelLess>(_model);




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
            var sportpostoji = Context.Sports.Find(insert.Sportid);
            var countrypostoji = Context.Countries.Find(insert.Countryid);


            if(sportpostoji == null)
            {

                throw new Exception($"Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {insert.Sportid} ne postoji ");

                   
            }

            if (countrypostoji == null)
            {

                throw new Exception($"Nije moguce napraviti vezu sa tabelom Country jer Country sa CountryID {insert.Countryid} ne postoji ");


            }











        }
        public override IEnumerable<Competition> AddRange(IEnumerable<CompetitionUpsertRequest> insertlist, DbSet<Competition> set)
        {
            
            
            List<Competition> Result = new List<Competition>();
            Competition aa = null;

            //provjerava exceptione
            foreach (var a in insertlist)
            {
                var sportpostoji = Context.Sports.Find(a.sportid);
                var countrypostoji = Context.Countries.Find(a.countryid);

                if(sportpostoji == null)
                {
                    throw new Exception ($"Competition {a.naziv} Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {a.sportid} ne postoji ");

                }
                if (countrypostoji == null)
                {
                    throw new Exception($"Competition {a.naziv} Nije moguce napraviti vezu sa tabelom sport jer Sport sa sportsID {a.countryid} ne postoji ");
                }

            }


            foreach (var a in insertlist)
            {
                if (a.id == 0)
                {

                    aa = Mapper.Map<Competition>(a);
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

                    entry.Naziv = a.naziv;
                    entry.Countryid = a.countryid;
                    entry.Sportid = a.sportid;

                    Result.Add(Mapper.Map<Competition>(entry));

                }
                else
                {
                    if (a.id != 0)
                    {

                        a.id = 0;

                    }

                    aa = Mapper.Map<Competition>(a);

                    set.Add(aa);
                    Context.SaveChanges();


                    Result.Add(Mapper.Map<Competition>(aa));

                }





            }


            IEnumerable<Competition> entity = Mapper.Map<IEnumerable<Competition>>(Result);
            //set.AddRange(entity);

            return entity;




            
        }

        public async Task<List<CompetitionModel>> AddDataAsync(List<PodaciSaStranice> Podaci)
        {
            //lista koja ce biti poslana u InsertOneOrMoreAsync
            List<Competition> competitions = new List<Competition>();

            foreach (var b in Podaci)
            {
                //geta id by name
                var Competition = await GetIdbyNazivAsync(b.Competitionname);
                var Sport = await ISportService.GetSportIdbyNameAsync(b.Sport);
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

                var x = new Competition
                {

                    //kad dobijemo sve id pohranjujemo u competition
                    Naziv = b.Competitionname,           //competencija1
                    Id = Competition.Id,  //45
                    Sportid = Sport.SportsId,        //kosarka 5  ako ne postoji doda i onda vrati id
                    Countryid = Country.CountryId,     //country ukraine 5


                };

                //dodajemo u listu
                competitions.Add(x);

            }


            //convertujemo listu tako da je mozemo koristiti u insertoneormore async
            var list = Mapper.Map<IEnumerable<CompetitionUpsertRequest>>(competitions);

            //pokrecemo pohranu podataka
            var result = await InsertOneOrMoreAsync(list);


            //competitions = Mapper.Map<List<Competition>>(result);

            //ako kompetition sadrzi 0 onda mjenjamo id sa id iz result
            if (competitions.Where(X => X.Id == 0).FirstOrDefault() != null)
            {

                foreach (var a in competitions)
                {
                    foreach (var b in result)
                    {
                        if (a.Id == 0)
                        {
                            a.Id = b.Id;

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

        //..

        public async void FetchStoreCacheCompetition()
        {
            

            List<PodaciSaStranice> Lista =  IFetchService.FetchSportAndData();

           
            Func<Task<List<CompetitionModel>>> a = ( ) => { return AddDataAsync(Lista); };


            var Lista2 = await ICacheService.SetCacheCompetition(Lista, a);











        
        
        
        
        }





    }
}
