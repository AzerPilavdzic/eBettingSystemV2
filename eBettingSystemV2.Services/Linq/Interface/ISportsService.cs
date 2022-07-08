using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Linq.Interface;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Linq.Interface
{
    public interface ISportService :
        ICRUDService<SportModel,
            SportSearchObject,
            SportInsertRequest,
            SportUpsertRequest,
            SportModelLess>
    {
        //test
        //SportModel UpdateJson(int id, JsonPatchDocument update);
        //Get Metode
        Task<SportModelLess> GetSportIdbyNameAsync(string name);





    }
}
