//using AutoMapper;
//using eBettingSystemV2.Model;
//using RS2_Tourism_Agency.Model.Request;
//using RS2_Tourism_Agency.Model.SearchObjects;
using AutoMapper;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
using eProdaja.Services;
using RS2_Tourism_Agency.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TourismAgency.Services.Database;

namespace TourismAgency.Services
{
    public class DrzavaService :
        BaseCRUDService
        <
        CountryModel,
        Country,
        CountrySearchObject,
        object,
        object
        >
        //ICountryService
    {
        public DrzavaService(eBettingSystemV2.Services.Database.BettingSystemContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;



        }


        //vraca model iz baze
        public override IQueryable<Country> AddFilter(IQueryable<Country> query, CountrySearchObject search = null)
        {
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.NazivGT))
            {

                filterquery = filterquery.Where(X => X.Country1.StartsWith(search.NazivGT));


            }
            return filterquery;

            //IQueryable<object> T = null;

            //return T;

        }






    }
}

//inheriting base crud service 
//using Interface IDrzava 
//needs context and mapper intalized into crud
