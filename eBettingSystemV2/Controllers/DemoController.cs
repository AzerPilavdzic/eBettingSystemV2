using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
//using eBettingSystemV2.Models;
using eProdaja.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //public class CountryController
    public class DemoController
    {

        private IDemo IDemoService { get; set; }

        public DemoController(IDemo service)
        {
            IDemoService = service;
          
        
        
        }


       
     

        [HttpPost]
        [Route("InsertCompetitionUsingNamesOnly")]
        public async Task<ActionResult<List<CompetitionModel>>> InsertCompetitionUsingNamesOnly(List<PodaciSaStranice> lista)
        {
            var result = await IDemoService.AddDataAsync(lista);


            return result;
                  
          
        }

       


           
        
    }
}
