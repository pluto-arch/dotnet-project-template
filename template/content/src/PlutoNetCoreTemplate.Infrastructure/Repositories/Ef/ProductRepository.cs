namespace PlutoNetCoreTemplate.Infrastructure.Repositories
{
    using Domain.Aggregates.ProductAggregate;
    using PlutoData;

    public class ProductRepository : EfRepository<EfCoreDbContext, Product>, IProductRepository
    {
        public ProductRepository(EfCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}