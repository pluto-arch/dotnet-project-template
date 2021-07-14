namespace PlutoNetCoreTemplate.Application.Command
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;

    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;

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
            await _productsRepository.InsertAsync(model, cancellationToken: cancellationToken);
            await _productsRepository.Uow.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}