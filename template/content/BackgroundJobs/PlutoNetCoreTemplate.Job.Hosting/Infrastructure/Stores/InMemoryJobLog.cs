namespace PlutoNetCoreTemplate.Job.Hosting.Infrastructure
{
    using Models;

    using Quartz;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class InMemoryJobLog : IJobLogStore
    {
        private const int QUEUE_LENGTH = 20;
        private static readonly Dictionary<string, FixLengthQueue> JobLog = new();

        public Task RecordAsync(JobKey job, JobLogModel model)
        {
            var key = $"{job.Group}_{job.Name}";
            if (!JobLog.ContainsKey(key))
            {
                JobLog[key] = new FixLengthQueue(QUEUE_LENGTH);
            }
            JobLog[key].Enqueue(model);
            return Task.CompletedTask;
        }

        public Task<List<JobLogModel>> GetListAsync(JobKey job, int count = 20)
        {
            var key = $"{job.Group}_{job.Name}";
            if (!JobLog.ContainsKey(key))
            {
                return Task.FromResult(new List<JobLogModel>());
            }
            var logs = JobLog[key].ToArray();
            var res = logs.OrderByDescending(x => ((JobLogModel)x)?.Time).Take(count).Select(x => (JobLogModel)x).ToList();
            return Task.FromResult(res);
        }
    }

}