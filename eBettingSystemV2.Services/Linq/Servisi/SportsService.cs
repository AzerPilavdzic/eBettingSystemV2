using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBettingSystemV2.Services.Linq.Servisi;
using eBettingSystemV2.Services.Linq.Interface;

namespace eBettingSystemV2.Services.Linq.Servisi
{

    public class SportService :
       BaseCRUDService
       <
       SportModel,
       sport,
       SportSearchObject,
       SportInsertRequest,
       SportUpsertRequest,
       SportModelLess
       >,
       ISportService

    {



        public SportService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;
        }


        public override SportModelLess Insert(SportInsertRequest insert)
        {
            return base.Insert(insert);
        }



        //metoda za dodavanje sportova sa odgovarajucim pravilima

        //get metode

        public async Task<SportModelLess> GetSportIdbyNameAsync(string name)
        {

            var _model = await Context.Sports
                .Where(x => x.name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
            
            if (_model == null)
            {

                //throw new Exception($"Sport sa imenom {name} ne postoji u bazi");
                return new SportModelLess { SportsId = 0 };

            }

            return Mapper.Map<SportModelLess>(_model);




        }

        //Get esktenzije
        public override IQueryable<sport> AddFilter(IQueryable<sport> query, SportSearchObject search = null)
        {
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.SportName))
            {
                filterquery = filterquery.Where(x => x.name != null)
                    .Where(X => X.name.ToLower()
                    .StartsWith(search.SportName.ToLower()));
            }

            if (search.SportId!= null)
            {
                filterquery = filterquery.Where(X => X.SportsId == search.SportId);

            }

            return filterquery;




        }

        // Upsert insert ekstenzije
        public override IEnumerable<sport> AddRange(IEnumerable<SportUpsertRequest> insertlist, DbSet<sport> set)
        {

            List<sport> Result = new List<sport>();
            sport aa = null;


            foreach (var a in insertlist)
            {
                if (a.SportsId == 0)
                {

                    aa = Mapper.Map<sport>(a);
                    //dodaj u bazu
                    set.Add(aa);
                    //dodaj u result
                    Context.SaveChanges();

                    Result.Add(aa);
                    continue;
                
                }

                var entry = set.Find(a.SportsId);

                if (entry != null)
                {

                    entry.name = a.name;

                    Result.Add(Mapper.Map<sport>(entry));

                }
                else
                {
                    if (a.SportsId != 0)
                    {

                        a.SportsId = 0;
                    
                    }

                    aa = Mapper.Map<sport>(a);
                  
                    set.Add(aa);
                    Context.SaveChanges();


                    Result.Add(Mapper.Map<sport>(aa));

                }


            
            
            
            }


            IEnumerable<sport> entity = Mapper.Map<IEnumerable<sport>>(Result);
            //set.AddRange(entity);

            return entity;



        }      
        public override bool checkIfNameSame(SportInsertRequest insert, sport entry)
        {
            if (insert.name == entry?.name)
            {

                return true;


            }
            return false;
        }
           
        public override bool BeforeInsertBool(SportInsertRequest insert)
        {
            var entity = Context.Sports.Where(x => x.name.ToLower() == insert.name.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: IME SPORTA VEC POSTOJI.");

        }

        // delete ekstenzije
        public override void BeforeDelete(int id)
        {
            var daliposotji = Context.Teams.Where(X => X.sportid == id).ToList();
            var daliposotjicompetition = Context.Competitions.Where(X => X.sportid == id).ToList();

            if (daliposotji != null)
            {
                foreach (var a in daliposotji)
                {
                    a.sportid = null;
                
                }
                Context.SaveChanges();
            }

            if (daliposotjicompetition != null)
            {
                foreach (var a in daliposotjicompetition)
                {

                    throw new Exception($"Nije moguce obrisati sport sa id {id} jer Competition {a.naziv} ima referencu na ovaj sport {a.sportid}");



                }
               
            }

        }





    }
}
