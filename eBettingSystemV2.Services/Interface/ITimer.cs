using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace eBettingSystemV2.Services.Interface
{
    public interface ITimer
    {
        void TimerSeconds(int seconds, Func<object> methodName);
        void TimerHour(int hours, Func<object> methodName);
        void TimerDay(int day, Func<object> methodName);
        void TimerSecondsAsync(int seconds, Action methodName );


        public void SetTimer();
        public void OnTimedEvent(Object source, ElapsedEventArgs e);
        public void EventsTESTBEZASYNCA();

        public System.Timers.Timer aTimer { get; set; }
    }




        
}
