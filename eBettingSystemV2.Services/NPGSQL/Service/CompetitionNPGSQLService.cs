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
        BCrudNPGSQLService<CompetitionModel,Competition, CompetitionSearchObject, CompetitionInsertRequest, CompetitionUpsertRequest, CompetitionModelLess>
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
        















    }
}
