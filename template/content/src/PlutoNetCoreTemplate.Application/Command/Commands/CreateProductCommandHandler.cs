namespace PlutoNetCoreTemplate.Application.Command
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.Events.Products;
    using Domain.SeedWork;
    using MediatR;
    using PlutoData;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
    {
        private readonly IPlutoNetCoreTemplateBaseRepository<Product> _productsRepository;
        private readonly ICurrentTenant _currentTenant;

        public CreateProductCommandHandler(IPlutoNetCoreTemplateBaseRepository<Product> productsRepository, ICurrentTenant currentTenant)
        {
            _productsRepository = productsRepository;
            _currentTenant = currentTenant;
        }


        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            using (_currentTenant.Change("T20210602000002","租户一",out var scope))
            using (scope)
            {
                var repository = scope.ServiceProvider.GetService<IPlutoNetCoreTemplateBaseRepository<Product>>();
                var t = _currentTenant.Id;
                for (int i = 0; i < 30; i++)
                {
                    var model = new Product
                    {
                        Id = $"{i}",
                        Name = request.ProductName,
                        Remark = "备注",
                        Devices = new List<Device>
                        {
                            new Device
                            {
                                Name = $"{i}",
                                SerialNo = $"SN20210403000{i}",
                                Address = new DeviceAddress($"街道{i}", "杭州市", "浙江省", "中国", "450000"),
                                Coordinate = (GeoCoordinate)"121.2323,34.312",
                                Online = true,
                            }
                        }
                    };
                    await repository?.InsertAsync(model,true,cancellationToken:cancellationToken);
                }
            }

            var ten = _currentTenant.Id;
            for (int i = 0; i < 30; i++)
            {
                var model = new Product
                {
                    Id = $"{i}",
                    Name = request.ProductName,
                    Remark = "备注",
                    Devices = new List<Device>
                    {
                        new Device
                        {
                            Name = $"{i}",
                            SerialNo = $"SN20210403000{i}",
                            Address = new DeviceAddress($"街道{i}", "杭州市", "浙江省", "中国", "450000"),
                            Coordinate = (GeoCoordinate)"121.2323,34.312",
                            Online = true,
                        }
                    }
                };
                await _productsRepository.InsertAsync(model,true,cancellationToken:cancellationToken);
            }

            return Unit.Value;
        }
    }
}