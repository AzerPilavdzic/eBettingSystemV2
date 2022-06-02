using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
using Microsoft.AspNetCore.JsonPatch;
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

        public override bool BeforeInsertBool(SportUpsertRequest insert)
        {
            var entity = Context.Sport.Where(x => x.name.ToLower() == insert.name.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: IME SPORTA VEC POSTOJI.");
        }



    }
}
