using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public class TeamService:
        BaseCRUDService<TeamModel,Team,TeamSearchObject,TeamUpsertRequest, TeamUpsertRequest>,
        ITeamService      
    {
        public TeamService(eBettingSystemV2.Services.Database.BettingSystemContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;



        }


    }
}
