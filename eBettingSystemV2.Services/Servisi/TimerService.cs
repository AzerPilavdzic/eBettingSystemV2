using eBettingSystemV2.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{
    public class TimerService:ITimer
    {
        public  void TimerSeconds(int seconds, Func<object> methodName)
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

    }
}
