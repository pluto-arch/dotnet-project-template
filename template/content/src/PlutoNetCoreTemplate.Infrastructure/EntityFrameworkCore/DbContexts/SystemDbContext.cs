namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Aggregates.SystemAggregate;
    using Microsoft.EntityFrameworkCore;
    using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
    using PlutoNetCoreTemplate.Domain.UnitOfWork;
    using EntityTypeConfigurations;


    public class SystemDbContext : PlutoDbContext<SystemDbContext>, IUowDbContext
    {

        public SystemDbContext(DbContextOptions<SystemDbContext> options)
            : base(options)
        {
        }


        #region Entitys and configuration

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionString { get; set; }
        public DbSet<PermissionGroupDefinition> PermissionGroupDefinition { get; set; }
        public DbSet<PermissionDefinition> PermissionDefinition { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);// 不能删除，软删除，多租户过滤器
            modelBuilder.ApplyConfiguration(new SystemEntityTypeConfiguration.TenantEntityTypeConfiguration())
                .ApplyConfiguration(new SystemEntityTypeConfiguration.TenantConnectionStringEntityTypeConfiguration())
                .ApplyConfiguration(new SystemEntityTypeConfiguration.PermissionGroupDefinitionEntityTypeConfiguration())
                .ApplyConfiguration(new SystemEntityTypeConfiguration.PermissionDefinitionEntityTypeConfiguration());
        }
        #endregion
    }
}