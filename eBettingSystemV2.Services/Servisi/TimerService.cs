using eBettingSystemV2.Services.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
        public static int brojac=0;
        public static HtmlNodeCollection events;
        public static List<string> eventList = new List<string>();


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
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void TimerSeconds(int seconds, Func<object> methodName)
        {
            var _startTimeSpan = TimeSpan.Zero;
            var _period = TimeSpan.FromSeconds(seconds);
            int i = 1;
            var timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine("test poziv timer seconds " + i++);
                methodName();

            }, null, _startTimeSpan, _period);
        }

        public  void TimerHour(int hours, Func<object> methodName)
        {
            var _startTimeSpan = TimeSpan.Zero;
            var _period = TimeSpan.FromHours(hours);
            int i = 1;
            var timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine("test poziv timer hour " + i++);
                methodName();

            }, null, _startTimeSpan, _period);
        }


        public  void TimerDay(int day, Func<object> methodName)
        {
            var _startTimeSpan = TimeSpan.Zero;
            var _period = TimeSpan.FromDays(day);
            int i = 1;
            var timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine("test poziv timer day " + i++);
                methodName();

            }, null, _startTimeSpan, _period);
        }

        public void TimerSecondsAsync(int seconds, Action methodName)
        {
            var _startTimeSpan = TimeSpan.Zero;
            var _period = TimeSpan.FromSeconds(seconds);
            int i = 1;
            var timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine("test poziv timer seconds " + i++);
                methodName();

            }, null, _startTimeSpan, _period);
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
                Console.WriteLine(e.SignalTime);
                EventsTESTBEZASYNCA();
        }
       
    }
}
