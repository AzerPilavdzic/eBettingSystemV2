﻿using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Linq.Interface
{
    public interface ITeamService :
        ICRUDService<TeamModel,
            TeamSearchObject,
            TeamInsertRequest,
            TeamUpsertRequest,
            TeamModelLess>
    {
        //test
        TeamModel UpdateJson(int id, JsonPatchDocument update);






    }
}
