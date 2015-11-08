using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Contract
{
    public interface ISystemStatus
    {
        Dictionary<string, bool> GetSystemStatus(List<string> names);
    }
}
