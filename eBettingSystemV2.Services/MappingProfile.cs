using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
//using eProdaja.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //var o1 = new Object1();



            //models 
            CreateMap<Team, TeamModel>();
            CreateMap<TeamModel, Team>();

            CreateMap<Sport, SportModel>();
            CreateMap<SportModel, Sport>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<Team, TeamModelLess>();
            CreateMap<TeamModelLess, Team>();

            CreateMap<Sport, SportModelLess>();
            CreateMap<SportModelLess, Sport >();

            CreateMap<Country, CountryModelLess>();
            CreateMap<CountryModelLess, Country>();




            //requests
            CreateMap<Country, CountryUpsertRequest>();
            CreateMap<CountryUpsertRequest,Country>();

            CreateMap<Country, CountryInsertRequest>();
            CreateMap<CountryInsertRequest, Country>();

            CreateMap<Sport, SportUpsertRequest>();
            CreateMap<SportUpsertRequest, Sport>();

            CreateMap<Sport, SportInsertRequest>();
            CreateMap<SportInsertRequest, Sport>();

            CreateMap<Team, TeamUpsertRequest>();
            CreateMap<TeamUpsertRequest,Team>();

            CreateMap<TeamInsertRequest, Team>();
            CreateMap<Team,TeamInsertRequest>();



            //search
            CreateMap<Country, CountrySearchObject>();
            CreateMap<Team, TeamSearchObject>();
            CreateMap<Sport, SportSearchObject>();






        }
    }
}
