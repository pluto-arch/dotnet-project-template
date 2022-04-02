namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Entities;
    using Domain.Repositories;
    using Domain.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.TenantAggregate;

    public class EFCoreUnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IUowDbContext
    {
        private TContext _currenDbContext;
        private readonly TContext _rootContext;
        private IServiceProvider _serviceProvider;
        private bool disposedValue;
        private readonly IServiceProvider _rootServiceProvider;

        public TContext Context => GetDbContext();

        public EFCoreUnitOfWork(TContext ctx, IServiceProvider serviceProvider,ICurrentTenantAccessor tenantAccessor)
        {
            _currenDbContext = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _rootContext = ctx;
            _rootServiceProvider = serviceProvider;
            tenantAccessor.OnScopeBind += OnScopeChange;
        }


        private async Task OnScopeChange(IServiceScope scoped)
        {
            if (scoped==null)
            {
                _currenDbContext = _rootContext;
                _serviceProvider = _rootServiceProvider;
                await Task.CompletedTask;
                return;
            }
            _serviceProvider = scoped.ServiceProvider;
            _currenDbContext = _serviceProvider.GetRequiredService<TContext>();
            await Task.CompletedTask;
        }


        public TContext GetDbContext()
        {
            return _currenDbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _currenDbContext.SaveChangesAsync(cancellationToken);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            return _serviceProvider.GetService<IRepository<TEntity>>();
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return _serviceProvider.GetService<IRepository<TEntity, TKey>>();
        }


        public TRep GetCustomRepository<TRep>() where TRep : IRepository
        {
            return _serviceProvider.GetService<TRep>();
        }





        public async ValueTask DisposeAsync()
        {
            await this._currenDbContext.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    this._currenDbContext.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~EFCoreUnitOfWork()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}