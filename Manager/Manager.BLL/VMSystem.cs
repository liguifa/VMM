using Common.Logger;
using Common.Message;
using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.BLL
{
    public class VMSystem : BaseBLL<Manager.Model.VM>
    {
        public DashboardMessage GetSystemStatus(Guid id)
        {
            Logger.Instance(typeof(VMSystem)).Info("获取VM系统状态.");
            //查询数据库列表
            List<Manager.Model.VM> systems = base.Search(d => d.VM_User == id && d.VM_IsDel == false);
            SystemRquestMessage request = new SystemRquestMessage();
            request.Names = systems.Select(d => d.VM_Name).ToList();
            SystemResponseMessage response = null;
            using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))
            {
                IVMSystem proxy = channelFactory.CreateChannel();
                response = proxy.GetSystemStatus(request);
            }
            DashboardMessage message = new DashboardMessage();
            message.MyActiveSystemCount = response.SystemInfo.Select(d => (bool)d.Value == true).Count();
            message.MyShutdownSystemCount = response.SystemInfo.Select(d => (bool)d.Value == false).Count();
            message.ActiveSystem = 10;
            message.ShutdownSystemCount = 12;
            return message;
        }
    }
}
