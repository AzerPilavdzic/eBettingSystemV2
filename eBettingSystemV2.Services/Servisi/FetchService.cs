using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{
    public class FetchService:IFetch
    {
        public static List<PodaciSaStranice> podaciSaStranices = new List<PodaciSaStranice>();




        public List<PodaciSaStranice> FetchSportAndData()
        {
            var listaSportova = FetchAllSports();
            var _PageDataList = new List<PodaciSaStranice>();

            foreach (var item in listaSportova)
            {
                if (FetchDataBySport(item, false) != null)
                {
                    _PageDataList.AddRange(FetchDataBySport(item, true));
                }
            }

            if (_PageDataList.Count != 0)
            {
                return _PageDataList;
                //await ApiService.Post<PodaciSaStranice>(_PageDataList);
            }
            else
            {


                return null;

            }
        }

        public  List<string> FetchAllSports()
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

        public  List<PodaciSaStranice> FetchDataBySport(string sport, bool ispis = true)
        {
            if (sport.Contains("š"))
            {
                sport = sport.Replace("š", "s");

            }
            if (sport == "Am. nogomet")
            {
                sport = "americki-nogomet";
            }




            sport = sport.ToLower();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://m.rezultati.com/");

            if (sport != "nogomet")
            {
                document = web.Load("https://m.rezultati.com/" + sport + "/");
            }

            List<PodaciSaStranice> _podaciSaStranice = new List<PodaciSaStranice>();

            List<string> listaKategorija = new List<string>();
            List<string> listaNatjecanja = new List<string>();

            var categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");

            var competitions = categories;

            if (categories != null)
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
                        Country = listaKategorija[i]
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







    }
}
