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




        static void Main(string[] args)
        {
            var listaSportova = RezultatiService.FetchAllSports();
            foreach (var item in listaSportova)
            {
            RezultatiService.FetchDataBySport(item);
            }
            //FetchAll();

            Console.ReadKey(); 



            SetTimer();
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();
            Console.WriteLine("Terminating the application...");


            //const string str = "Name: music mix.mp3 Size: 2356KB";
            //var match = Regex.Match(str, @"(\w*\:)");
            //Console.WriteLine(match.Groups[1].Value);

            //Console.ReadKey();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://m.rezultati.com/");



            //var title = document.DocumentNode.SelectNodes("//div/h1").First().InnerText;
            //var description = document.DocumentNode.SelectNodes("//div/p").First().InnerText;
            //*[@id="score-data"]

            var categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");

            //*[@id="main"]/h2
            var _sport = document.DocumentNode.SelectSingleNode("//*[@id='main']/h2").InnerText;
            //var _sport = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");

            Console.WriteLine(_sport.ToString());


            Console.ReadKey();

            var competitions = categories;
            
            List<string> listaKategorija = new List<string>();
            List<string> listaNatjecanja = new List<string>();
            List<string> doDvotacke = new List<string>();

            //Console.WriteLine(title);
            Console.WriteLine();


            //Console.WriteLine(categories);
            //do ove linije categories nije lista



            //categories.ToList().ForEach(i => Console.WriteLine(i.InnerText));
            categories.ToList().ForEach(i => listaKategorija.Add(i.InnerText));
            competitions.ToList().ForEach(i => listaNatjecanja.Add(i.InnerText));

            //string _regex = @"\w*\:";
            //jedna rijec 
            var _regexSport = Regex.Match(_sport, "^\\S*");
                
            Console.Write(_regexSport.ToString());
            Console.ReadKey();
            //Console.Write(Regex.Match(_sport, "^(\S*)[\s]+.*").ToString());


            for (int i = 0; i < listaKategorija.Count(); i++)
            {
                //var _match = Regex.Match(lista[i], @"(\w*\:)");
                //var _match = Regex.Match(lista[i], @"^.*?(?=:)");
                //var _CompetitionName = Regex.Match(listaNatjecanja[i], @"",RegexOptions.RightToLeft);
                var _match = Regex.Match(listaKategorija[i], @"(\w*)(?=:)");
                var _matchCompetitions = Regex.Match(listaNatjecanja[i], @"(?<=: )(?:(?! -).)*");

                listaKategorija[i] = _match.ToString();

                listaNatjecanja[i] = _matchCompetitions.ToString();
                //listaNatjecanja[i] = _CompetitionName.ToString();
                //Console.WriteLine(lista[i]);
            }


            //klasa
            //List<Competitions> Competitions= new List<Competitions>();



            HashSet<string> CategoryHashSet = new HashSet<string>();
            HashSet<string> CompetitionHashSet = new HashSet<string>();

            foreach (var item in listaKategorija)
            {
                CategoryHashSet.Add(item);
                //lista.Distinct().ToList();
            }
            foreach (var item in listaNatjecanja)
            {
                CompetitionHashSet.Add(item);
                //lista.Distinct().ToList();
            }


            //for (int i = 0; i < KategorijaHashSet.Count(); i++)
            //{
            //    Console.WriteLine()
            //    for (int j = 0; j < NatjecanjaHashSet.Count(); j++)
            //    {
            //    }
            //}
            Console.WriteLine("========================================================");
            //foreach (var category in CategoryHashSet)
            //{
            //    Console.WriteLine(category);
            //}

            foreach (var item in CompetitionHashSet)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("========================================================");
            //}
            Console.WriteLine("========================================================");
            //foreach (var category in CategoryHashSet)
            //{
            //    Console.WriteLine(category);
            //}



            foreach (var item in CategoryHashSet)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("========================================================");


            //Console.ReadKey();
            //foreach (var item in lista)
            //{
            //    //do dvotacke
            //    var _match = Regex.Match(item, @"(\w*\:)");
            //    GroupCollection groupCollection = _match.Groups;
            //    //item = groupCollection[0].ToString();
            //    //Console.WriteLine(item);
            //    //item.Split()
            //}

            //foreach (var item in doDvotacke)
            //{
            //Console.WriteLine(item);
            //    //Console.WriteLine(item.Split(":"));
            //}


            ////foreach (var item in lista)
            ////{
            ////    Console.WriteLine(item);
            ////}


            ////press any key to continue
            //Console.ReadKey();

            //foreach (var item in lista)
            //{
            //Console.WriteLine(item);
            //}




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
