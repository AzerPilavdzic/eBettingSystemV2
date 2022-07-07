using eBettingSystemV2.Services.NPGSQL.Interface;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.NPGSQL.Interface
{
    public interface ICrudNPGSQL<T, TSearch, TInsert, TUpdate,Tless,T1> : 
        IBaseNPGSQL<T, TSearch,Tless,T1> 
        where T : class 
        where TSearch : class 
        where TInsert : class 
        where TUpdate : class
        where Tless : class
        where T1:class //atribute from generic class
    {
        Task<Tless> InsertAsync(TInsert insert);
        Task<Tless> UpsertbyIdAsync(TInsert Insert, int Id);
        Task<T> UpdateAsync(int id, TUpdate update);
        Task<IEnumerable<T>> UpsertOneOrMoreAsync(IEnumerable<TUpdate> List);
        Task<IEnumerable<T>> InsertOneOrMoreAsync(IEnumerable<TInsert> List);
        Task<int> DeleteAsync(int id);







    }
}
