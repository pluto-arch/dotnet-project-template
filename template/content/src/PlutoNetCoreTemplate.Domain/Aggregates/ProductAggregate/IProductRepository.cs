namespace PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate
{
    using Repositories;

    using System.Collections.Generic;

    public interface IProductRepository : IRepository<Product>
    {
        IAsyncEnumerable<Product> GetListAsync();
    }
}