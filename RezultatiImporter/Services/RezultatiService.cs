using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Services.DataBase;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezultatiImporter.Services
{
    public class RezultatiService 
    {
        public praksa_dbContext Context { get; set; }

        public static List<PodaciSaStranice> podaciSaStranices = new List<PodaciSaStranice>();
        public RezultatiService(praksa_dbContext context)
        {
            Context = context;
        }


        //vrati object
        public static List<string> FetchAllSports()
        {
            //sportovi za sada
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://m.rezultati.com/");

          

            var sports = document.DocumentNode.SelectSingleNode("//*[@id='main']/p[1]").InnerText;


            string[] sportsArray = sports.Split('|');

            for (int i = 0; i < sportsArray.Count(); i++)
            {

                //sportsList[i] = ReplaceWhitespace(sportsList[i], sportsList[i]);
                sportsArray[i] = Regex.Match(sportsArray[i], @"[^\s].*").ToString();
            }

            List<string> sportsList = new List<string>(sportsArray);
            sportsList.RemoveAt(sportsList.Count() - 1);

            for (int i = 0; i < sportsList.Count(); i++)
            {
                sportsList[i] = sportsList[i].Trim();
                Console.WriteLine(sportsList[i]);

            }

            //List<PodaciSaStranice> _rezultat = new List<PodaciSaStranice>();
            //foreach (var item in sportsList)
            //{
            //    _rezultat.Add(new PodaciSaStranice() { Sport = item });
            //}

            return sportsList;


        }
    
        public static List<PodaciSaStranice> FetchDataBySport(string sport, bool ispis=true)
        {
            if (sport.Contains("š"))
            {
                sport= sport.Replace("š", "s");
            
            }
            if (sport=="Am. nogomet")
            {
                sport = "americki-nogomet" ;
            }




            sport = sport.ToLower();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://m.rezultati.com/");

            if (sport!="nogomet")
            {
                document = web.Load("https://m.rezultati.com/"+sport+"/");
            }

            List <PodaciSaStranice> _podaciSaStranice = new List<PodaciSaStranice>();

            List<string> listaKategorija = new List<string>();
            List<string> listaNatjecanja = new List<string>();
          
            var categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");

            var competitions = categories;

            if (categories!=null)
            {

            categories.ToList().ForEach(i => listaKategorija.Add(i.InnerText));
            competitions.ToList().ForEach(i => listaNatjecanja.Add(i.InnerText));

            for (int i = 0; i < listaKategorija.Count(); i++)
            {
                var _match = Regex.Match(listaKategorija[i], @"(\w*)(?=:)");
                var _matchCompetitions = Regex.Match(listaNatjecanja[i], @"(?<=: )(?:(?! -).)*"); //regex za natjecanja/competitione

                listaKategorija[i] = _match.ToString();

                listaNatjecanja[i] = _matchCompetitions.ToString();

            }

            HashSet<string> CategoryHashSet = new HashSet<string>();
            HashSet<string> CompetitionHashSet = new HashSet<string>();

            foreach (var item in listaKategorija)
            {
                CategoryHashSet.Add(item);
            }

            foreach (var item in listaNatjecanja)
            {
                CompetitionHashSet.Add(item);
            }

            var _CompetitionHashSetList = CompetitionHashSet.ToList();
            var _CategoryHashSetList = CategoryHashSet.ToList();

            for (int i = 0; i < CompetitionHashSet.Count(); i++)
            {
                _podaciSaStranice.Add(new PodaciSaStranice()
                {
                    Competitionname = _CompetitionHashSetList[i],
                    Sport = sport,
                    Country= listaKategorija[i]
                });
            }

            podaciSaStranices = _podaciSaStranice;

            //long size = 0;
            //using ( Stream s = new MemoryStream())
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(s, podaciSaStranices);
            //    size = s.Length;
            //    Console.Write(size);
            //}
            Console.ReadKey();

                if (ispis)
                {

                    Console.WriteLine("\n:::::::::::::::::::::: " + sport.ToUpper() + " ::::::::::::::::::::::\n");
                    foreach (var item in _podaciSaStranice)
                    {
                        Console.WriteLine(item.Country + " : " + item.Competitionname);
                    }
                }
                    return podaciSaStranices;
            }
            return null;
        }

        public static void FetchDate()
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://m.rezultati.com/");

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)null;

            using (myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
            {
                Console.WriteLine("\r\nThe following headers were received in the response:");

                for (int i = 0; i < myHttpWebResponse.Headers.Count; ++i)
                    Console.WriteLine("\nHeader Name:{0}, Value :{1}", myHttpWebResponse.Headers.Keys[i], myHttpWebResponse.Headers[i]);
                // Releases the resources of the response.
                myHttpWebResponse.Close();

                Console.WriteLine(myHttpWebResponse.LastModified);

            }

         


            WebClient client = new WebClient();
         


            string downloadString = client.DownloadString("http://www.gooogle.com");

            


            

        }
    }
}
