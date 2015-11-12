using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Agent.Installer
{
    [RunInstaller(true)]
    public partial class AgentInstaller : System.Configuration.Install.Installer
    {
        public AgentInstaller()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
      
            AgentConfig.Create(Context.Parameters["assemblypath"], Context.Parameters["ADDRESS"], Context.Parameters["PORT"], Context.Parameters["KEY"]);
        }
    }
}
