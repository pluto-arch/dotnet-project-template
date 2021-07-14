namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using EntityFrameworkCore.Extension.UnitOfWork;

    public interface IPlutoNetCoreTemplateBaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    { }
}