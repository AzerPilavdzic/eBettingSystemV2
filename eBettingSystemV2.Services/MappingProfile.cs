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

            //models 
            CreateMap<Team, TeamModel>();
            CreateMap<Country, CountryModel>();

            //requests
            CreateMap<Country, CountryUpsertRequest>();
            CreateMap<Team, TeamUpsertRequest>();


            //search
            CreateMap<Country, CountrySearchObject>();
            CreateMap<Team, TeamSearchObject>();





        }
    }
}
