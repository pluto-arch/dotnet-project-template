namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using PlutoData;

    public interface IPlutoNetCoreTemplateDapperRepository<TEntity>:IBaseDapperRepository<TEntity>
        where TEntity:class,new() 
    {
        
    }
}