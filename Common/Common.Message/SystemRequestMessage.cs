using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Common.Message
{
    [DataContract]
    public class SystemRquestMessage : Message
    {
        [DataMember]
        public List<string> Names { get; set; }

        [DataMember]
        public string Key { get; set; }
    }
}
