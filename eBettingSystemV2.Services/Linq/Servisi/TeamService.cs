using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Linq.Interface;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Linq.Servisi
{
    public class TeamService :
        BaseCRUDService<TeamModel, teams, TeamSearchObject, TeamInsertRequest, TeamUpsertRequest, TeamModelLess>,
        ITeamService
    {
        public TeamService(eBettingSystemV2.Services.DataBase.praksa_dbContext context_, IMapper mapper_) : base(context_, mapper_)
        {

            Context = context_;
            Mapper = mapper_;



        }

        public override bool BeforeInsertBool(TeamInsertRequest insert)
        {
            var dalipostoji = Context.Countries.Find(insert.countryid);
            if (dalipostoji == null)
            {
                throw new Exception($"Country with the countryid {insert.countryid} Does not exist in the Database");

            }








            var entity = Context.Teams.Where(x => x.teamname.ToLower() == insert.teamname.ToLower()).FirstOrDefault();
            if (entity == null)
            {
                return true;
            }
            throw new Exception("EXCEPTION: IME TIMA VEC POSTOJI.");
        }

        public override Task<TeamModelLess> InsertAsync(TeamInsertRequest insert)
        {
            //if (insert.countryid <= 0)
            //{
            //    throw new Exception("Country ID ne moze biti nula.");
            //}

            return base.InsertAsync(insert);
        }


        // get by foreign key
        public override IQueryable<teams> ForeignKeyFilter(IQueryable<teams> query, int id)
        {
            var Team = query.Where(X => X.countryid == id);

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

        public override TeamUpsertRequest Coalesce(int Id,TeamInsertRequest update, teams entry)
        {

            var entry2 = new TeamUpsertRequest
            {
                teamname = checkatributestring(update.teamname, entry.teamname),
                city = checkatributestring(update.teamname, entry.city),
                countryid = CheckatributeInt(update.countryid, entry.countryid),
                foundedyear = CheckatributeInt(update.foundedyear.Value, entry.foundedyear.Value),

            };


            return entry2;
        }

        public string checkatributestring(string text, string bazatext)
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

            return broj == 0 ? bazabroj : broj;


        }


        public override IQueryable<teams> AddFilter(IQueryable<teams> query, TeamSearchObject search = null)
        {

            var filterquery = base.AddFilter(query, search);
            IQueryable<teams> filter = filterquery;

            if (!string.IsNullOrWhiteSpace(search?.Naziv))
            {
                filter = filterquery.Where(x => x.teamname != null).Where(X => X.teamname.ToLower().StartsWith(search.Naziv.ToLower()));
            }




            if (!string.IsNullOrWhiteSpace(search?.City))
            {
                filter = filter.Where(x => x.city != null).Where(X => X.city.ToLower().StartsWith(search.City.ToLower()));
            }

            if (search.CountryId != 0)
            {
                filter = filter.Where(X => X.countryid == search.CountryId);

            }

            return filter;


        }





        
    }
}
