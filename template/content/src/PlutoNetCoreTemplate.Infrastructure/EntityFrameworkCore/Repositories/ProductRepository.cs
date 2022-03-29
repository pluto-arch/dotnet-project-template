namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Repositories
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.UnitOfWork;

    using System.Collections.Generic;

    public class ProductRepository : EFCoreRepository<DeviceCenterDbContext, Product, string>, IProductRepository
    {
        public ProductRepository(IUnitOfWork<DeviceCenterDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public IAsyncEnumerable<Product> GetListAsync()
        {
            return DbSet.AsAsyncEnumerable();
        }
    }
}