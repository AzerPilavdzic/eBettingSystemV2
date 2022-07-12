using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.NPGSQL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.NPGSQL.Interface
{
    public interface ICountryNPGSQL :
         ICrudNPGSQL<CountryModel,
             CountrySearchObject,
             CountryInsertRequest,
             CountryUpsertRequest,
             CountryModelLess>
    {
        Task<CountryModelLess> GetIdByNameAsync(string name);




    }
}
