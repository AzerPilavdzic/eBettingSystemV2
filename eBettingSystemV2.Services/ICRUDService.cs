using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public interface ICRUDService<T, TSearch, TInsert, TUpdate,Tless> : 
        IService<T, TSearch,Tless> 
        where T : class 
        where TSearch : class 
        where TInsert : class 
        where TUpdate : class
        where Tless:class
    {
        Tless Insert(TInsert insert);

        T Update(int id, TUpdate update);

      
        T Delete(int id);

    }
}
