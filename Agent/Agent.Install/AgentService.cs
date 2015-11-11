using Common.Logger;
using Manager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Install
{
    public partial class AgentService : ServiceBase
    {
        private ServiceHost host = new ServiceHost(typeof(VMSystem));

        public AgentService()
        {
            InitializeComponent();
            host.Opened += host_Opened;
            host.Closed += host_Closed;
        }

        void host_Closed(object sender, EventArgs e)
        {
            Logger.Instance(typeof(AgentService)).Info("VMM Agent Service 已关闭.");
        }

        void host_Opened(object sender, EventArgs e)
        {
            Logger.Instance(typeof(AgentService)).Info("VMM Agent Service 已启动.");
        }

        protected override void OnStart(string[] args)
        {
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }
}
