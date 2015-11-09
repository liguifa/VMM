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
            SystemResponseMessage reponse = new SystemResponseMessage();
            Random random = new Random();
            foreach (string name in request.Names)
            {
                reponse.SystemInfo.Add(name, random.Next(0, 2) == 1 ? true : false);
            }
            return reponse;
        }
    }
}
