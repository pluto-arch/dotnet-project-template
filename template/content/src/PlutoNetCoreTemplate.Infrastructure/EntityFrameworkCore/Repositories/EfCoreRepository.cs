using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Repositories
{
    using Domain.UnitOfWork;

    using Microsoft.EntityFrameworkCore;

    public class EFCoreRepository<TContext, TEntity> : EFCoreBaseRepository<TContext, TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext, IUowDbContext
    {
        public EFCoreRepository(IUnitOfWork<TContext> unitOfWork) : base(unitOfWork)
        {
        }
    }


    public class EFCoreRepository<TContext, TEntity, TKey> : EFBaseRepository<TContext, TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TContext : DbContext, IUowDbContext
    {
        public EFCoreRepository(IUnitOfWork<TContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}