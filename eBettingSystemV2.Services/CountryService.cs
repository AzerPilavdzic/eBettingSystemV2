//using AutoMapper;
//using eBettingSystemV2.Model;
//using RS2_Tourism_Agency.Model.Request;
//using RS2_Tourism_Agency.Model.SearchObjects;
using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TourismAgency.Services.Database;

namespace eBettingSystemV2.Services
{
    public class CountryService :
        BaseCRUDService
        <
        CountryModel,
        Country,
        CountrySearchObject,
        CountryUpsertRequest,
        CountryUpsertRequest,
        object
        >,
        ICountryService
        
    {
        

        public CountryService(eBettingSystemV2.Services.Database.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;



        }


        //vraca model iz baze
        public override IQueryable<Country> AddFilter(IQueryable<Country> query, CountrySearchObject search = null)
        {
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.Naziv))
            {
                filterquery = filterquery.Where(X => X.CountryName.ToLower().StartsWith(search.Naziv.ToLower()));
            }

            if (search.CountryId != null)
            {
                filterquery = filterquery.Where(X => X.CountryId==search.CountryId);

            }

            return filterquery;

            //IQueryable<object> T = null;

            //return T;

        }

        public override CountryModel Insert(CountryUpsertRequest insert)
        {
            if (!BeforeInsertBool(insert))
            {
            throw new Exception("Drzava sa tim imenom vec postoji.");
            }
                return base.Insert(insert);

        }

        public override bool BeforeInsertBool(CountryUpsertRequest insert)
        {
            var entity = Context.Countries.Where(x=>x.CountryName.ToLower()==insert.CountryName.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: DRZAVA SA TIM IMENOM VEC POSTOJI.");
        }





    }
}

//inheriting base crud service 
//using Interface IDrzava 
//needs context and mapper intalized into crud
