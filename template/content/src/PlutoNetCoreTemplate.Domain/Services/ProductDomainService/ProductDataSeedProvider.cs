namespace PlutoNetCoreTemplate.Domain.Services.ProductDomainService
{
    using Aggregates.ProductAggregate;
    using Aggregates.TenantAggregate;

    using Microsoft.Extensions.DependencyInjection;

    using SeedWork;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductDataSeedProvider : IDataSeedProvider
    {
        private readonly ICurrentTenant _currentTenant;

        public ProductDataSeedProvider(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }


        public int Sorts => 100;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            (string id, string name)[] tenantIds = new (string id, string name)[]
             {
                 ("T20210602000001","租户一"),
                 ("T20210602000002","租户二"),
                 ("T20210602000003","租户三")
             };
            var tenantProvider = serviceProvider.GetRequiredService<ITenantProvider>();
            var repository = serviceProvider.GetRequiredService<IProductRepository>();
            TenantInfo t = null;
            foreach (var (id, _) in tenantIds)
            {
                t = await tenantProvider.InitTenant(id);
                using (_currentTenant.Change(t))
                {
                    var tenantName = _currentTenant.Name;
                    if (repository.Any())
                    {
                        continue;
                    }

                    var p = new List<Product>();
                    for (int i = 0; i < 21; i++)
                    {
                        var device = new Device
                        {
                            Name = $"{tenantName}的设备{i}",
                            SerialNo = $"SN20210403000{i}",
                            Address = new DeviceAddress($"街道{i}", "杭州市", "浙江省", "中国", "450000"),
                            Coordinate = (GeoCoordinate)"121.2323,34.312",
                            Online = true,
                        };
                        var product = new Product
                        {
                            Name = $"{tenantName}的产品{i}",
                            CreationTime = DateTimeOffset.Now
                        };
                        product.AddDevice(device);
                        p.Add(product);
                    }
                    await repository.InsertAsync(p, true);
                }
            }
        }
    }
}