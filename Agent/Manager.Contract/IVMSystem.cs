using Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Manager.Contract
{
    [ServiceContract(Namespace = "Manager.Contract")]
    public interface IVMSystem
    {
        [OperationContract]
        SystemResponseMessage GetSystemStatus(SystemRquestMessage request);
    }
}
