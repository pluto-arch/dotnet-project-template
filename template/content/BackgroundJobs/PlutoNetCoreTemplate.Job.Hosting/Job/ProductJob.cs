namespace PlutoNetCoreTemplate.Job.Hosting.Job
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.Repositories;

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
        private readonly IRepository<Tenant> _tenants;
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantProvider _tenantProvider;
        private readonly IRepository<Product> _productsRepo;

        public ProductJob(
            ILogger<ProductJob> logger, 
            IRepository<Tenant> tenants, 
            ICurrentTenant currentTenant, 
            ITenantProvider tenantProvider,
            IRepository<Product> productsRepo)
        {
            _logger = logger;
            _tenants = tenants;
            _currentTenant = currentTenant;
            _tenantProvider = tenantProvider;
            _productsRepo = productsRepo;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var tenants = await _tenants.AsNoTracking().Include(x => x.ConnectionStrings).ToListAsync();
            if (tenants == null && !tenants.Any())
            {
                return;
            }

            foreach (var item in tenants)
            {
                var t = await _tenantProvider.InitTenant(item.Id);
                using (_currentTenant.Change(t))
                {
                    var count = await _productsRepo.CountAsync();
                    await Task.Delay(4000);
                    _logger.LogInformation("{tenant} 的产品总数：{count}", item.Id, count);
                    // TODO current Tenant data
                    int a = 1 / int.Parse("0");
                }
            }
        }


    }
}