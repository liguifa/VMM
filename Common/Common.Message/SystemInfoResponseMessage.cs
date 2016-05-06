using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    [DataContract]
    public class SystemInfoResponseMessage : Message
    {
        [DataMember]
        public List<VMSystem> VMSystyems { get; set; }

        [DataContract]
        public class VMSystem
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public string Status { get; set; }

            [DataMember]
            public string Cpu { get; set; }

            [DataMember]
            public string Memory { get; set; }

            [DataMember]
            public string CreateTime { get; set; }

            [DataMember]
            public string LastOperationTime { get; set; }
        }
    }
}
