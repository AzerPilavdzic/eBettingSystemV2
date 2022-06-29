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
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using eBettingSystemV2.APIVersionHelper;
using System.Threading;
using System.Timers;
using eBettingSystemV2.Services.Servisi;
using System.Globalization;
using System.Configuration;


namespace eBettingSystemV2
{
    public class Program
    {
        public static void Main(string[] args)
        {


            //novi kod za service

            var host = CreateHostBuilder(args).Build();

            //required using Microsoft.Extensions.DependencyInjection;
            // required using Microsoft.AspNetCore.Identity;
            /* using*/
            var scope = host.Services.CreateScope();           
            var services = scope.ServiceProvider;               
            var ITimer = services.GetRequiredService<ITimer>();
            var CleanSql = services.GetRequiredService<ICountryNPGSQL>();

            CleanSql.TestNPGSQL();
            ITimer.SetTimer();
               

                    
                                                                         
            host.Run();


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
