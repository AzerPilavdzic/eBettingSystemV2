using AutoMapper;
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
        static readonly HtmlDocument document = web.Load("https://m.rezultati.com/");

        public string _sportName { get; set; }
        public int _sportID { get; set; }


        public CompetitionService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;


            GetSportName();

            //pozovi funkciju za dodavanje drzava
            AddCountries();
            Console.WriteLine(_sportName);

            //this was a test..
            //GetByObjectName("Compentecija32");
        }

        private void GetSportName()
        {
            var _sport = document.DocumentNode.SelectSingleNode("//*[@id='main']/h2").InnerText;
            var _regexSport = Regex.Match(_sport, "^\\S*").ToString();

            _sportName = _regexSport.ToString();

            //.....TEMPORARY.....

            var sportModel = Context.Sports.Where(x => x.Name.ToLower() == _sportName.ToLower()).FirstOrDefault();
            _sportID = sportModel.SportsId;



        }

        public void AddCountries()
        {
            var categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");
            List<string> listaKategorija = new List<string>();

            categories.ToList().ForEach(i => listaKategorija.Add(i.InnerText));

            for (int i = 0; i < listaKategorija.Count(); i++)
            {
                var _match = Regex.Match(listaKategorija[i], @"(\w*)(?=:)");
                listaKategorija[i] = _match.ToString();
            }

            HashSet<string> CategoryHashSet = new HashSet<string>();


            foreach (var item in listaKategorija)
            {
                CategoryHashSet.Add(item);
            }


            var _listaKategorijaFinal = CategoryHashSet.ToList();

            for (int i = 0; i < _listaKategorijaFinal.Count(); i++)
            {
                //provjeriti u bazi da li postoji _listaKategorijaFinal[i]
                //CheckCountryExist(_listaKategorijaFinal[i]);
                if (GetByObjectName(_listaKategorijaFinal[i]) != null)
                {
                    continue;
                }
                else
                {
                    //prebaciti ovaj dio koda u UpsertOneOrMoreCountries
                    Context.Countries.Add(new Country()
                    {
                        CountryName = _listaKategorijaFinal[i]
                    });

                }

            }

            Context.SaveChanges();
        }


       



        //get esktenzije
        public override IQueryable<Competition> AddFilter(IQueryable<Competition> query, CompetitionSearchObject search = null)
        {
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




        public override Task<CompetitionModel> GetByObjectName(string name)
        {
            var _model = Context.Countries.Where(x => x.CountryName.ToLower() == name.ToLower()).FirstOrDefault();
            if (_model == null)
            {
                Console.WriteLine(name + $" ne postoji u bazi.\n");
                return null;
            }

            return base.GetByObjectName(name);
        }

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

        //..

    }
}
