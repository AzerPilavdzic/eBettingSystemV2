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
            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            //requests
            CreateMap<Country, CountryUpsertRequest>();
            CreateMap<CountryUpsertRequest,Country>();
            CreateMap<Team, TeamUpsertRequest>();
            CreateMap<TeamUpsertRequest,Team>();


            //search
            CreateMap<Country, CountrySearchObject>();
            CreateMap<Team, TeamSearchObject>();





        }
    }
}
