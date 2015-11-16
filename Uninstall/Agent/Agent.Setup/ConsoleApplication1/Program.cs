using Agent.Installer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            AgentService agentService = new AgentService();
            agentService.Assemblypath = Path.Combine(@"I:\VMM\Agent\Agent.Install\bin\Release\2222", "../");
            agentService.Install();
        }
    }
}
