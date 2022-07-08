using AutoMapper;
using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eBettingSystemV2.Model.Models.FetchEventModel;

namespace eBettingSystemV2.Services.Servisi
{

    
    public class FetchCacheInsertService : IFetchCacheInsert
    {

        private IFetch IFetchService { get; set; }
        private ICache ICacheService { get; set; }
        private IEventService IEventService { get; set; }

        public FetchCacheInsertService(IFetch fetchService, IEventService eventService, ICache ICacheServicee)
        {
            IFetchService = fetchService;
            IEventService = eventService;
            ICacheService = ICacheServicee;
        }


        public FetchCacheInsertService(IFetch Service1, ICache Service2)
        {

            IFetchService = Service1;
            ICacheService = Service2;
        
        
        }

        public async Task FetchStoreCacheCompetition()
        {


            List<PodaciSaStranice> Lista = IFetchService.FetchSportAndData();


            //var Lista2 = await ICacheService.SetCacheCompetition(Lista);



            var Lista2 = await ICacheService.SetCacheCompetition(Lista);

        }

       

        public void InsertEvents()
        {
            //IFetchService.FetchEventKeys();
             //IFetchService.FetchEventData();


        //var eventList = IFetchService.EventsTESTBEZASYNCA();

            //List<Model.SearchObjects.EventUpsertRequest> naziv = new List<Model.SearchObjects.EventUpsertRequest>();

            //foreach (var item in eventList)
            //{
            //    naziv.Add(new Model.SearchObjects.EventUpsertRequest() {
            //    EventName=item.EventName,
            //    EventKey=item.LinkId,
            //    Result=item.Result
            //    });
            //}




            //IEnumerable<Model.SearchObjects.EventUpsertRequest> list = naziv;

            //foreach (var eventObject in eventList)
            //{
                //Console.WriteLine(eventObject.ToString());
                //IEventService.InsertOneOrMoreAsync(naziv);
            
            //}
            //naziv.Clear();
            //eventList.Clear();
        }
    }
}
