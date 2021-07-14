namespace PlutoNetCoreTemplate.Infrastructure.Repositories
{
    using Domain.Aggregates.ProductAggregate;
    using global::EntityFrameworkCore.Extension.UnitOfWork;

    public class ProductRepository : Repository<PlutoNetTemplateDbContext, Product>, IProductRepository
    {
        public ProductRepository(PlutoNetTemplateDbContext dbContext) : base(dbContext)
        {
        }
    }
}