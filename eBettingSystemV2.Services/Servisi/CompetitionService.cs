using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
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

        public void AddCompetitions()
        {
            //Context.Competitions.Add(new Competition()
            //{                       
            //    Sportid=_sportID,
            //    //Naziv=//iz fora
            //});
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
    }
}
