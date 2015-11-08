using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public class Message
    {
        public bool Status { get; set; }

        public string Context { get; set; }

        public object Append { get; set; }
    }
}
