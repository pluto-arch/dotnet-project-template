namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PlutoData;

    public class PlutoNetCoreTemplateEfRepository<TEntity> : EfRepository<EfCoreDbContext, TEntity>
        where TEntity : class
    {
        public PlutoNetCoreTemplateEfRepository(EfCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
