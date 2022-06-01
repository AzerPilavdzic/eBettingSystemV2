﻿using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public interface ISportService :
        ICRUDService<SportModel,
            SportSearchObject,
            SportUpsertRequest,
            SportUpsertRequest,
            SportModelLess>
    {
        //test
        //SportModel UpdateJson(int id, JsonPatchDocument update);






    }
}