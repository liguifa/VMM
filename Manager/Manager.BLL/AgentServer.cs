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
    }
}
