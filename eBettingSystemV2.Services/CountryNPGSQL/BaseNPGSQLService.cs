using AutoMapper.Configuration;
using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public class BaseNPGSQLService<T, TDb, TSearch, Tless>:
        IBaseNPGSQL<T, TSearch, Tless>
        where T : class
        where TDb : class
        where TSearch : BaseSearchObject//base search service
        where Tless : class
    {
        public IConfiguration Configuration { get; }
        public BaseNPGSQLService(IConfiguration Service1)
        {
            Configuration = Service1;
        }





    }
}
