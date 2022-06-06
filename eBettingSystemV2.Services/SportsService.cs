using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace eBettingSystemV2.Services
{

    public class SportService :
       BaseCRUDService
       <
       SportModel,
       Sport,
       SportSearchObject,
       SportInsertRequest,
       SportUpsertRequest,
       SportModelLess
       >,
       ISportService

    {



        public SportService(eBettingSystemV2.Services.Database.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;
        }


        public override SportModelLess Insert(SportInsertRequest insert)
        {
            return base.Insert(insert);
        }



        //metoda za dodavanje sportova sa odgovarajucim pravilima
        public override IEnumerable<Sport> AddRange(IEnumerable<SportUpsertRequest> insertlist, DbSet<Sport> set)
        {

            List<Sport> Result = new List<Sport>();
            Sport aa = null;


            foreach (var a in insertlist)
            {
                if (a.SportsId == 0)
                {

                    aa = Mapper.Map<Sport>(a);
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

                    Result.Add(Mapper.Map<Sport>(entry));

                }
                else
                {
                    if (a.SportsId != 0)
                    {

                        a.SportsId = 0;
                    
                    }

                    aa = Mapper.Map<Sport>(a);
                  
                    set.Add(aa);
                    Context.SaveChanges();


                    Result.Add(Mapper.Map<Sport>(aa));

                }


            
            
            
            }


            IEnumerable<Sport> entity = Mapper.Map<IEnumerable<Sport>>(Result);
            //set.AddRange(entity);

            return entity;



        }


        public override IQueryable<Sport> AddFilter(IQueryable<Sport> query, SportSearchObject search = null)
        {
            var filterquery = base.AddFilter(query, search);

            if (!string.IsNullOrWhiteSpace(search?.SportName))
            {
                filterquery = filterquery.Where(x => x.name != null)
                    .Where(X => X.name.ToLower()
                    .StartsWith(search.SportName.ToLower()));
            }

            if (search.SportId != null)
            {
                filterquery = filterquery.Where(X => X.SportsId == search.SportId);

            }

            return filterquery;




        }



        //insersport by id

        public override bool checkIfNameSame(SportInsertRequest insert, Sport entry)
        {
            if (insert.name == entry?.name)
            {

                return true;


            }
            return false;
        }
           

        public override bool BeforeInsertBool(SportInsertRequest insert)
        {
            var entity = Context.Sport.Where(x => x.name.ToLower() == insert.name.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: IME SPORTA VEC POSTOJI.");

        }


        public override void BeforeDelete(int id)
        {
            var daliposotji = Context.Teams.Where(X => X.sportid == id).ToList();

            if (daliposotji != null)
            {
                foreach (var a in daliposotji)
                {
                    a.sportid = null;
                
                }
                Context.SaveChanges();
            }



            base.BeforeDelete(id);
        }





    }
}
