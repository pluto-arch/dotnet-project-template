namespace PlutoNetCoreTemplate.Application.Command
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.Events.Products;
    using Domain.Repositories;

    using MediatR;

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
    {
        private readonly IProductRepository _productsRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IRepository<Tenant> _tenantRep;

        public CreateProductCommandHandler(IProductRepository productsRepository, ICurrentTenant currentTenant, IRepository<Tenant> tenantRep)
        {
            _productsRepository = productsRepository;
            _currentTenant = currentTenant;
            _tenantRep = tenantRep;
        }


        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var model = new Product
            {
                Name = request.ProductName,
                Remark = "备注哈哈哈",
            };
            var e = await _productsRepository.InsertAsync(model, true, cancellationToken: cancellationToken);
            model.AddDomainEvent(new CreateProductDomainEvent(e));
            var t = await _tenantRep.FirstOrDefaultAsync(x => x.Id == _currentTenant.Id, cancellationToken);
            if (t != null)
            {
                t.CreateTime = DateTime.Parse("2021-10-10 10:10:10");
                await _tenantRep.UpdateAsync(t, true, cancellationToken: cancellationToken);
            }
            return Unit.Value;
        }
    }
}