//using eProdaja.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Linq.Interface
{
    public interface IService<T, TSearch,Tless> 
        where T : class 
        where TSearch : class
        where Tless : class
    {
        Task<IEnumerable<T>> Get(TSearch search = null);
        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        IEnumerable<T> GetbyForeignKey(int Id);
        bool CheckPage0(TSearch search = null);
        bool CheckNegative(TSearch search = null);
        Task<IEnumerable<T>> GetbyForeignKeyAsync(int Id);
        Task<T> GetByName(string name);

    }
}