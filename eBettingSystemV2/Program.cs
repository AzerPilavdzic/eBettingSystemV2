using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using eBettingSystemV2.APIVersionHelper;
using System.Threading;
using System.Timers;
using eBettingSystemV2.Services.Servisi;

namespace eBettingSystemV2
{
    public class Program
    {

         public static List<string> eventList = new List<string>();


        //public static object LoadData()
        //{
        //    var requests=urls.


        //    return null;
        //}
             public static HtmlNodeCollection events; 
             public static int brojac=0; 
        public async static Task<object> Events()
        {
            //HtmlWeb web = new HtmlWeb();
            //Console.WriteLine("aaaaaaaaa");
            Action action = new Action(delegate ()
            {

                Console.WriteLine("POZIV " + ++brojac + ". PUT ");
                HtmlWeb web = new HtmlWeb();

                Task<HtmlDocument> document = web.LoadFromWebAsync("https://m.rezultati.com/"); //LoadFromWebAsync
                HtmlDocument objbjbjb = document.Result;
                events = document.Result.DocumentNode.SelectNodes("//*[@id='score-data']/text()");
                Thread.Sleep(10000);
                Console.WriteLine("Events metoda tred isAlive " + Thread.CurrentThread.IsAlive);
            });
                Console.WriteLine("Events metoda tred isAlive "+ Thread.CurrentThread.IsAlive);
            //await Task.Delay(TimeSpan.FromSeconds(10000));

            action.Invoke(); 
            await Task.Yield();
           //await Task.Run(action);

           

            //events.ToList().ForEach(i => eventList.Add(i.InnerText));

            //foreach (var item in eventList)
            //{

            //    Console.WriteLine(item.ToString());
            //}

            Console.WriteLine(DateTime.Now.ToString());
            //Console.WriteLine("POZIV "+ ++brojac + ". PUT ");
            return true;
        }

        public async static Task<object> GroupedNodesAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            Console.WriteLine("Grouped Nodes Async thread is alive" + Thread.CurrentThread.IsAlive);
            Action action = new Action(delegate () { _ = Events(); });
            action.Invoke();
                return await Task.Run(() => Events());
        }


            public static HtmlNodeCollection eventsTEST2;
        public static void EventsTESTBEZASYNCA()
        {
            //try
            //{

                Console.WriteLine("POZIV " + ++brojac + ". PUT ");
               HtmlWeb web = new HtmlWeb();

              HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync
            eventsTEST2 = document.DocumentNode.SelectNodes("//*[@id='score-data']/text()");
            eventsTEST2.ToList().ForEach(i => eventList.Add(i.InnerText));

            foreach (var eventObject in eventList)
            {
                Console.WriteLine(eventObject.ToString());
                //
            }

            //u zasebnom threadu pokusati uraditi provjere i dodavanje u bazu
            
    


            Console.WriteLine("BEZ ASYNCA " + DateTime.Now.ToString());
            Thread.Sleep(2500);

            //Console.WriteLine("POZIV " + ++brojac + ". PUT ");
            //return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return false;
        }

        private static System.Timers.Timer aTimer;

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(EventsTESTBEZASYNCA));

            t1.Start();
            Thread.Sleep(1250);


            t1.Join();

            //EventsTESTBEZASYNCA();
        }
        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }


        public static void Main(string[] args)
        {
            FetchService fetchService = new FetchService();

            //FetchService

            //CallMethodTimer.TimerSeconds(5, GroupedNodesAsync);
            SetTimer();

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
            //logger
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
               
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

















            //var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var database = scope.ServiceProvider.GetService<praksa_dbContext>();

            //    new SetupService().Init(database);
            //}

            ////test
            //host.Run();







            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>//logger
                {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();  // NLog: Setup NLog for Dependency injection
            
            
            
    }
}
