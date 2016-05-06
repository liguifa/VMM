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

namespace Manager.TimerHost
{
    public partial class Service : ServiceBase
    {
        private ServiceHost mHost = new ServiceHost(typeof(Manager.TimerService.JobReport));
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.mHost.Open();
        }

        protected override void OnStop()
        {
            this.mHost.Close();
        }
    }
}
