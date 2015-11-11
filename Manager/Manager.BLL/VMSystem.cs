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
        private object syncRoot = new object();

        /// <summary>
        /// 获取VM系统开关机个数
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>包含VM系统开关机个数的消息</returns>
        public DashboardMessage GetSystemStatus(Guid id)
        {
            DashboardMessage message = new DashboardMessage();
            Logger.Instance(typeof(VMSystem)).Info("获取VM系统状态.");
            try
            {
                SystemRquestMessage request = new SystemRquestMessage();
                //查询AgentServer列表
                List<Manager.Model.AgentServer> agentServers = new AgentServer().GetAgentServer();
                List<Thread> serverThread = new List<Thread>();
                foreach (Manager.Model.AgentServer agentServer in agentServers)
                {
                    //查询VM系统列表
                    request.Names = base.Search(d => d.VM_User == id && !d.VM_IsDel && d.VM_Agent == agentServer.AgentServer_Id).Select(d => d.VM_Name).ToList(); ;
                    request.Key = agentServer.AgentServer_Key;
                    EndpointAddress endpoint = new EndpointAddress(string.Format("net.tcp://{0}:{1}", agentServer.AgentServer_Address, agentServer.AgentServer_Port));
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            SystemResponseMessage response = null;
                            //调用接口获取VM系统信息
                            using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))
                            {
                                IVMSystem proxy = channelFactory.CreateChannel(endpoint);
                                response = proxy.GetSystemStatus(request);
                            }
                            lock (this.syncRoot)
                            {
                                message.MyActiveSystemCount += response.SystemInfo.Where(d => (bool)d.Value).Count();
                                message.MyShutdownSystemCount += response.SystemInfo.Where(d => !(bool)d.Value).Count();
                            }
                        }
                        catch (TimeoutException e)
                        {
                            Logger.Instance(typeof(VMSystem)).Error(string.Format("线程超时,Thread Name:{0},{1}", Thread.CurrentThread.Name, e.Message));
                        }
                        catch (CommunicationException e)
                        {
                            Logger.Instance(typeof(VMSystem)).Error(string.Format("WCF通信异常,{0}:{1},{2}", agentServer.AgentServer_Address, agentServer.AgentServer_Port, e.Message));
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
                while (serverThread.Where(d => d.IsAlive).Count() > 0 && currentTime.AddMinutes(30) > DateTime.Now)
                {
                    Thread.Sleep(100);
                }
                //message.ActiveSystem = 10;
                //message.ShutdownSystemCount = 12;
            }
            catch (Exception e)
            {
                Logger.Instance(typeof(VMSystem)).Error(e.Message);
            }
            return message;
        }

        public SystemInfoMessage GetVMSystemInfo(Guid id)
        {
            SystemInfoMessage message = new SystemInfoMessage();
            try
            {
                //查询AgentServer列表
                List<Manager.Model.AgentServer> agentServers = new AgentServer().GetAgentServer();
                SystemInfoRequestMessage request = new SystemInfoRequestMessage();
                List<Thread> serverThread = new List<Thread>();
                foreach (Manager.Model.AgentServer agentServer in agentServers)
                {
                    //查询VM系统列表
                    request.Names = base.Search(d => d.VM_User == id && !d.VM_IsDel && d.VM_Agent == agentServer.AgentServer_Id).Select(d => d.VM_Name).ToList(); ;
                    //request.Key = agentServer.AgentServer_Key;
                    EndpointAddress endpoint = new EndpointAddress(string.Format("net.tcp://{0}:{1}", agentServer.AgentServer_Address, agentServer.AgentServer_Port));
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            SystemInfoResponseMessage response = null;
                            //调用接口获取VM系统信息
                            using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))
                            {
                                IVMSystem proxy = channelFactory.CreateChannel(endpoint);
                                response = proxy.GetSystemInfo(request);
                            }
                            lock (this.syncRoot)
                            {
                                SystemInfoMessage.VMSystem vmSystemMessageObject = new SystemInfoMessage.VMSystem();
                                //TODO
                            }
                        }
                        catch (TimeoutException e)
                        {
                            Logger.Instance(typeof(VMSystem)).Error(string.Format("线程超时,Thread Name:{0},{1}", Thread.CurrentThread.Name, e.Message));
                        }
                        catch (CommunicationException e)
                        {
                            Logger.Instance(typeof(VMSystem)).Error(string.Format("WCF通信异常,{0}:{1},{2}", agentServer.AgentServer_Address, agentServer.AgentServer_Port, e.Message));
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
                while (serverThread.Where(d => d.IsAlive).Count() > 0 && currentTime.AddMinutes(30) > DateTime.Now)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Logger.Instance(typeof(VMSystem)).Error(e.Message);
            }
            return message;
        }

        public SysytemActiveMessage ActiveSystem(Guid AgentId, string vnName)
        {
            try
            {
                Manager.Model.AgentServer agentServer = new AgentServer().GetAgentServer(AgentId);
                //TODO
            }
            catch (Exception e)
            {

            }
        }
    }
}
