using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Installer
{
    public class AgentService
    {
        private readonly string installutil = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe";

        private string assemblyPath = null;

        public string Assemblypath
        {
            get
            {
                return this.assemblyPath;
            }
            set
            {
                this.assemblyPath = Path.Combine(value, "bin", "VMMAgentInstall.exe");
            }
        }

        public void Install()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = this.installutil;
            //cmd.StartInfo.Arguments = this.assemblyPath;
            cmd.StartInfo.Arguments = @"C:\Users\Guifa.Lee\Desktop\VMM Agent\bin\VMMAgentInstall.exe";
            cmd.Start();
            cmd.WaitForExit();
        }
    }
}
