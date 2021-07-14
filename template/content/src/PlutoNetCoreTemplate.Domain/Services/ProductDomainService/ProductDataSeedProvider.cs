namespace PlutoNetCoreTemplate.Domain.Services.ProductDomainService
{
    using Aggregates.ProductAggregate;
    using Aggregates.TenantAggregate;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using SeedWork;

    using System;
    using System.Threading.Tasks;

    public class ProductDataSeedProvider : IDataSeedProvider
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<ProductDataSeedProvider> _logger;

        public ProductDataSeedProvider(
            ICurrentTenant currentTenant,
            ILogger<ProductDataSeedProvider> logger)
        {
            _currentTenant = currentTenant;
            _logger = logger;
        }


        public int Sorts => 100;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            string[] tenantIds = new[] { "T20210602000001", "T20210602000002" };
            foreach (var tenantId in tenantIds)
            {
                using (_currentTenant.Change(tenantId, "租户一", out var scope))
                {
                    var productRepository = scope.ServiceProvider.GetService<IProductRepository>();
                    if (await productRepository.AnyAsync())
                    {
                        continue;
                    }
                    for (int i = 0; i < 40; i++)
                    {
                        var device = new Device
                        {
                            Name = $"{i}",
                            SerialNo = $"SN20210403000{i}",
                            Address = new DeviceAddress($"街道{i}", "杭州市", "浙江省", "中国", "450000"),
                            Coordinate = (GeoCoordinate)"121.2323,34.312",
                            Online = true,
                        };
                        var product = new Product
                        {
                            Name = $"{i}",
                        };
                        product.AddDevice(device);
                        await productRepository.InsertAsync(product);
                    }
                    await productRepository.Uow.SaveChangesAsync();
                }
            }
        }
    }
}