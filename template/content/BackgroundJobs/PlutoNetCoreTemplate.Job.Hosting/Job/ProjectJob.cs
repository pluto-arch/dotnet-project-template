namespace PlutoNetCoreTemplate.Job.Hosting.Job
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public class ProjectJob:IJob,IBackgroundJob
    {
        private readonly ILogger<ProjectJob> _logger;
        private readonly ISystemBaseRepository<Tenant> _tenants;
        private readonly ICurrentTenant _currentTenant;
       

        public ProjectJob(
            ILogger<ProjectJob> logger, ISystemBaseRepository<Tenant> tenants, ICurrentTenant currentTenant)
        {
            _logger = logger;
            _tenants = tenants;
            _currentTenant = currentTenant;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var tenants = await _tenants.GetListAsync();
            if (tenants==null&&!tenants.Any())
            {
                return;
            }

            foreach (var item in tenants)
            {
                using (_currentTenant.Change(item.Id,item.Name,out var scope))
                {
                    var productRep = scope.ServiceProvider.GetService<IPlutoNetCoreTemplateBaseRepository<Product>>();
                    var count = await productRep.CountAsync();
                    await Task.Delay(4000);
                    _logger.LogInformation("{tenant} 的产品总数：{count}",item.Id,count);
                    // TODO current Tenant data
                }
            }
        }

       
    }
}