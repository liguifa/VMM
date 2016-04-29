using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Manager.Contract
{
    public class Config
    {
        private static readonly object mSyncRoot = new object();
        private static Config mConfig;

        private Config()
        {
        }

        public static Config GetInstance(string config)
        {
            if (mConfig == null)
            {
                lock (mSyncRoot)
                {
                    if (mConfig == null)
                    {
                        using (FileStream stream = new FileStream(config, FileMode.Open, FileAccess.Read))
                        {
                            var seralizer = new XmlSerializer(typeof(Config));
                            mConfig = seralizer.Deserialize(stream) as Config;
                        }
                    }
                }
            }
            return mConfig;
        }

        public List<VMMachine> Machines { get; set; }
    }

    public class VMMachine
    {
        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}

