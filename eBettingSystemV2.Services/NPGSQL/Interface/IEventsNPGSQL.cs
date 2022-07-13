using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.NPGSQL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.NPGSQL.Interface
{
    public interface IEventsNPGSQL :
         ICrudNPGSQL<EventModel,
                     EventSearchObject,
                     EventInsertRequest,
                     EventUpsertRequest,
                     EventModelLess
             >
    {

        Task<EventModelLess> GetIdByNameAsync(string name);
        EventUpsertRequest GetByEventKey(string eventKey);


    }
}
