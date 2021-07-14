namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.SeedWork;
    using global::EntityFrameworkCore.Extension.UnitOfWork;

    public class PlutoNetCoreTemplateBaseRepository<TEntity> : Repository<PlutoNetTemplateDbContext, TEntity>, IPlutoNetCoreTemplateBaseRepository<TEntity>
        where TEntity : class, new()
    {
        public PlutoNetCoreTemplateBaseRepository(PlutoNetTemplateDbContext dbContext) : base(dbContext)
        {
        }
    }
}
