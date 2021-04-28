namespace PlutoNetCoreTemplate.Domain.Services.ProductDomainService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Aggregates.ProductAggregate;
    using Microsoft.EntityFrameworkCore;
    using SeedWork;

    public class ProductDataSeedProvider:IDataSeedProvider
    {
        private readonly IProductRepository _productRepository;

        public ProductDataSeedProvider(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public int Sorts => 100;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            List<Product> list = new ();
            if (await _productRepository.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            for (int i = 0; i < 20; i++)
            {
                list.Add(new Product
                {
                    Name = $"product_{i}",
                    Devices = new List<Device>
                    {
                        new Device
                        {
                            Name = $"product_{i}_device_{i}",
                            SerialNo = $"SN20210403000{i}",
                            Address = new DeviceAddress($"街道{i}","杭州市","浙江省","中国","450000"),
                            Coordinate = (GeoCoordinate)"121.2323,34.312",
                            Online = true,
                        }
                    },
                    Remark = $"备注——{i}"
                });
            }
            await _productRepository.InsertAsync(list, true);
        }
    }
}