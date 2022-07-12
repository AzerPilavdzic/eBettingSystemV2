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

            CreateMap<sport, SportModel>();
            CreateMap<SportModel, sport>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<teams, TeamModelLess>();
            CreateMap<TeamModelLess, teams>();

            CreateMap<sport, SportModelLess>();
            CreateMap<SportModelLess, sport >();

            CreateMap<Country, CountryModelLess>();
            CreateMap<CountryModelLess, Country>();


            CreateMap<competition, CompetitionModel>();
            CreateMap<CompetitionModel, competition>();

            CreateMap<competition, CompetitionModelLess>();
            CreateMap<CompetitionModelLess, competition>();

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


            CreateMap<CountryInsertRequest, CountryUpsertRequest>();
            CreateMap<CountryUpsertRequest, CountryInsertRequest>();

            CreateMap<sport, SportUpsertRequest>();
            CreateMap<SportUpsertRequest, sport>();

            CreateMap<sport, SportInsertRequest>();
            CreateMap<SportInsertRequest, sport>();

            CreateMap<teams, TeamUpsertRequest>();
            CreateMap<TeamUpsertRequest,teams>();

            CreateMap<TeamInsertRequest, teams>();
            CreateMap<teams,TeamInsertRequest>();

            CreateMap<competition, CompetitionUpsertRequest>();
            CreateMap<CompetitionUpsertRequest, competition>();

            CreateMap<competition, CompetitionInsertRequest>();
            CreateMap<CompetitionInsertRequest, competition>();

            CreateMap<Event, EventUpsertRequest>();
            CreateMap<EventUpsertRequest, Event>();

            CreateMap<Event, EventInsertRequest>();
            CreateMap<EventInsertRequest, Event>();



            //search
            CreateMap<Country, CountrySearchObject>();
            CreateMap<teams, TeamSearchObject>();
            CreateMap<sport, SportSearchObject>();
            CreateMap<competition, CompetitionSearchObject>();
            CreateMap<Event, EventSearchObject>();




        }
    }
}
