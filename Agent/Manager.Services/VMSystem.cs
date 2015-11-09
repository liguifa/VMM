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
    }
}
