using Common.Message;
using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Services
{
    public class VMSystem : IVMSystem
    {
        public SystemResponseMessage GetSystemStatus(SystemRquestMessage request)
        {
            //TODO   现在是模拟数据
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
            //TODO   现在是模拟数据
            SystemInfoResponseMessage response = new SystemInfoResponseMessage();
            foreach (string vmSystemName in request.Names)
            {
                SystemInfoResponseMessage.VMSystem vmSystem = new SystemInfoResponseMessage.VMSystem();
                vmSystem.Status = true;
                vmSystem.Name = vmSystemName;
                vmSystem.CreateTime = "2012/12/12";
                vmSystem.LastOperationTime = "2013/11/11";
                response.VMSystyems.Add(vmSystem);
            }
            return response;
        }

        public SystemActiveResponseMessage ActiveSystem(SystemActiveRequestMessage request)
        {
            //TODO
            return null;
        }
    }
}
