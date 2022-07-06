using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static eBettingSystemV2.Model.Models.FetchEventModel;

namespace eBettingSystemV2.Services.Servisi
{
    public class FetchService : IFetch
    {
        public static List<PodaciSaStranice> podaciSaStranices = new List<PodaciSaStranice>();

        public System.Timers.Timer aTimer { get; set; } = new System.Timers.Timer();
        public static int brojac = 0;
        public static HtmlNodeCollection events;
        public static List<Podaci> eventList = new List<Podaci>();
        public static IEventService _eventService { get; set; }

        public static List<Podaci> eventsList = new List<Podaci>();
        public static HtmlNodeCollection _eventsNode;

        public static List<string> EventKeysList = new List<string>();


        public FetchService(IEventService eventService)
        {
            _eventService = eventService;
        }

        public List<Podaci> EventsTESTBEZASYNCA()
        {
            eventList.Clear();

            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();




            Console.WriteLine("POZIV " + ++brojac + ". PUT ");
            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync
                                                                          //events = document.DocumentNode.SelectNodes("//*[@id='score-data']/text()");

            //var nestotot = document.GetElementbyId("detail").GetAttributeValue("div",string.Empty);

            //prije nego sto vrati listu, provjeriti da li postoje          
            var _PageDataList = new List<Podaci>();
            //_PageDataList.AddRange(FetchAllEvents().Distinct());

            //events.ToList().ForEach(i => eventList.Add(new Podaci() {
            //EventName = i.InnerText
            //}));


            Console.WriteLine("BEZ ASYNCA " + DateTime.Now.ToString());

            return _PageDataList;

        }


        public List<PodaciSaStranice> FetchSportAndData()
        {
            var listaSportova = FetchAllSports();
            var _PageDataList = new List<PodaciSaStranice>();

            foreach (var item in listaSportova)
                if (FetchDataBySport(item, false) != null)
                    _PageDataList.AddRange(FetchDataBySport(item, true));


            if (_PageDataList.Count != 0)
                return _PageDataList;
            //await ApiService.Post<PodaciSaStranice>(_PageDataList);

            else
                return null;
        }

        public List<string> FetchAllSports()
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

            return sportsList;


        }

        public List<PodaciSaStranice> FetchDataBySport(string sport, bool ispis = true)
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


        public List<string> FetchAllEvents()
        {
            //EventsTESTBEZASYNCA();
            eventsList.Clear();

            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync
            _eventsNode = document.DocumentNode.SelectNodes("//*[@id='score-data']/text()");



            var eventsListFetched = _eventsNode.ToList().Where(x => x.InnerHtml.ToString() != " ").ToList();
            List<string> EventsFetched = new List<string>();
            for (int i = 0; i < eventsListFetched.Count(); i++)
            {
                if (!eventsListFetched[i].InnerText.ToString().Contains("-"))
                {
                    EventsFetched.Add(eventsListFetched[i].InnerText + eventsListFetched[i + 1].InnerText);
                    i++;
                    continue;
                }

                EventsFetched.Add(eventsListFetched[i].InnerText);
            }

            return EventsFetched;


            ////eventsKeyListFetched.RemoveAll(item => item == " ");


            //for (int i = 0; i < eventsKeyListFetched.Count(); i++)
            //{
            //    eventsList.Add(new Podaci()
            //    {
            //        EventName = EventsFetched[i],
            //        LinkId = eventsKeyListFetched[i],
            //        Result=  FetchEventResult(eventsKeyListFetched[i])

            //    });
            //    //Console.WriteLine(eventsList[i].EventName + " || " + eventsList[i].LinkId);
            //    //FetchEventResult(eventsList[i].LinkId);
            //}

            //eventsKeyListFetched.Clear();
            //return eventsList;
        }

        public List<string> FetchEventKeys()
        {
            EventKeysList.Clear();
            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync



            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            document = hw.Load("https://m.rezultati.com/");

            //regex koji fetcha event keyeve
            string RegexEventKeyMatch = @"(?<=/utakmica/)(.)*";
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//*[@id='score-data']/a"))
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                // Get the value of the HREF attribute

                var _match = Regex.Match(hrefValue, RegexEventKeyMatch).ToString();
                _match = _match.Remove(_match.Length - 1);
                EventKeysList.Add(_match.ToString());
            }


            return EventKeysList;
        }

        public async Task FetchEventData()
        {
            List<string> ListEventStatus = new List<string>() {
            "Not started","Live","Cancelled", "Ended"
            };


            List<string> EventName = FetchAllEvents();
            List<string> EventKey = FetchEventKeys();

            //List<Tuple<string, string>> EventNameKey = new();
            var MergedEventNameKey = EventName.Zip(EventKey, (a, b) => Tuple.Create(a, b));


            //InsertOneOrMoreAsync(IEnumerable < EventUpsertRequest > List)
            List<EventUpsertRequest> eventUpsertRequests = new();



            foreach (var EventNameKey in MergedEventNameKey)
            {
                //Result,Status,StartTime


                HtmlWeb web = new HtmlWeb();
                //da li ima razlike sa load from web async
                //HtmlDocument document = web.Load(" https://m.rezultati.com/utakmica/" + EventNameKey.Item2); //LoadFromWebAsync


                //svakom iteracijom ucitava sa stranice i utice na brzinu izvrsavanja.
                //too slow.
                HtmlDocument document = web.Load("https://m.rezultati.com/utakmica/" + EventNameKey.Item2); //LoadFromWebAsync
                
                //HtmlDocument document = await web.LoadFromWebAsync(" https://m.rezultati.com/utakmica/" + "OlAuWGfm"); //LoadFromWebAsync

                //ako je lajv, ima 3||4 diva
                //div klasa details
                //*[@id="main"]/div[3]
                var DetailsNodeCollection = document.DocumentNode.SelectNodes("//*[@id='main']/div[contains(@class, 'detail')]");
                //*[@id="main"]/div[contains(@class)]

                var shortEventName = document.DocumentNode.SelectSingleNode("/html/head/title").InnerText;

                string[] split = shortEventName.Split();

                string HomeShortName = split[0]; // [BAR]
                string AwayShortName = split[2];  //[REAL]

                int YellowCardsHome = 0;
                int RedCardsHome = 0;
                int YellowCardsAway = 0;
                int RedCardsAway = 0;



                //svi kartoni                                                      //*[@id="detail-tab-content"]/div/div/p 
                var YellowCardsNodeCollection = document.DocumentNode.SelectNodes("//*[@id='detail-tab-content']/div/div/p[contains(@class, 'i-field icon y-card')]");
                var RedCardsNodeCollection = document.DocumentNode.SelectNodes("//*[@id='detail-tab-content']/div/div/p[contains(@class, 'i-field icon r-card')]");

                //string regexMatchLastWord = "[[].*]";
                //last word without []
                string regexMatchLastWord = "(?<=(\\[))(.*)(?=])";

                YellowCardsHome = YellowCardsNodeCollection == null ? 0 : YellowCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == HomeShortName).ToList().Count();
                YellowCardsAway = YellowCardsNodeCollection == null ? 0 : YellowCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == AwayShortName).ToList().Count();

                RedCardsHome = RedCardsNodeCollection == null ? 0 : RedCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == HomeShortName).ToList().Count();
                RedCardsAway = RedCardsNodeCollection == null ? 0 : RedCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == AwayShortName).ToList().Count();

                DateTime _StartTime = new DateTime();
                string _Status = "";
                string _Result = "";
                string _Period = "";

                string[] homeAway = EventNameKey.Item1.Split(" - ");

                //za insert u tabelu naziv home i away tima
                string _home = homeAway[0].ToString();
                string _away = homeAway[1].ToString();


                switch (DetailsNodeCollection.Count())
                {
                    case 1:
                        _StartTime = DateTime.Parse(DetailsNodeCollection[0].InnerText);
                        _Status = "Not started";
                        _Result = _StartTime.ToString("HH:mm");
                        _Period = _Status;
                        break;

                    case 2:
                        _StartTime = DateTime.Parse(DetailsNodeCollection[1].InnerText);
                        _Status = "Odgodjeno";
                        _Result = "Odgodjeno";
                        break;


                    case 3:
                        if (DetailsNodeCollection[1].InnerText == "Kraj")
                        {
                            _StartTime = DateTime.Parse(DetailsNodeCollection[2].InnerText);
                            _Status = "Kraj";
                            _Result = DetailsNodeCollection[0].InnerText;
                            //Ukoliko meč nije uživo period meča nek bude isti kao status meča
                            _Period = _Status;
                            break;
                        }
                        else
                        {
                            _StartTime = DateTime.Parse(DetailsNodeCollection[2].InnerText);
                            _Status = "LIVE";
                            _Result = DetailsNodeCollection[0].InnerText;
                            break;
                        }

                    case 4:
                        _StartTime = DateTime.Parse(DetailsNodeCollection[3].InnerText);
                        //_Status = "LIVE";
                        _Status = DetailsNodeCollection[1].InnerText;
                        _Result = DetailsNodeCollection[0].InnerText;
                        _Period = _Status;
                        break;

                        //default: throw new Exception("Greska u fetchanju DetailsNodeCollectiona. FetchService Prvi Switch");

                }

                switch (_Status)
                {
                    case "LIVE":
                        _Period = DetailsNodeCollection[1].InnerText;
                        break;

                    case "Odgodjeno":
                        _Period = "Not started";
                        _Result = _Period;
                        break;

                    case "Not started":
                        _Result = null;
                        break;

                        //default: throw new Exception("Greska u fetchanju Status. FetchService Drugi switch");
                }

                eventUpsertRequests.Add(new EventUpsertRequest()
                {

                    //EventId=evet
                    EventName = EventNameKey.Item1,
                    HomeTeam = _home,
                    AwayTeam = _away,
                    EventKey = EventNameKey.Item2,

                    EventStartTime = _StartTime,
                    Result = _Result,
                    EventStatus = _Status,
                    EventPeriod = _Period,

                    RedCardsAwayTeam = RedCardsAway,
                    YellowCardsAwayTeam = YellowCardsAway,
                    RedCardsHomeTeam = RedCardsHome,
                    YellowCardsHomeTeam = YellowCardsHome,
                });
            }


           await _eventService.InsertOneOrMoreAsync(eventUpsertRequests);

        }




        //return "return od metode FetchService";


    }
}