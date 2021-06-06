namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.SeedWork;
    using global::EntityFrameworkCore.Extension;

    /// <summary>
    /// 系统基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SystemBaseRepository<TEntity> : Repository<SystemDbContext,TEntity>,ISystemBaseRepository<TEntity> where TEntity : class,new()
    {
        public SystemBaseRepository(SystemDbContext dbContext) : base(dbContext)
        {
        }
    }
}