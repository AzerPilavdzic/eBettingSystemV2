using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Interface
{
    public interface ICache
    {
        Task<List<PodaciSaStranice>> SetCacheCompetition(List<PodaciSaStranice> podaciSaStranices, Func<Task<List<CompetitionModel>>> AddDataAsync);








    }
}
