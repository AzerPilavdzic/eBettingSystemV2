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
    public class CompetitionNPGSQLService:
        BCrudNPGSQLService<CompetitionModel,competition, CompetitionSearchObject, CompetitionInsertRequest, CompetitionUpsertRequest, CompetitionModelLess>
        ,ICompetitionNPGSQL
    {

        public CompetitionNPGSQLService(IConfiguration Service1, IMapper Service3) : base(Service1, Service3)
        {
            PrimaryKey = "id";
            ForeignKey = "countryid";
            Conflictinsert = PrimaryKey;
            ConflictUpsert = PrimaryKey;
        }


        //Get Ekstenzije
        public override string AddFilter(string query, CompetitionSearchObject search = null)
        {

            List<string> ListaSearch = new List<string>();
            List<string> ListaValues = new List<string>();
            List<string> ListaType   = new List<string>();

            if (!string.IsNullOrWhiteSpace(search?.naziv))
            {
                ListaSearch.Add("naziv");
                ListaValues.Add(search.naziv);
                ListaType.Add("S");


            }

            if (search?.countryid!=null)
            {
                ListaSearch.Add("countryid");
                ListaValues.Add(search.countryid.ToString());
                ListaType.Add("I");


            }

            if (search.sportid != null)
            {
                ListaSearch.Add("sportid");
                ListaValues.Add(search.sportid.ToString());
                ListaType.Add("I");


            }
            if (search.id != null)
            {
                ListaSearch.Add("id");
                ListaValues.Add(search.id.ToString());
                ListaType.Add("I");


            }




            if (ListaSearch.Count() != 0)
            {
                query += "where ";
            }


            for (int i = 0; i < ListaSearch.Count(); i++)
            {
                if (ListaType[i] == "S")
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

            return query;
               

        }















    }
}
