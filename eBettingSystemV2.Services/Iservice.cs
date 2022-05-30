//using eProdaja.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public interface IService<T, TSearch> where T : class where TSearch : class
    {
        Task<IEnumerable<T>> Get(TSearch search = null);

        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        IEnumerable<T> GetbyForeignKey(int Id);
        bool CheckPage0(TSearch search = null);
        bool CheckNegative(TSearch search = null);
    }
}