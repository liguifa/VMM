using Common.Message;
using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.VMware
{
    public class VMSystem : IVMSystem
    {
        public SystemResponseMessage GetSystemStatus(SystemRquestMessage request)
        {
            //TODO
            SystemResponseMessage response = new SystemResponseMessage();
            Random random = new Random();
            response.SystemInfo = new Dictionary<string, object>();
            foreach (string name in request.Names)
            {
                response.SystemInfo.Add(name, random.Next(0, 2) == 1 ? true : false);
            }
            return response;
        }


        public SystemInfoResponseMessage GetSystemInfo(SystemInfoRequestMessage request)
        {
            List<SystemInfoResponseMessage.VMSystem> outSystems = new List<SystemInfoResponseMessage.VMSystem>();
            object syncRoot = new object();
            Parallel.ForEach(Config.GetInstance("vmware.config").Machines, machine =>
            {
                VMware manager = new VMware(machine.Address,machine.Username, machine.Password);
                List<SystemInfoResponseMessage.VMSystem> systems = manager.GetVMSystems();
                lock (syncRoot)
                {
                    outSystems = outSystems.Concat(systems).ToList();
                }
            });
            outSystems = outSystems.OrderBy(d => d.Name).ToList(); ;
            SystemInfoResponseMessage response = new SystemInfoResponseMessage();
            response.VMSystyems = outSystems.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize).ToList();
            return response;
        }

        public SystemActiveResponseMessage ActiveSystem(SystemActiveRequestMessage request)
        {
            //TODO
            return null;
        }
    }
}
