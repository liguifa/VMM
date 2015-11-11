using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Agent.Installer
{
    public static class AgentConfig
    {
        public static void Create(string address,string port,string key)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string configFolderPath = Path.Combine(path, "Config");
            string configFilePath = Path.Combine(configFolderPath, "config.xml");
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }
            if (!File.Exists(configFilePath))
            {
                File.Create(configFilePath);
            }

            //TODO
        }

        private void BuildConfig()
        {
            XDocument config = new XDocument();
            config.Add("AgentService");
            config.Save("");

            //TODO
        }
    }
}
