namespace PlutoNetCoreTemplate.Application.Command
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.ProductAggregate;
    using Domain.Events.Products;
    using Domain.SeedWork;
    using MediatR;
    using PlutoData;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
    {
        private readonly IPlutoNetCoreTemplateBaseRepository<Product> _productsRepository;

        public CreateProductCommandHandler(IPlutoNetCoreTemplateBaseRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }


        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var model = new Product
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = request.ProductName,
                Remark = "备注",
            };
            model.AddDomainEvent(new CreateProductDomainEvent(model));
            await _productsRepository.InsertAsync(model,cancellationToken:cancellationToken);
            return Unit.Value;
        }
    }
}