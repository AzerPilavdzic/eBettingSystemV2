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
       SportUpsertRequest,
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


        public override SportModelLess Insert(SportUpsertRequest insert)
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


        //insersport by id

        public override bool checkIfNameSame(SportUpsertRequest insert, Sport entry)
        {
            if (insert.name == entry?.name)
            {

                return true;
            
            
            }
            return false;

           
        }






    }
}
