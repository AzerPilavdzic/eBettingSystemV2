﻿using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Linq.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace eBettingSystemV2.Services.Linq.Interface
{


    public interface ICompetitionService :
    ICRUDService<CompetitionModel,
        CompetitionSearchObject,
        CompetitionInsertRequest,
        CompetitionUpsertRequest,
        CompetitionModelLess>
    {
        CompetitionModelLess GetIdbyNazivAsync(string name);
        Task<List<CompetitionModel>> AddDataAsync(List<PodaciSaStranice> Podaci);

       









    }



}
