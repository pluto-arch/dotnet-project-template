
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.Entities;
using PlutoNetCoreTemplate.Infrastructure.Extensions;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Infrastructure
{
    using EntityTypeConfigurations;
    using MediatR;
    using Providers;

    public class PlutoNetTemplateDbContext : DbContext
    {

        private readonly IMediator _mediator;

        private readonly ITenantProvider _tenantProvider;

        public PlutoNetTemplateDbContext(DbContextOptions<PlutoNetTemplateDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
            _mediator = this.GetInfrastructure().GetService<IMediator>() ?? NullMediatorProvider.GetNullMediator();
        }

        #region Entitys and configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration.DeviceTagEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration.ProductEntityTypeConfiguration());



            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                // 多租户
                if (item.ClrType.IsAssignableTo(typeof(IMultiTenant)))
                {
                    modelBuilder.Entity(item.ClrType).AddQueryFilter<IMultiTenant>(e => e.TenantId == _tenantProvider.GetTenantId());
                }


                // 软删除
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

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
