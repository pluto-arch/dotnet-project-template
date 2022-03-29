﻿namespace PlutoNetCoreTemplate.Job.Hosting.Job
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.Repositories;

    using Infrastructure;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using Quartz;

    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class DeviceJob : IJob, IBackgroundJob
    {
        private readonly ILogger<DeviceJob> _logger;
        private readonly IRepository<Device> _devices;

        public DeviceJob(
            ILogger<DeviceJob> logger,
            IRepository<Device> devices)
        {
            _logger = logger;
            _devices = devices;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var count = await _devices.IgnoreQueryFilters().CountAsync();
            await Task.Delay(1000);
            _logger.LogInformation("设备总数：{count}", count);
            // TODO operator database
        }
    }
}