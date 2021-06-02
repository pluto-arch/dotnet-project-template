namespace PlutoNetCoreTemplate.Job.Hosting.Job
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Aggregates.ProductAggregate;
    using Domain.SeedWork;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public class DeviceJob:IJob,IBackgroundJob
    {
        private readonly ILogger<DeviceJob> _logger;
        private readonly IPlutoNetCoreTemplateBaseRepository<Device> _devices;

        public DeviceJob(
            ILogger<DeviceJob> logger, 
            IPlutoNetCoreTemplateBaseRepository<Device> devices)
        {
            _logger = logger;
            _devices = devices;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var count = await _devices.IgnoreQueryFilters().CountAsync();
            await Task.Delay(1000);
            _logger.LogInformation("设备总数：{count}",count);
            // TODO operator database
        }
    }
}