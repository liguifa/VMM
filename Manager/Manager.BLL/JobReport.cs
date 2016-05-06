using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.BLL
{
    public class JobReport : BaseBLL<Model.JobReport>
    {
        public void AddCommit(string jobId, int progress, string commit)
        {
            Model.JobReport report = new Model.JobReport();
            report.JobReport_Id = Guid.NewGuid();
            report.JobReport_JobId = Guid.Parse(jobId);
            report.JobReport_Progress = progress;
            report.JobReport_Commit = commit;
        }
    }
}
