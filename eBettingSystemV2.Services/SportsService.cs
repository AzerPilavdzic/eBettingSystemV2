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
            int takenumber = insertlist.Count(); //koliko ih treba uzeti
            int addnumber = 0;  //koliko ih treba dodati
            var list = insertlist.ToList(); //konverzija

            if (takenumber > set.Count())
            {
                addnumber = set.Count() - takenumber;
                set.AddRange(Mapper.Map<IEnumerable<Sport>>(list.Skip(takenumber - addnumber)));
                takenumber = set.Count();

            }




            var SetMini = set.Take(takenumber);

            int i = 0;




            foreach (var a in SetMini)
            {

                if (a.name != list[i].name)
                {
                    //a.name = list[i].name;
                    set.Find(a.SportsId).name = list[i].name;
                }
                i++;


            }







            IEnumerable<Sport> entity = Mapper.Map<IEnumerable<Sport>>(SetMini);
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
            var entity = Context.Sports.Where(x => x.name.ToLower() == insert.name.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: IME SPORTA VEC POSTOJI.");

        }


        public override void BeforeDelete(int id)
        {
            var daliposotji = Context.Teams.Where(X => X.Sportid == id).ToList();

            if (daliposotji != null)
            {
                foreach (var a in daliposotji)
                {
                    a.Sportid = null;
                
                }
                Context.SaveChanges();
            }



            base.BeforeDelete(id);
        }





    }
}
