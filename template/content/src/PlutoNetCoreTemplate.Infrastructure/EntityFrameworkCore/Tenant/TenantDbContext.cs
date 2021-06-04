namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.TenantAggregate;
    using Domain.Entities;
    using EntityTypeConfigurations;
    using Extensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;


    /// <summary>
    /// 租户DB上下文
    /// </summary>
    public class TenantDbContext: DbContext
    {
        private readonly IMediator _mediator;

        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
            _mediator=this.GetInfrastructure().GetService<IMediator>() ?? NullMediatorProvider.GetNullMediator();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TenantConnectionStringEntityTypeConfiguration());
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                // 软删除
                if (item.ClrType.IsAssignableTo(typeof(ISoftDelete)))
                {
                    modelBuilder.Entity(item.ClrType).AddQueryFilter<ISoftDelete>(e => !e.Deleted);
                }
            }
        }


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