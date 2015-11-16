using Common.Message;
using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.AgentServices
{
    public class VMSystem : IVMSystem
    {
        public EndpointAddress endpoint { get; set; }

        public VMSystem(EndpointAddress endpoint)
        {
            this.endpoint = endpoint;
        }

        public SystemResponseMessage GetSystemStatus(SystemRquestMessage request)
        {
            SystemResponseMessage response = null;
            using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))
            {
                IVMSystem proxy = channelFactory.CreateChannel(this.endpoint);
                response = proxy.GetSystemStatus(request);
            }
            return response;
        }

        public SystemInfoResponseMessage GetSystemInfo(SystemInfoRequestMessage request)
        {
            SystemInfoResponseMessage response = null;
            using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))
            {
                IVMSystem proxy = channelFactory.CreateChannel(this.endpoint);
                response = proxy.GetSystemInfo(request);
            }
            return response;
        }

        public SystemActiveResponseMessage ActiveSystem(SystemActiveRequestMessage request)
        {
            SystemActiveResponseMessage response = null;
            using (ChannelFactory<IVMSystem> channelFactory = new ChannelFactory<IVMSystem>("VMSystem"))
            {
                IVMSystem proxy = channelFactory.CreateChannel(this.endpoint);
                response = proxy.ActiveSystem(request);
            }
            return response;
        }
    }
}
