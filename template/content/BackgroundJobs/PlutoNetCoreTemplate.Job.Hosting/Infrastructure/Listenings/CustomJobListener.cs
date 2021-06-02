namespace PlutoNetCoreTemplate.Job.Hosting.Infrastructure.Listenings
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Quartz;



    public class NullJobListener : IJobListener
    {
        /// <inheritdoc />
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public string Name => "CustomerJobListener";
    }




    public class CustomJobListener:IJobListener
    {
        private readonly IJobLogStore _jobLogStore;
        public CustomJobListener(IJobLogStore jobLogStore)
        {
            _jobLogStore = jobLogStore;
        }


        /// <inheritdoc />
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var job = context.JobDetail.Key;
            bool hasException = jobException != null;
            _jobLogStore.RecordAsync(job,new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = context.JobRunTime.Seconds,
                State = hasException?EnumJobStates.Exception:EnumJobStates.Normal,
                Message = jobException?.Message
            });
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public string Name => "CustomerJobListener";
    }
}