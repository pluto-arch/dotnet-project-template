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
            var model = new Product
            {
                Name = request.ProductName,
                Remark = "备注哈哈哈",
            };
            await _productsRepository.InsertAsync(model,cancellationToken:cancellationToken);
            await _productsRepository.Uow.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}