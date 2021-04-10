namespace PlutoNetCoreTemplate.Infrastructure.DapperCore
{
    using Domain.SeedWork;
    using PlutoData;

    public class PlutoNetCoreTemplateDapperRepository<TEntity> :
        BaseDapperRepository<PlutoNetCoreDapperDbContext, TEntity>, IPlutoNetCoreTemplateDapperRepository<TEntity>
        where TEntity:class,new() 
    {
        /// <inheritdoc />
        public PlutoNetCoreTemplateDapperRepository(PlutoNetCoreDapperDbContext dapperDb) : base(dapperDb)
        {
        }
    }

}