using Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Manager.HyperV
{
    public class HyperVManager
    {
        private string mUsername { get; set; }
        private string mPassword { get; set; }
        private string mAddress { get; set; }

        public HyperVManager(string username, string password, string address)
        {
            this.mUsername = username;
            this.mPassword = password;
            this.mAddress = address;
        }

        public List<SystemInfoResponseMessage.VMSystem> GetVMSystems()
        {
            ConnectionOptions co = new ConnectionOptions();
            co.Username = this.mUsername;
            co.Password = this.mPassword;
            ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\virtualization", this.mAddress), co);
            scope.Connect();
            ObjectQuery query = new ObjectQuery("SELECT * FROM MsVM_ComputerSystem WHERE Caption LIKE 'Virtual%' ");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            var systemObject = searcher.Get();
            List<SystemInfoResponseMessage.VMSystem> systems = new List<SystemInfoResponseMessage.VMSystem>();
            foreach (var obj in systemObject)
            {
                SystemInfoResponseMessage.VMSystem system = new SystemInfoResponseMessage.VMSystem()
                {
                    Name = obj.GetPropertyValue("ElementName") as string,
                    Status = obj.GetPropertyValue("Status") as string,
                    CreateTime = (obj.GetPropertyValue("InstallDate") as DateTime?).ToString(),
                    LastOperationTime = (obj.GetPropertyValue("TimeOfLastConfigurationChange") as DateTime?).ToString()
                };
                systems.Add(system);
            }
            return systems;
        }

    }
}
