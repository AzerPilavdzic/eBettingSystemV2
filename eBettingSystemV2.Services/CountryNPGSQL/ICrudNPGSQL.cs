using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.CountryNPGSQL
{
    public interface ICrudNPGSQL<T, TSearch, TInsert, TUpdate,Tless> : 
        IBaseNPGSQL<T, TSearch,Tless> 
        where T : class 
        where TSearch : class 
        where TInsert : class 
        where TUpdate : class
        where Tless:class
    {
        





    }
}
