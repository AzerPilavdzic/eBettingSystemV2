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
    public class TeamService:
        BaseCRUDService<TeamModel,Team,TeamSearchObject,TeamUpsertRequest, TeamUpsertRequest,TeamModelLess>,
        ITeamService      
    {
        public TeamService(eBettingSystemV2.Services.Database.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;



        }

        public override bool BeforeInsertBool(TeamUpsertRequest insert)
        {
            var entity = Context.Teams.Where(x => x.Teamname.ToLower() == insert.TeamName.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: IME TIMA VEC POSTOJI.");
        }

        public override Task<TeamModelLess> InsertAsync(TeamUpsertRequest insert)
        {
            if (insert.Countryid<=0)
            {
                throw new Exception("Country ID ne moze biti nula.");
            }

            return base.InsertAsync(insert);
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

        public virtual TeamModel UpdateJson(int id, JsonPatchDocument update)
        {
            //var set = Context.Set<TDb>();

            var entity = Context.Teams.Find(id);

            //update = Coalesce(update, entity);


            if (entity != null)
            {
                update.ApplyTo(entity);
            }
            //else
            //{
            //    return null;
            //}

            Context.SaveChanges();

            return Mapper.Map<TeamModel>(entity);



        }

        public override TeamUpsertRequest Coalesce(TeamUpsertRequest update, Team entry)
        {

            var entry2 = new TeamUpsertRequest
            {
                TeamName = checkatributestring(update.TeamName,entry.Teamname),
                City =checkatributestring(update.City,entry.City),
                Countryid = CheckatributeInt(update.Countryid,entry.Countryid),
                Foundedyear = CheckatributeInt(update.Foundedyear.Value, entry.Foundedyear.Value),

            };


            return entry2;
        }

        public string checkatributestring(string text,string bazatext)
        {

            if (text == "string" || string.IsNullOrEmpty(text))
            {

                return bazatext;

            }
            else
            {

                return text;
            
            }





    }

        public int CheckatributeInt(int broj, int bazabroj)
        {

            return broj==0?bazabroj:broj;


        }


        public override IQueryable<Team> AddFilter(IQueryable<Team> query, TeamSearchObject search = null)
        {
            
            var filterquery = base.AddFilter(query, search);
            IQueryable<Team> filter = filterquery;

            if (!string.IsNullOrWhiteSpace(search?.Naziv))
            {
                filter = filterquery.Where(x => x.Teamname != null).Where(X => X.Teamname.ToLower().StartsWith(search.Naziv.ToLower()));
            }




            if (!string.IsNullOrWhiteSpace(search?.City))
            {
                filter = filter.Where(x=>x.City!=null).Where(X => X.City.ToLower().StartsWith(search.City.ToLower()));
            }

            if (search.CountryId != 0)
            {
                filter = filter.Where(X => X.Countryid == search.CountryId);

            }

            return filter;


        }




    }
}
