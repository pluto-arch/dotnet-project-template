namespace PlutoNetCoreTemplate.Domain.Events.Products
{
    using Aggregates.ProductAggregate;
    using MediatR;

    public class CreateProductDomainEvent: INotification
    {
        public Product Product { get; private set; }

        public CreateProductDomainEvent(Product product) => Product = product;
    }
}