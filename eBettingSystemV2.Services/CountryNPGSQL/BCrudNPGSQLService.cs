using Microsoft.Extensions.Configuration;
using eBettingSystemV2.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBettingSystemV2.Services.Interface;
using AutoMapper;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public class BCrudNPGSQLService<T, TDb, TSearch, TInsert, TUpdate, Tless,T1>
       :BaseNPGSQLService<T, TDb, TSearch, Tless,T1>,
       ICrudNPGSQL<T, TSearch, TInsert, TUpdate, Tless,T1>
       where T : class
       where TDb : class
       where TSearch : BaseSearchObject
       where TInsert : class
       where TUpdate : class
       where Tless : class
       where T1 : class
    {
        private IConfiguration service1;
        private IMapper service3;

        public BCrudNPGSQLService(IConfiguration Service1 , IMapper Service3)
        : base(Service1,Service3) { }

       
    }
}
