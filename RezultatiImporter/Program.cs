using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RezultatiImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            


            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://m.rezultati.com/");

            //var title = document.DocumentNode.SelectNodes("//div/h1").First().InnerText;
            //var description = document.DocumentNode.SelectNodes("//div/p").First().InnerText;

            //*[@id="score-data"]
            var categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");
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


            for (int i = 0; i < listaKategorija.Count(); i++)
            {
                //var _match = Regex.Match(lista[i], @"(\w*\:)");
                //var _match = Regex.Match(lista[i], @"^.*?(?=:)");
                //var _CompetitionName = Regex.Match(listaNatjecanja[i], @"",RegexOptions.RightToLeft);
                var _match = Regex.Match(listaKategorija[i], @"(\w*)(?=:)");
                var _matchCompetitions = Regex.Match(listaNatjecanja[i], @"(?<=:).*((?=-)|($))");

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


           
            Console.WriteLine("========================================================");
           

            foreach (var item in CompetitionHashSet)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("========================================================");



          




        }
    }
}
