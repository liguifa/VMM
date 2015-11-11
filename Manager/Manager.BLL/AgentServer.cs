using Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.BLL
{
    public class AgentServer : BaseBLL<Manager.Model.AgentServer>
    {
        public List<Manager.Model.AgentServer> GetAgentServer()
        {
            return base.Search(d => d.AgentServer_IsDel == false);
        }

        public Manager.Model.AgentServer GetAgentServer(Guid id)
        {
            List<Manager.Model.AgentServer> agentServers = base.Search(d => d.AgentServer_Id == id && !d.AgentServer_IsDel);
            if (agentServers.Count == 1)
            {
                return agentServers.First();
            }
            else if (agentServers.Count > 1)
            {
                //TODO
                Logger.Instance(typeof(AgentServer)).Warn("");
                return agentServers.First();
            }
            else
            {
                //TODO
                throw new Exception();
            }
        }
    }
}
