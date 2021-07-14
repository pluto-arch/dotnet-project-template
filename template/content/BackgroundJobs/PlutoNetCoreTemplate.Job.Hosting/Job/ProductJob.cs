namespace PlutoNetCoreTemplate.Job.Hosting.Job
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;

    using Infrastructure;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Quartz;

    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class ProductJob : IJob, IBackgroundJob
    {
        private readonly ILogger<ProductJob> _logger;
        private readonly ISystemBaseRepository<Tenant> _tenants;
        private readonly ICurrentTenant _currentTenant;


        public ProductJob(
            ILogger<ProductJob> logger, ISystemBaseRepository<Tenant> tenants, ICurrentTenant currentTenant)
        {
            _logger = logger;
            _tenants = tenants;
            _currentTenant = currentTenant;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var tenants = await _tenants.GetListAsync();
            if (tenants == null && !tenants.Any())
            {
                return;
            }

            foreach (var item in tenants)
            {
                using (_currentTenant.Change(item.Id, item.Name, out var scope))
                {
                    var productRep = scope.ServiceProvider.GetService<IPlutoNetCoreTemplateBaseRepository<Product>>();
                    var count = await productRep.CountAsync();
                    await Task.Delay(4000);
                    _logger.LogInformation("{tenant} 的产品总数：{count}", item.Id, count);
                    // TODO current Tenant data
                }
            }
        }


    }
}