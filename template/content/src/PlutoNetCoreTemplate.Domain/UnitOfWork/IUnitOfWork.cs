using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.UnitOfWork
{
    using Entities;
    using Repositories;
    using System;



    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

        TRep GetCustomRepository<TRep>() where TRep : IRepository;
    }

    public interface IUnitOfWork<T> : IUnitOfWork where T : IUowDbContext
    {
        T Context { get; }

        T GetDbContext();
    }
}