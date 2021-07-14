namespace PlutoNetCoreTemplate.Job.Hosting.Infrastructure
{
    using EntityFrameworkCore.Extension.UnitOfWork.Uows;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Models;
    using PlutoNetCoreTemplate.Infrastructure;
    using Polly;
    using Quartz;
    using System;
    using System.Threading.Tasks;
    using LogContext = Serilog.Context.LogContext;

    public class QuartzJobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QuartzJobRunner> _logger;
        /// <summary>
        /// 单个任务异常重试次数
        /// </summary>
        private readonly int _retryCount = 0;
        /// <summary>
        /// 重试时间间隔 单位秒
        /// </summary>
        private readonly int _retryAttempt = 0;

        public QuartzJobRunner(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<QuartzJobRunner> logger)
        {
            _serviceProvider = serviceProvider;
            _retryCount = configuration.GetValue<int>("JobRetry:RetryCount");
            _retryAttempt = configuration.GetValue<int>("JobRetry:RetryAttempt");
            _logger = logger;
        }


        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context)
        {
            var jobType = context.JobDetail.JobType;
            using (LogContext.PushProperty("JobType", jobType.Name))
            using (var scope = _serviceProvider.CreateScope())
            {
                var job = context.JobDetail.Key;
                var jobInfoStore = scope.ServiceProvider.GetService<IJobInfoStore>() ?? new InMemoryJobStore();
                var jobLogStore = scope.ServiceProvider.GetService<IJobLogStore>() ?? new InMemoryJobLog();
                try
                {
                    var policy = Policy.Handle<Exception>()
                        .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(_retryAttempt), async (ex, time) =>
                            {
                                _logger.LogWarning("{jobType} has an error : {ex}. retry after {time}s ", jobType.Name, ex.Message, time);
                                await jobLogStore.RecordAsync(job, new JobLogModel
                                {
                                    Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                                    RunSeconds = context.JobRunTime.Seconds,
                                    State = EnumJobStates.Exception,
                                    Message = $"出现异常{ex.Message}，{time}后重试"
                                });
                            }
                        );
                    await policy.ExecuteAsync(async () =>
                    {
                        var job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;
                        if (job is null)
                        {
                            _logger.LogWarning("no {jobType} found !", jobType.Name);
                        }
                        else
                        {
                            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork<PlutoNetTemplateDbContext>>();
                            _logger.LogInformation("{jobType} executing...", jobType.Name);
                            await job.Execute(context);
                            await uow.SaveChangesAsync();
                            _logger.LogInformation("{jobType} has been executed, and all changes has been saved", jobType.Name);
                        }
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "job running has an error : {@message}", e.Message);
                    await jobLogStore.RecordAsync(job, new JobLogModel
                    {
                        Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                        RunSeconds = context.JobRunTime.Seconds,
                        State = EnumJobStates.Exception,
                        Message = e?.Message
                    });
                    var jobModel = await jobInfoStore.GetAsync(job);
                    jobModel.Status = EnumJobStates.Exception;
                    await jobInfoStore.UpdateAsync(jobModel);
                    await context.Scheduler.PauseJob(job);
                }
            }
        }

    }
}