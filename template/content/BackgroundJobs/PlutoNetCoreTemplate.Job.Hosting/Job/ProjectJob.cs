namespace PlutoNetCoreTemplate.Job.Hosting.Job
{
    using System;
    using System.Threading.Tasks;
    using Domain.Aggregates.ProductAggregate;
    using Domain.SeedWork;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ProjectJob:IJob,IBackgroundJob
    {
        private readonly ILogger<ProjectJob> _logger;
        private readonly IPlutoNetCoreTemplateBaseRepository<Product> _products;
       

        public ProjectJob(
            ILogger<ProjectJob> logger, 
            IPlutoNetCoreTemplateBaseRepository<Product> products)
        {
            _logger = logger;
            _products = products;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var count = await _products.IgnoreQueryFilters().CountAsync();
            await Task.Delay(4000);
            _logger.LogInformation("产品总数：{count}",count);
            var now = DateTime.Now;
            if (now.Second is > 10 and < 25)
            {
                // policy retry test
                throw new InvalidOperationException("ExampleJob error");
            }
            // TODO operator database
        }

       
    }
}