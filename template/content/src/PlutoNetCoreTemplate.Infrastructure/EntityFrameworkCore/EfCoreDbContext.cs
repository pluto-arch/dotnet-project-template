
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.Entities;
using PlutoNetCoreTemplate.Infrastructure.Extensions;

namespace PlutoNetCoreTemplate.Infrastructure
{
    using Domain.Aggregates.System;
    using MediatR;
    using Providers;

    public class EfCoreDbContext : DbContext
    {

        private readonly IMediator _mediator;

        public EfCoreDbContext(DbContextOptions<EfCoreDbContext> options)
            : base(options)
        {
            _mediator=this.GetInfrastructure().GetService<IMediator>() ?? NullMediatorProvider.GetNullMediator();
        }

        #region Entitys and configuration  (OnModelCreating中配置了对应Entity 那么对应DbSet<>可以不写)
        public DbSet<UserEntity> Users { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // 查询过滤器
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                // clr type实现了 多租户接口
                if (item.ClrType.IsAssignableTo(typeof(IMultiTenant)))
                {
                    var currentTenant = this.GetInfrastructure().GetService<ICurrentTenant>(); ;
                    if (!string.IsNullOrEmpty(currentTenant?.Id) && !string.IsNullOrWhiteSpace(currentTenant?.Id))
                    {
                        modelBuilder.Entity(item.ClrType).AddQueryFilter<IMultiTenant>(e => e.TenantId == currentTenant.Id);
                    }
                }

                // 实现了软删除的接口
                if (item.ClrType.IsAssignableTo(typeof(ISoftDelete)))
                {
                    modelBuilder.Entity(item.ClrType).AddQueryFilter<ISoftDelete>(e => !e.Deleted);
                }
            }
        }
        #endregion




        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            #region 软删除
            var deletedEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Deleted && entry.Entity is ISoftDelete);
            deletedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Reload();
                entityEntry.State = EntityState.Modified;
                ((ISoftDelete)entityEntry.Entity).Deleted = true;
            });
            #endregion

            #region 集成事件
            await _mediator.DispatchDomainEventsAsync(this, cancellationToken);
            #endregion

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
