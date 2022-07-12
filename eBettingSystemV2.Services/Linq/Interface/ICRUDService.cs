using eBettingSystemV2.Services.Linq.Interface;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Linq.Interface
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

        T Update(int id, TInsert update);
        Task<T> UpdateAsync(int id,TInsert update);

      
        T Delete(int id);
        Task<int> DeleteAsync(int id);
        Task<Tless> InsertAsync(TInsert insert);
        Task<IEnumerable<Tless>> UpsertOneOrMoreAsync(IEnumerable<TUpdate> List);
        Task<Tless> InsertById(TInsert Insert, int Id);
        Task<T> GetByObjectName(string name);

    }
}
