namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.SeedWork;
    using PlutoData;

    public class PlutoNetCoreTemplateEfRepository<TEntity> : EfRepository<EfCoreDbContext, TEntity>, IPlutoNetCoreTemplateEfRepository<TEntity>
        where TEntity : class,new()
    {
        public PlutoNetCoreTemplateEfRepository(EfCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
