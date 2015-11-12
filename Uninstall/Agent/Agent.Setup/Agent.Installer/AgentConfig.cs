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
        public static void Create(string path, string address, string port, string key)
        {
            string configFilePath = Path.Combine(path, "../", "Config", "config.xml");
            BuildConfig(configFilePath, address, port);
        }

        private static void BuildConfig(string configPath, string address, string port)
        {
            XDocument config = new XDocument();
            XElement rootElement = new XElement("AgentConfig");
            XElement agentAddress = new XElement("address");
            agentAddress.Value = address;
            XElement agentPort = new XElement("port");
            agentPort.Value = port;
            rootElement.Add(agentAddress);
            rootElement.Add(agentPort);
            config.Add(rootElement);
            config.Save(configPath);
        }
    }
}
