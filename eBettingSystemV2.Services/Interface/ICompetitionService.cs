using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Interface
{
    
    
        public interface ICompetitionService :
        ICRUDService<CompetitionModel,
            CompetitionSearchObject,
            CompetitionUpsertRequest,
            CompetitionInsertRequest,
            CompetitionModelLess>
        { 
        }

        
    
}
