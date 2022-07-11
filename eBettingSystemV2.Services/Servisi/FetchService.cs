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
        public static List<EventUpsertRequest> eventList = new List<EventUpsertRequest>();
        public static IEventService _eventService { get; set; }
        public static ICompetitionService _competitionService { get; set; }

        public static List<EventUpsertRequest> eventsList = new List<EventUpsertRequest>();
        public static HtmlNodeCollection _eventsNode;

        public static List<string> EventKeysList = new List<string>();

       public readonly List<string> ListEventStatus = new List<string>() {
            "Not started","Live","Cancelled", "Ended"
            };

        int YellowCardsHome = 0;
        int RedCardsHome = 0;
        int YellowCardsAway = 0;
        int RedCardsAway = 0;


        public DateTime _StartTime = new DateTime();
        public string _Status = "";
        public string _Result = "";
        public string _Period = "";

        public string HomeShortName;
        public string AwayShortName;

        public string _home;
        public string _away;


        public FetchService(IEventService eventService, ICompetitionService competitionService)
        {
            _eventService = eventService;
            _competitionService = competitionService;
        }

       //izbrisan EventTestBezAasynca


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
                //wait for complete
                _competitionService.AddDataAsync(_podaciSaStranice).Wait();
                Console.WriteLine(" DODAVANJE COMPETITIONA ");
                //Console.ReadKey();

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

        public void FetchEventDetails(HtmlNodeCollection DetailsNodeCollection)
        {

            //vrijednosti se moraju resetovat ukoliko se funkcija poziva vise puta.
            _StartTime = new DateTime();
            _Status = "";
            _Result = "";
            _Period = "";


            switch (DetailsNodeCollection.Count())
            {
                case 1:
                    _StartTime = DateTime.Parse(DetailsNodeCollection[0].InnerText);
                    _Status = "Not started";
                    _Result = _StartTime.ToString("HH:mm");
                    _Period = _Status;
                    break;

                case 2:
                    if (DetailsNodeCollection[0].InnerText == "Odgođeno")
                    {
                        _StartTime = DateTime.Parse(DetailsNodeCollection[1].InnerText);
                        _Status = "Odgodjeno";
                        _Result = "Odgodjeno";
                        _Period = "Odgodjeno";
                        break;
                    }
                    else
                    {
                        _StartTime = DateTime.Parse(DetailsNodeCollection[1].InnerText);
                        _Status = "Not started";
                        _Result = "Not started";
                        _Period = "Not started";
                        break;
                    }


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
                        _Status = "Live";
                        _Result = DetailsNodeCollection[0].InnerText;
                        break;
                    }

                case 4:
                    _StartTime = DateTime.Parse(DetailsNodeCollection[3].InnerText);
                    _Status = "Live";
                    //_Status = DetailsNodeCollection[1].InnerText;
                    _Result = DetailsNodeCollection[0].InnerText;
                    _Period = _Status;
                    break;

                    //default: throw new Exception("Greska u fetchanju DetailsNodeCollectiona. FetchService Prvi Switch");

            }


            switch (_Status)
            {
                case "Live":
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

        }
        public void EventYellowRedCardsInfo(HtmlNodeCollection YellowCardsNodeCollection, HtmlNodeCollection RedCardsNodeCollection)
        {
            //reset vrijednosti jer se funkcija poziva vise puta.
            YellowCardsHome = 0;
            RedCardsHome = 0;
            YellowCardsAway = 0;
            RedCardsAway = 0;



            //svi kartoni                                                      //*[@id="detail-tab-content"]/div/div/p 

            //string regexMatchLastWord = "[[].*]";
            //last word without []
            string regexMatchLastWord = "(?<=(\\[))(.*)(?=])";

            YellowCardsHome = YellowCardsNodeCollection == null ? 0 : YellowCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == HomeShortName).ToList().Count();
            YellowCardsAway = YellowCardsNodeCollection == null ? 0 : YellowCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == AwayShortName).ToList().Count();

            RedCardsHome = RedCardsNodeCollection == null ? 0 : RedCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == HomeShortName).ToList().Count();
            RedCardsAway = RedCardsNodeCollection == null ? 0 : RedCardsNodeCollection.Where(x => Regex.Match(x.NextSibling.InnerText, regexMatchLastWord).Groups[2].ToString() == AwayShortName).ToList().Count();

        }

        public void EventShortName(HtmlNode shortName) //BAR - REAL
        {
            
            var shortEventName = shortName.InnerText;

            string[] split = shortEventName.Split();

            HomeShortName = split[0]; // [BAR]
            AwayShortName = split[2];  //[REAL]
        }

        public void EventHomeAwayName(string eventName)
        {

            string[] homeAway = eventName.Split(" - ");
            _home ="";
            _away="";
            //za insert u tabelu naziv home i away tima
            _home = homeAway[0].ToString();
            _away = homeAway[1].ToString();

        }


        public async Task FetchEventData()
        {
            #region ne koristi se 

            //List<string> EventName = FetchAllEvents();
            //List<string> EventKey = FetchEventKeys();

            ////List<Tuple<string, string>> EventNameKey = new();
            //var MergedEventNameKey = EventName.Zip(EventKey, (a, b) => Tuple.Create(a, b));


            ////InsertOneOrMoreAsync(IEnumerable < EventUpsertRequest > List)
            //List<EventUpsertRequest> eventUpsertRequests = new();



            //foreach (var EventNameKey in MergedEventNameKey)
            //{
            //    //Result,Status,StartTime


            //    HtmlWeb web = new HtmlWeb();
            //    //too slow.
            //    HtmlDocument document = web.Load("https://m.rezultati.com/utakmica/" + EventNameKey.Item2); //LoadFromWebAsync


            //    //HTML NODES
            //    var DetailsNodeCollection = document.DocumentNode.SelectNodes("//*[@id='main']/div[contains(@class, 'detail')]");
            //    var YellowCardsNodeCollection = document.DocumentNode.SelectNodes("//*[@id='detail-tab-content']/div/div/p[contains(@class, 'i-field icon y-card')]");
            //    var RedCardsNodeCollection = document.DocumentNode.SelectNodes("//*[@id='detail-tab-content']/div/div/p[contains(@class, 'i-field icon r-card')]");
            //    var shortNameNode = document.DocumentNode.SelectSingleNode("/html/head/title");

            //    string[] homeAway = EventNameKey.Item1.Split(" - ");

            //    FetchEventDetails(DetailsNodeCollection);
            //    EventShortName(shortNameNode);
            //    EventYellowRedCardsInfo(YellowCardsNodeCollection, RedCardsNodeCollection);
            //    EventHomeAwayName(EventNameKey.Item1);


            //    //CompetitionInsertRequest competitionInsertRequest = new() {
            //    //Countryid =12,
            //    //Sportid =44,
            //    //Naziv="comp"
            //    //};



            //    eventUpsertRequests.Add(new EventUpsertRequest()
            //    {

            //        CompetitionId=3,

            //        //EventId=evet
            //        EventName = EventNameKey.Item1,
            //        HomeTeam = _home,
            //        AwayTeam = _away,
            //        EventKey = EventNameKey.Item2,


            //        EventStartTime = _StartTime,
            //        Result = _Result,
            //        EventStatus = _Status,
            //        EventPeriod = _Period,

            //        RedCardsAwayTeam = RedCardsAway,
            //        YellowCardsAwayTeam = YellowCardsAway,
            //        RedCardsHomeTeam = RedCardsHome,
            //        YellowCardsHomeTeam = YellowCardsHome,
            //    });
            //}


            //await _eventService.InsertOneOrMoreAsync(eventUpsertRequests);
            #endregion

        }

        //return "return od metode FetchService";



        public IEnumerable<Tuple<string, string>> FetchEventsForUpdate()
        {
            var AllEventsList = _eventService.Get();

            List<string> EventName = new();
            List<string> EventKey = new();

            //List<Tuple<string, string>> EventNameKey = new();


            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync
            _eventsNode = document.DocumentNode.SelectNodes("//*[@id='score-data']/text()");
            var _TimeNode = document.DocumentNode.SelectNodes("//*[@id='score-data']/span");
            var _KeyNode = document.DocumentNode.SelectNodes("//*[@id='score-data']/a");

            var eventsListFetched = _eventsNode.ToList().Where(x => x.InnerHtml.ToString() != " ").ToList();

            List<string> EventsFetched = new List<string>();

            for (int i = 0; i < _TimeNode.Count(); i++)
            {
                //update evenata koji su live. (obrada manje requestova)
                if (!_TimeNode[i].InnerText.Contains("'"))
                {
                    continue;
                }

                string hrefValue = _KeyNode[i].GetAttributeValue("href", string.Empty);
                // Get the value of the HREF attribute
                string RegexEventKeyMatch = @"(?<=/utakmica/)(.)*";
                var _match = Regex.Match(hrefValue, RegexEventKeyMatch).ToString();
                _match = _match.Remove(_match.Length - 1);



                EventKey.Add(_match.ToString());
                EventName.Add(_eventsNode[i].InnerText);
            }
            var MergedEventNameKey = EventName.Zip(EventKey, (a, b) => Tuple.Create(a, b));


            return MergedEventNameKey;

        }


        //moje rijesenje 
        //fetcha competitions po sportu 
        public List<string> FetchCompetitionsName(string sport)
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

            List<string> listaNatjecanja = new List<string>();

            var categories = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");

            var competitions = categories;

            if (categories != null)
            {
                competitions.ToList().ForEach(i => listaNatjecanja.Add(i.InnerText));

                HashSet<string> CompetitionHashSet = new HashSet<string>();

                for (int i = 0; i < listaNatjecanja.Count(); i++)
                {
                    var _matchCompetitions = Regex.Match(listaNatjecanja[i], @"(?<=: )(?:(?! -).)*"); //regex za natjecanja/competitione               
                    listaNatjecanja[i] = _matchCompetitions.ToString();
                    CompetitionHashSet.Add(listaNatjecanja[i]);
                }
            }
            //_competitionService.InsertOneOrMoreAsync();
            return listaNatjecanja;



        }

        public List<FetchEventModel> FetchAllEvents2()
        {
            //Funkcija FetchDataBySport insertuje u bazu sve competitione koji postoje za proslijedjeni sport.
            //FetchDataBySport("nogomet");



            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://m.rezultati.com/"); //LoadFromWebAsync

            //moze se koristiti FetchDataBySport (samo promijeniti povratni tip u List<string>) ili FetchCompetitionsName
            var CompetitionNaziv = FetchCompetitionsName("nogomet");



            //fetch eventove od X competitiona do Y 
            int pocetakIncrementa = 0;
            int a = 0;
            string competition = "Competition " + a;
            var competitionNode = document.DocumentNode.SelectNodes("//*[@id='score-data']/h4");
            int brojcompetition = competitionNode.Count();
            List<FetchEventModel> FetchModel = new List<FetchEventModel>();

            EventKeysList.Clear();

            //fetch event keyeva preko kojih se preuzimaju svi detalji o eventovima. (kartoni, rezultat, StartTime....)
            var eventsKeyList = FetchEventKeys();


            eventsList.Clear();

            _eventsNode = document.DocumentNode.SelectNodes("//*[@id='score-data']/text()");

            var eventsListFetched = _eventsNode.ToList().Where(x => x.InnerHtml.ToString() != " ").ToList();
            List<string> EventsFetched = new List<string>();


            for (int y = 0; y < brojcompetition; y++)
            {

                var competitionID =  _competitionService.GetIdbyNazivAsync(CompetitionNaziv[y]);

                FetchModel.Add(
                    new FetchEventModel
                    {
                       // CompetitionId = competitionID.Id,
                        CompetitionName = CompetitionNaziv[y],
                        CompetitionId=competitionID.Id,
                        _eventi = new List<EventUpsertRequest>()
                         
                    }) ;
                a++;

                _eventsNode = document.DocumentNode.SelectNodes($"//*[@id='score-data']//text()[preceding-sibling::h4[{y + 1}] and not(preceding-sibling::h4[position() > {y + 1}])]");

                eventsListFetched = _eventsNode.ToList().Where(x => x.InnerHtml.ToString() != " ").ToList();

                for (int i = 0; i < eventsListFetched.Count(); i++)
                {
                    HtmlWeb _web = new HtmlWeb();
                    //too slow.
                    HtmlDocument _document = web.Load("https://m.rezultati.com/utakmica/" + eventsKeyList[pocetakIncrementa]);


                    #region HTML Nodovi
                    var DetailsNodeCollection = _document.DocumentNode.SelectNodes("//*[@id='main']/div[contains(@class, 'detail')]");
                    var YellowCardsNodeCollection = _document.DocumentNode.SelectNodes("//*[@id='detail-tab-content']/div/div/p[contains(@class, 'i-field icon y-card')]");
                    var RedCardsNodeCollection = _document.DocumentNode.SelectNodes("//*[@id='detail-tab-content']/div/div/p[contains(@class, 'i-field icon r-card')]");
                    var shortNameNode = _document.DocumentNode.SelectSingleNode("/html/head/title");
                    #endregion


                    FetchEventDetails(DetailsNodeCollection);
                    EventShortName(shortNameNode);
                    EventYellowRedCardsInfo(YellowCardsNodeCollection, RedCardsNodeCollection);

                       

                    if (!eventsListFetched[i].InnerText.ToString().Contains("-"))
                    {
                        //funkcija dodjeljuje vrijednosti unutar propertija _home i _away koje cemo proslijediti u bazu.
                        EventHomeAwayName(eventsListFetched[i].InnerText + eventsListFetched[i + 1].InnerText);
                        


                        FetchModel[y]._eventi.Add(new EventUpsertRequest
                        {
                            CompetitionId = FetchModel[y].CompetitionId,
                            
                            EventName = eventsListFetched[i].InnerText + eventsListFetched[i].InnerText,
                            EventKey = eventsKeyList[pocetakIncrementa],
                            EventPeriod = _Period,
                            EventStatus = _Status,
                            Result = _Result,
                            EventStartTime =_StartTime,

                            YellowCardsHomeTeam=YellowCardsHome,
                            YellowCardsAwayTeam=YellowCardsAway,
                            RedCardsHomeTeam=RedCardsHome,
                            RedCardsAwayTeam=RedCardsAway,
                            
                            HomeTeam=_home,
                            AwayTeam=_away
                            
                        });
                        i++;
                        pocetakIncrementa++;
                        continue;
                    }
                    //string[] homeAway = eventsListFetched[i].InnerText.Split(" - ");
                    EventHomeAwayName(eventsListFetched[i].InnerText);
                    FetchModel[y]._eventi.Add(new EventUpsertRequest
                    {
                        CompetitionId = FetchModel[y].CompetitionId,

                        EventName = eventsListFetched[i].InnerText,
                        EventKey = eventsKeyList[pocetakIncrementa],

                        EventPeriod = _Period,
                        EventStatus = _Status,
                        Result = _Result,
                        EventStartTime = _StartTime,

                        YellowCardsHomeTeam = YellowCardsHome,
                        YellowCardsAwayTeam = YellowCardsAway,
                        RedCardsHomeTeam = RedCardsHome,
                        RedCardsAwayTeam = RedCardsAway,

                        HomeTeam = _home,
                        AwayTeam = _away

                    });
                    pocetakIncrementa++;
                }
                Console.WriteLine("Dodajem listu evenata broj "+ y+ " od "+ brojcompetition);
            var lista =  _eventService.InsertOneOrMoreAsync(FetchModel[y]._eventi).Result;

            }


            return FetchModel;



        }


    }
}