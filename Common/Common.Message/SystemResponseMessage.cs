using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    [DataContract]
    public class SystemResponseMessage : Message
    {
        [DataMember]
        public Dictionary<string, object> SystemInfo { get; set; }
    }
}
