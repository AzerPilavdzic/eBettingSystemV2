using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace eBettingSystemV2.Services.Servisi
{
    public class TimerService: ITimer
    {
        public System.Timers.Timer aTimer { get; set; } = new System.Timers.Timer();

        public System.Timers.Timer DayTimer { get; set; } = new System.Timers.Timer(10000);

        //public System.Timers.Timer DayTimer { get; set; } = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);


        public static int brojac=0;
        public static HtmlNodeCollection events;
        public static List<string> eventList = new List<string>();

        public int i = 0;



        //public ICompetitionService ICompetitionService { get; set;}
        public IFetchCacheInsert   IFetchCacheService { get; set; }

        public TimerService(IFetchCacheInsert service)
        {

            IFetchCacheService = service;     
        }



        //TimerService()
        //{
        //    aTimer = new System.Timers.Timer();
        //}
       

        public void EventsTESTBEZASYNCA()
        {
            //try
            //{



            Console.WriteLine("POZIV " + ++brojac + ". PUT ");
            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync
            events = document.DocumentNode.SelectNodes("//*[@id='score-data']/text()");
            events.ToList().ForEach(i => eventList.Add(i.InnerText));

            foreach (var eventObject in eventList)
            {
                Console.WriteLine(eventObject.ToString());
                //
            }

            //u zasebnom threadu pokusati uraditi provjere i dodavanje u bazu




            Console.WriteLine("BEZ ASYNCA " + DateTime.Now.ToString());

            //Console.WriteLine("POZIV " + ++brojac + ". PUT ");
            //return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return false;
        }

        public void SetTimer()
        {
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += OnTimedEventDay;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;


          
           
        }

           

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
                Console.WriteLine(e.SignalTime);
                EventsTESTBEZASYNCA();





        }

        public void OnTimedEventDay(object source, ElapsedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string Date = config.AppSettings.Settings["DateKey"].Value;

            var DateTimeFromConfig = DateTime.Parse(Date);

            if (DateTimeFromConfig.Date < DateTime.Now.Date)
            {

                IFetchCacheService.FetchStoreCacheCompetition();

                config.AppSettings.Settings["DateKey"].Value = DateTimeFromConfig.AddDays(1).ToString();

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                //Properties.Settings.Default.Reload();
                

            }
            config = null;


            IFetchCacheService.FetchStoreCacheCompetition();

            
            i++;

            aTimer.Start();


        }

    }
}
