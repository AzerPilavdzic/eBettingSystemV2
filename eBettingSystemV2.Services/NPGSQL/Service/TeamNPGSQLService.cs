using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.NPGSQL.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.NPGSQL.Service
{
    public class TeamNPGSQLService:
        BCrudNPGSQLService<TeamModel, teams, TeamSearchObject, TeamInsertRequest, TeamUpsertRequest, TeamModelLess,string>
        ,ITeamNPGSQL
    {

        public TeamNPGSQLService(IConfiguration Service1, IMapper Service3) : base(Service1, Service3)
        {
            PrimaryKey = "teamid";
            ForeignKey = "countryid";
            Conflictinsert = PrimaryKey;
            ConflictUpsert = PrimaryKey;
        }


        //Get Ekstenzije
        public override string AddFilter(string query, TeamSearchObject search = null)
        {

            List<string> ListaSearch = new List<string>();
            List<string> ListaValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(search?.Naziv))
            {
                ListaSearch.Add("teamname");
                ListaValues.Add(search.Naziv);
                
            
            }

            if (!string.IsNullOrWhiteSpace(search?.City))
            {
                ListaSearch.Add("city");
                ListaValues.Add(search.City);


            }

            if (search.CountryId!=0)
            {
                ListaSearch.Add("countryid");
                ListaValues.Add($"{search.CountryId}");


            }


            if (ListaSearch.Count() != 0)
            {
                query += "where ";
            }


            for (int i = 0; i < ListaSearch.Count(); i++)
            {
                if (ListaSearch[i] != "countryid")
                {
                    query += $@"(lower({ListaSearch[i]}) LIKE lower('%{ListaValues[i]}%')) ";

                }
                else
                {

                    query += $@"{ListaSearch[i]} = {ListaValues[i]} ";

                }
                if ((i + 1) != ListaSearch.Count())
                {

                    query += "or ";
                
                }


            }
            








            //ako prvi postoji
            //if (!string.IsNullOrWhiteSpace(search?.Naziv))
            //{
            //    query += $@"where (lower(teamname) LIKE lower('%{search.Naziv}%')) ";

            //}


            ////ako drugi postoji a prvi ne postoji
            //if (string.IsNullOrWhiteSpace(search?.Naziv) && !string.IsNullOrWhiteSpace(search?.City))
            //{
            //    query += $@"where (lower(city)) LIKE lower('%{search.City}%')";

            //}

            ////ako prvi postoji i drugi postoji
            //if (!string.IsNullOrWhiteSpace(search?.Naziv) && !string.IsNullOrWhiteSpace(search?.City))
            //{
            //    query += $@"or (lower(city)) LIKE lower('%{search.City}%')";

            //}

            return query;


            //ako sva tri postoje 
            //ako prvi i drugi postoje
            //ako prvi postoji






            
        }



















    }
}
