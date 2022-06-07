//using AutoMapper;
//using eBettingSystemV2.Model;
//using RS2_Tourism_Agency.Model.Request;
//using RS2_Tourism_Agency.Model.SearchObjects;
using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
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
        CountryInsertRequest,
        CountryUpsertRequest,
        CountryModelLess
        >,
        ICountryService
        
    {
        

        public CountryService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
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

        public override CountryModelLess Insert(CountryInsertRequest insert)
        {
            if (!BeforeInsertBool(insert))
            {
            throw new Exception("Drzava sa tim imenom vec postoji.");
            }
                return base.Insert(insert);

        }

        public override bool BeforeInsertBool(CountryInsertRequest insert)
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
            List<Country> Result = new List<Country>();
            Country aa = null;




            foreach (var a in insertlist)
            {
                if (Context.Countries.Where(X=>X.CountryName==a.CountryName).FirstOrDefault()!=null)
                {

                    throw new Exception($"Entry with the name {a.CountryName} already exist in the database");
                
                }


                if (a.CountryId == 0)
                {

                    aa = Mapper.Map<Country>(a);
                    //dodaj u bazu
                    set.Add(aa);
                    //dodaj u result
                    Context.SaveChanges();

                    Result.Add(aa);
                    continue;

                }

                var entry = set.Find(a.CountryId);

                if (entry != null)
                {

                    entry.CountryName = a.CountryName;

                    Result.Add(Mapper.Map<Country>(entry));

                }
                else
                {
                    if (a.CountryId != 0)
                    {

                        a.CountryId = 0;

                    }

                    aa = Mapper.Map<Country>(a);

                    set.Add(aa);
                    Context.SaveChanges();


                    Result.Add(Mapper.Map<Country>(aa));

                }





            }


            IEnumerable<Country> entity = Mapper.Map<IEnumerable<Country>>(Result);
            //set.AddRange(entity);

            return entity;





        }

        public override bool checkIfNameSame(CountryInsertRequest insert, Country entry)
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
