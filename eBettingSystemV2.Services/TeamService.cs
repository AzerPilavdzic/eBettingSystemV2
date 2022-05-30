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
        public TeamService(eBettingSystemV2.Services.Database.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;



        }



        // get by foreign key
        public override IQueryable<Team> ForeignKeyFilter(IQueryable<Team> query,int id)
        {
            var Team = query.Where(X => X.Countryid == id);

            if (Team == null)
            {
                return null;


            }
            else
            {
                return Team;

            }      

        }


        public override TeamUpsertRequest Coalesce(TeamUpsertRequest update, Team entry)
        {

            var entry2 = new TeamUpsertRequest
            {
                TeamName = update.TeamName == "string" ? entry.Teamname : update.TeamName,
                City = update.City == "string" ? entry.City : update.City,
                Countryid = update.Countryid == 0 ? entry.Countryid : update.Countryid,
                Foundedyear = update.Foundedyear == 0 ? entry.Foundedyear : update.Foundedyear,

            };


            return entry2;






        }





    }
}
