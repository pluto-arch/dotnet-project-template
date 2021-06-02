namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using EntityFrameworkCore.Extension;

    public interface IPlutoNetCoreTemplateBaseRepository<TEntity>: IRepository<TEntity>
        where TEntity : class,new()
    {}
}