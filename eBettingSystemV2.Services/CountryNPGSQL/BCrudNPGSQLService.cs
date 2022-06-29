using AutoMapper.Configuration;
using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public class BCrudNPGSQLService<T, TDb, TSearch, TInsert, TUpdate, Tless>
       :BaseNPGSQLService<T, TDb, TSearch, Tless>,
       ICrudNPGSQL<T, TSearch, TInsert, TUpdate, Tless>
       where T : class
       where TDb : class
       where TSearch : BaseSearchObject
       where TInsert : class
       where TUpdate : class
       where Tless : class
    {
        public BCrudNPGSQLService(IConfiguration Service1)
        : base(Service1) { }




    }
}
