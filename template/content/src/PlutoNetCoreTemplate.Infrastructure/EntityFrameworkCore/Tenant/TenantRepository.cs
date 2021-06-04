namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.SeedWork;
    using global::EntityFrameworkCore.Extension;

    /// <summary>
    /// 租户基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class TenantRepository<TEntity> : Repository<TenantDbContext,TEntity>,ITenantRepository<TEntity> where TEntity : class,new()
    {
        public TenantRepository(TenantDbContext dbContext) : base(dbContext)
        {
        }
    }
}