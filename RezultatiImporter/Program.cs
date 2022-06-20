using eBettingSystemV2.Model.Models;
using HtmlAgilityPack;
using RezultatiImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace RezultatiImporter 
{
    class Program
    {
      
        private static Timer aTimer;

        public ApiService Service_ { get; set; } = new ApiService();



        static async Task Main(string[] args)
        {
           


            var listaSportova = RezultatiService.FetchAllSports();
            foreach (var item in listaSportova)
            {
              RezultatiService.FetchDataBySport(item);
            }
            //FetchAll();


            //await ApiService.Post<PodaciSaStranice>(new PodaciSaStranice { Competitionname = "Test123", Country = "Test123", Sport = "Test123" });
            Console.ReadKey();


            var list = new List<PodaciSaStranice>();
            list.Add(new PodaciSaStranice { Competitionname = "Test444", Country = "Test444", Sport = "Test444" });

            var list2 = new List<PodaciSaStranice>();
            list2.Add(new PodaciSaStranice { Competitionname = "Test555", Country = "Test444", Sport = "Test444" });

            var list3 = new List<PodaciSaStranice>();
            list3.Add(new PodaciSaStranice { Competitionname = "Test666", Country = "Test444", Sport = "Test444" });

            SetTimer();
            await ApiService.Post<PodaciSaStranice>(list);
            await ApiService.Post<PodaciSaStranice>(list2);
            await ApiService.Post<PodaciSaStranice>(list3);



           
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();
            Console.WriteLine("Terminating the application...");
            //}
            Console.WriteLine("========================================================");

            
            




        }
        private static void SetTimer()
        {
            // Create a timer with a one second interval.
            aTimer = new Timer(1000);
            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);


            

           

        }


    }
}
