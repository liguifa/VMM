﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string Context { get; set; }

        [DataMember]
        public object Append { get; set; }
    }
}
