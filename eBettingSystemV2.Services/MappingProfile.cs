using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
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
            CreateMap<teams, TeamModel>();
            CreateMap<TeamModel, teams>();

            CreateMap<Sport, SportModel>();
            CreateMap<SportModel, Sport>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<teams, TeamModelLess>();
            CreateMap<TeamModelLess, teams>();

            CreateMap<Sport, SportModelLess>();
            CreateMap<SportModelLess, Sport >();

            CreateMap<Country, CountryModelLess>();
            CreateMap<CountryModelLess, Country>();


            CreateMap<Competition, CompetitionModel>();
            CreateMap<CompetitionModel, Competition>();

            CreateMap<Competition, CompetitionModelLess>();
            CreateMap<CompetitionModelLess, Competition>();

            CreateMap<CompetitionModel, CompetitionModelLess>();
            CreateMap<CompetitionModelLess, CompetitionModel>();


            CreateMap<Event, EventModel>();
            CreateMap<EventModel, Event>();

            CreateMap<Event, EventModelLess>();
            CreateMap<EventModelLess, Event>();

            CreateMap<EventModel, EventModelLess>();
            CreateMap<EventModelLess, EventModel>();



            //requests
            CreateMap<Country, CountryUpsertRequest>();
            CreateMap<CountryUpsertRequest,Country>();

            CreateMap<Country, CountryInsertRequest>();
            CreateMap<CountryInsertRequest, Country>();

            CreateMap<Sport, SportUpsertRequest>();
            CreateMap<SportUpsertRequest, Sport>();

            CreateMap<Sport, SportInsertRequest>();
            CreateMap<SportInsertRequest, Sport>();

            CreateMap<teams, TeamUpsertRequest>();
            CreateMap<TeamUpsertRequest,teams>();

            CreateMap<TeamInsertRequest, teams>();
            CreateMap<teams,TeamInsertRequest>();

            CreateMap<Competition, CompetitionUpsertRequest>();
            CreateMap<CompetitionUpsertRequest, Competition>();

            CreateMap<Competition, CompetitionInsertRequest>();
            CreateMap<CompetitionInsertRequest, Competition>();

            CreateMap<Event, EventUpsertRequest>();
            CreateMap<EventUpsertRequest, Event>();

            CreateMap<Event, EventInsertRequest>();
            CreateMap<EventInsertRequest, Event>();



            //search
            CreateMap<Country, CountrySearchObject>();
            CreateMap<teams, TeamSearchObject>();
            CreateMap<Sport, SportSearchObject>();
            CreateMap<Competition, CompetitionSearchObject>();
            CreateMap<Event, EventSearchObject>();



        }
    }
}
