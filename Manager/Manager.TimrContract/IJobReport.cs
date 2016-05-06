using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.TimrContract
{
    [ServiceContract]
    public interface IJobReport
    {
        [OperationContract]
        void AddJobCommit(string jobId, int progress, string commit);
    }
}
