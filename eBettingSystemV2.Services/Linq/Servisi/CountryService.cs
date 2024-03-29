﻿//using AutoMapper;
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
using eBettingSystemV2.Services.Linq.Servisi;
using eBettingSystemV2.Services.Linq.Interface;

//using TourismAgency.Services.Database;

namespace eBettingSystemV2.Services.Linq.Servisi
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

        //ne koristi se

        public override CountryModelLess Insert(CountryInsertRequest insert)
        {
            if (!BeforeInsertBool(insert))
            {
                throw new Exception("Drzava sa tim imenom vec postoji.");
            }
            return base.Insert(insert);

        } //ne koristi se


        //Get ekstenzije

        public override IQueryable<Country> AddFilter(IQueryable<Country> query, CountrySearchObject search = null)
        {
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.CountryName))
            {
                filterquery = filterquery.Where(x=>x.CountryName!=null)
                    .Where(X => X.CountryName.ToLower()
                    .StartsWith(search.CountryName.ToLower()));
            }

            if (search.CountryId != null)
            {
                filterquery = filterquery.Where(X => X.CountryId==search.CountryId);

            }

            return filterquery.OrderBy(x=>x.CountryName);

            //IQueryable<object> T = null;

            //return T;

        }

        public async Task<CountryModelLess> GetIdByNameAsync(string name)
        {
            var entry = await Context.Countries
                .Where(X => X.CountryName.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            if (entry == null)
            {

                //throw new Exception($"Podatak sa imenom {name} ne postoji u bazi");
                return new CountryModelLess { CountryId = 0 };

            }


            return Mapper.Map<CountryModelLess>(entry); 
        }



        //Insert Upsert sekcija 

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
                    continue;
                    //throw new Exception($"Entry with the name {a.CountryName} already exist in the database");
                
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


        //update ekstenzije

        public override void BeforeDelete(int id)
        {
            var entry = Context.Teams.Where(X=>X.countryid==id).FirstOrDefault();
            var dalipostojicompetition = Context.Competitions.Where(X=>X.countryid==id).FirstOrDefault();

            if (entry != null)
            {

                throw new Exception("Team got a relation with the Country you want to Delete");
               
            
            }

            if (dalipostojicompetition != null)
            {

                throw new Exception("the Country you want to delete got a relation with a entry from the table Competition");

            }






        }




    }
}

//inheriting base crud service 
//using Interface IDrzava 
//needs context and mapper intalized into crud
