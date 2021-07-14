namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using EntityFrameworkCore.Extension.UnitOfWork;

    public interface ISystemBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    { }
}