using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace eBettingSystemV2.Services.Servisi
{
    public class TimerService: ITimer
    {
        public  System.Timers.Timer aTimer { get; set; } = new System.Timers.Timer();
        public static int brojac=0;
        public static HtmlNodeCollection events;
        public static List<string> eventList = new List<string>();

        public IEventService _eventService { get; set; }
        public IFetchCacheInsert _fetchService { get; set; }



        public TimerService(IEventService eventService, IFetchCacheInsert fetchService)
        {
            _eventService = eventService;
            _fetchService = fetchService;
        }

        public void SetTimer()
        {
            aTimer = new System.Timers.Timer(2000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            //AKO HOCSE DA DODAJE, MORA ENABLE BIT TRUE.
            aTimer.Enabled = false;
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
            _fetchService.InsertEvents();
            Console.Clear();
            aTimer.Start();
        }


    }
}
