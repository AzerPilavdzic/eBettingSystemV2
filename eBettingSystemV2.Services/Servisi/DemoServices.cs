using AutoMapper;
using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{
    public class DemoServices :IDemo
    {
        public praksa_dbContext Context { get; set; }
        public IMapper Mapper { get; set; }

        private ICountryService ICountryService { get; set; }
        private ISportService ISportService { get; set; }
        private ICompetitionService ICompetitionService { get; set; }

        public DemoServices(praksa_dbContext context,
            IMapper mapper,
            ICountryService service1,
            ISportService service2,
            ICompetitionService service3)
        {
            Context = context;
            Mapper = mapper;
            ICountryService = service1;
            ISportService = service2;
            ICompetitionService = service3;
        }


        public async Task<List<CompetitionModel>> AddDataAsync(List<PodaciSaStranice> Podaci)
        {
            
            List<Competition> competitions = new List<Competition>();

            foreach (var b in Podaci)
            {

                var Competition = await ICompetitionService.GetIdbyNazivAsync(b.Competitionname);
                var Sport = await ISportService.GetSportIdbyNameAsync(b.Sport);
                var Country = await ICountryService.GetIdByNameAsync(b.Country);


                if (Sport.SportsId == 0)
                {
                    Sport = await ISportService.InsertAsync(new SportInsertRequest
                    {
                        name = b.Sport
                    });
                                 
                };

                if(Country.CountryId==0)
                {
                    Country = await ICountryService.InsertAsync(new CountryInsertRequest
                    {

                        CountryName = b.Country

                    });
                }

                var x = new Competition
                {


                    Naziv = b.Competitionname,           //competencija1
                    Id = Competition.Id,  //45
                    Sportid = Sport.SportsId,        //kosarka 5  ako ne postoji doda i onda vrati id
                    Countryid = Country.CountryId,     //country ukraine 5


                };

               
                competitions.Add(x);

            }
            return Mapper.Map<List<CompetitionModel>>(competitions);

            //convert listu 
            //InsertOneOrMoreAsync(lista);
            //vrati rezultat





            //var sportoviq = Mapper.Map<List<CompetitionUpsertRequest>>(competitions);





        }





























    }
}
