namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using PlutoData;

    public interface IPlutoNetCoreTemplateEfRepository<TEntity>: IEfRepository<TEntity>
        where TEntity : class,new()
    {}
}