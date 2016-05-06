using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.BLL
{
    public class Job : BaseBLL<Manager.Model.Job>
    {
        public List<JobModel> GetJobs(string pageIndex, string pageSize)
        {
            List<JobModel> jobs = new List<JobModel>();
            foreach (var job in base.Search(d => d.Job_Id != null))
            {
                JobModel jobModel = new JobModel()
                {
                    Id = job.Job_Id.ToString(),
                    Progress = 10,
                    Commit = ""
                };
                jobs.Add(jobModel);
            }
            return jobs;
        }

        public void AddJob(int type)
        {
            Manager.Model.Job job = new Model.Job();
            job.Job_Id = Guid.NewGuid();
            job.Job_Status = false;
            job.Job_Type = type;
            base.Add(job);
        }
    }

    public class JobModel
    {
        public string Id { get; set; }

        public int Progress { get; set; }

        public string Commit { get; set; }
    }
}
