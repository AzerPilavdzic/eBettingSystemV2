using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Interface
{
    public interface ITimer
    {
        void TimerSeconds(int seconds, Func<object> methodName);
        void TimerHour(int hours, Func<object> methodName);
        void TimerDay(int day, Func<object> methodName);
        void TimerSecondsAsync(int seconds, Action methodName );

    }




        
}
