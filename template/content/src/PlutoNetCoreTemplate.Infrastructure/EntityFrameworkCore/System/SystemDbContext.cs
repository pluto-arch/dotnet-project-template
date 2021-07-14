namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Entities;
    using EntityTypeConfigurations;
    using Extensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    using PlutoNetCoreTemplate.Domain.Aggregates.DemoTree;

    using Providers;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    public class SystemDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public SystemDbContext(DbContextOptions<SystemDbContext> options)
            : base(options)
        {
            _mediator = this.GetInfrastructure().GetService<IMediator>() ?? NullMediatorProvider.GetNullMediator();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SystemEntityTypeConfiguration.TenantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SystemEntityTypeConfiguration.TenantConnectionStringEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SystemEntityTypeConfiguration.PermissionGroupDefinitionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SystemEntityTypeConfiguration.PermissionDefinitionEntityTypeConfiguration());



            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.HasOne(x => x.Parent)
                    .WithMany(x => x.SubFolders)
                    .HasForeignKey(x => x.ParentId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });



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