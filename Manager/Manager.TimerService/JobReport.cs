using Manager.BLL;
using Manager.TimrContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.TimerService
{
    public class JobReport : IJobReport
    {
        public void AddJobCommit(string jobId, int progress, string commit)
        {
            new Manager.BLL.JobReport().AddCommit(jobId, progress, commit);
        }
    }
}
