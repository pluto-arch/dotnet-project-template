namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using EntityFrameworkCore.Extension;

    public interface ITenantRepository<TEntity>:IRepository<TEntity> where TEntity : class,new()
    {}
}