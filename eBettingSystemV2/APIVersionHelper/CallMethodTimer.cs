using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2.APIVersionHelper
{
    public class CallMethodTimer
    {

        public static void Tajmer(int seconds, Func<object>methodName)
        {
            var _startTimeSpan = TimeSpan.Zero;
            var _period = TimeSpan.FromSeconds(seconds);
            int i = 0;
            var timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine("test poziv " + i++);
                methodName();

            }, null, _startTimeSpan, _period);
        }
    }
}
