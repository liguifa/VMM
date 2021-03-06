﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public class SystemInfoMessage : Message
    {
        public List<VMSystem> VMSystyems { get; set; }

        public class VMSystem
        {
            public string Name { get; set; }

            public string Status { get; set; }

            public string CreateTime { get; set; }

            public string LastOperationTime { get; set; }

            public string System { get; set; }

            public string Memory { get; set; }

            public string Cpu { get; set; }
        }
    }
}
