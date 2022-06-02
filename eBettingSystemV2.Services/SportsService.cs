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

    }
}
