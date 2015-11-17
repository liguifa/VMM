using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Common.SmartThread
{
    public class SmartThread
    {
        private List<Thread> threadList = new List<Thread>();

        public void Invoke(ParameterizedThreadStart func)
        {
            Thread thread = new Thread(func);
            thread.IsBackground = true;
            thread.Start();
            threadList.Add(thread);
        }

        public void Wait(int sec = 60)
        {
            DateTime currentTime = DateTime.Now;
            while (threadList.Where(d => d.IsAlive).Count() > 0 && currentTime.AddSeconds(sec) > DateTime.Now)
            {
                Thread.Sleep(100);
            }
        }
    }
}
