//using eProdaja.Model;
using eBettingSystemV2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.NPGSQL.Interface
{
    public interface IBaseNPGSQL<T, TSearch,Tless,T1> 
        where T : class 
        where TSearch : class
        where Tless : class
        where T1 :class
    {
        //Task<IEnumerable<GenericRasponseClass<T1>>> GetNPGSQLGeneric(TSearch search = null);

        Task<IEnumerable<T>> GetNPGSQLGeneric(TSearch search = null);
        Task<T> GetByIdAsync(int id);




    }
}