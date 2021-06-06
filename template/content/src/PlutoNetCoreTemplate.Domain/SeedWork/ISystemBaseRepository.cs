namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using EntityFrameworkCore.Extension;

    public interface ISystemBaseRepository<TEntity>:IRepository<TEntity> where TEntity : class,new()
    {}
}