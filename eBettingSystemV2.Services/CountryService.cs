//using AutoMapper;
//using eBettingSystemV2.Model;
//using RS2_Tourism_Agency.Model.Request;
//using RS2_Tourism_Agency.Model.SearchObjects;
using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
using Microsoft.EntityFrameworkCore;
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
        CountryModel
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
                filterquery = filterquery.Where(x=>x.CountryName!=null)
                    .Where(X => X.CountryName.ToLower()
                    .StartsWith(search.Naziv.ToLower()));
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

        public override IEnumerable<Country> AddRange(IEnumerable<CountryUpsertRequest> insertlist, DbSet<Country> set)
        {
            int takenumber = insertlist.Count(); //koliko ih treba uzeti
            int addnumber = 0;  //koliko ih treba dodati
            var list = insertlist.ToList(); //konverzija

            if (takenumber > set.Count())
            {
                addnumber = set.Count() - takenumber;
                set.AddRange(Mapper.Map<IEnumerable<Country>>(list.Skip(takenumber - addnumber)));
                takenumber = set.Count();

            }

            var SetMini = set.Take(takenumber);

            int i = 0;




            foreach (var a in SetMini)
            {

                if (a.CountryName != list[i].CountryName)
                {
                    //a.name = list[i].name;
                    set.Find(a.CountryId).CountryName = list[i].CountryName;
                }
                i++;


            }







            IEnumerable<Country> entity = Mapper.Map<IEnumerable<Country>>(SetMini);
            //set.AddRange(entity);

            return entity;
        }

        public override bool checkIfNameSame(CountryUpsertRequest insert, Country entry)
        {
            if (insert.CountryName == entry?.CountryName)
            {

                return true;


            }
            return false;
        }


        public override void BeforeDelete(int id)
        {
            var entry = Context.Teams.Where(X=>X.Countryid==id).FirstOrDefault();

            if (entry != null)
            {

                throw new Exception("Team got a relation with the Country you want to Delete");
               
            
            }






        }




    }
}

//inheriting base crud service 
//using Interface IDrzava 
//needs context and mapper intalized into crud
