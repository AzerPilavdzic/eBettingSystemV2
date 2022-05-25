
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eProdaja.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourismAgency.Services
{
    public interface ICountryService : 
        ICRUDService<CountryModel, 
            CountrySearchObject, 
            CountryUpsertRequest, CountryUpsertRequest>
    {










    }

}
