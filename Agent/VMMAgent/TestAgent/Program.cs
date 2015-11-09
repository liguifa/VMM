using Manager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(VMSystem)))
            {
                host.Opened += host_Opened;
                host.Open();
            }
            Console.Read();
        }

        static void host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Start！");
        }
    }
}
