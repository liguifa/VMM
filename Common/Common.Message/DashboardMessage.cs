﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public class DashboardMessage : Message
    {
        public int MyActiveSystemCount { get; set; }

        public int MyShutdownSystemCount { get; set; }

        public int ActiveSystem { get; set; }

        public int ShutdownSystemCount { get; set; }
    }
}
