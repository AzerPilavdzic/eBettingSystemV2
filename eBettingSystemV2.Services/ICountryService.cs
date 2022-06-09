
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public interface ICountryService : 
        ICRUDService<CountryModel, 
            CountrySearchObject, 
            CountryInsertRequest, 
            CountryUpsertRequest,
            CountryModelLess>
    {
        Task<CountryModelLess> GetIdByNameAsync(string name);









    }

}
