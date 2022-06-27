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
        
        public void SetTimer();
        public void OnTimedEvent(Object source, ElapsedEventArgs e);
        //public void EventsTESTBEZASYNCA();

        public System.Timers.Timer aTimer { get; set; }
        public System.Timers.Timer DayTimer { get; set; }



    }




        
}
