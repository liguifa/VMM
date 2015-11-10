using Common.Logger;
using Common.Message;
using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.BLL
{
    public class VMSystem : BaseBLL<Manager.Model.VM>
    {
        public DashboardMessage GetSystemStatus(Guid id)
        {
            Logger.Instance(typeof(VMSystem)).Info("获取VM系统状态.");
            //查询VM系统列表
            List<Manager.Model.VM> systems = base.Search(d => d.VM_User == id && d.VM_IsDel == false);
            SystemRquestMessage request = new SystemRquestMessage();
            request.Names = systems.Select(d => d.VM_Name).ToList();
            //查询AgentServer列表
            List<Manager.Model.AgentServer> agentServers = new AgentServer().GetAgentServer();
            List<Thread> serverThread = new List<Thread>();
            DashboardMessage message = new DashboardMessage();
            foreach (Manager.Model.AgentServer agentServer in agentServers)
            {
                request.Key = agentServer.AgentServer_Key;
                EndpointAddress endpoint = new EndpointAddress(string.Format("net.tcp://{0}:{1}", agentServer.AgentServer_Address, agentServer.AgentServer_Port));
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        //调用接口获取VM系统信息
                        using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))     
                        {
                            IVMSystem proxy = channelFactory.CreateChannel(endpoint);
                            SystemResponseMessage response = proxy.GetSystemStatus(request);
                            if (message.MyActiveSystemCount == null)
                            {
                                message.MyActiveSystemCount = response.SystemInfo.Select(d => (bool)d.Value == true).Count();
                                message.MyShutdownSystemCount = response.SystemInfo.Select(d => (bool)d.Value == false).Count();
                            }
                            else
                            {
                                message.MyActiveSystemCount += response.SystemInfo.Select(d => (bool)d.Value == true).Count();
                                message.MyShutdownSystemCount += response.SystemInfo.Select(d => (bool)d.Value == false).Count();
                            }
                        }
                    }
                    catch (TimeoutException e)
                    {
                        Logger.Instance(typeof(VMSystem)).Warn(e.Message);
                    }
                    catch (Exception e)
                    {
                        Logger.Instance(typeof(VMSystem)).Error(e.Message);
                    }
                });
                thread.Start();
                serverThread.Add(thread);
            }
            DateTime currentTime = DateTime.Now;
            //等待获取VM系统信息的线程全部结束 最大等待时间为30分钟
            while (serverThread.Select(d => d.IsAlive).Count() > 0 && currentTime.AddMinutes(30) > DateTime.Now)    
            {
                Thread.Sleep(100);
            }
            message.ActiveSystem = 10;
            message.ShutdownSystemCount = 12;
            return message;
        }
    }
}
