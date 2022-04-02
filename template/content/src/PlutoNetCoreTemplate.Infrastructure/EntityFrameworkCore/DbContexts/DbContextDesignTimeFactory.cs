namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.UnitOfWork;

    using EntityTypeConfigurations;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DeviceCenterMigrationDbContext>
    {
        public DeviceCenterMigrationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeviceCenterMigrationDbContext>();
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=Pnct_Default;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            return new DeviceCenterMigrationDbContext(optionsBuilder.Options);
        }
    }



    public class SystemDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SystemMigrationDbContext>
    {
        public SystemMigrationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SystemMigrationDbContext>();
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=Pnct_System;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            return new SystemMigrationDbContext(optionsBuilder.Options);
        }
    }


    #region migration dbcontext
    public class DeviceCenterMigrationDbContext : DbContext, IUowDbContext
    {

        public DeviceCenterMigrationDbContext(DbContextOptions<DeviceCenterMigrationDbContext> options)
            : base(options)
        {
        }

        #region Entitys and configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration())
                .ApplyConfiguration(new DeviceEntityTypeConfiguration())
                .ApplyConfiguration(new DeviceEntityTypeConfiguration.DeviceTagEntityTypeConfiguration())
                .ApplyConfiguration(new DeviceEntityTypeConfiguration.ProductEntityTypeConfiguration())
                .ApplyConfiguration(new ProjectEntityTypeConfiguration());
        }
        #endregion
    }

    public class SystemMigrationDbContext : DbContext, IUowDbContext
    {

        public SystemMigrationDbContext(DbContextOptions<SystemMigrationDbContext> options)
            : base(options)
        {
        }


        #region Entitys and configuration

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SystemEntityTypeConfiguration.TenantEntityTypeConfiguration())
                .ApplyConfiguration(new SystemEntityTypeConfiguration.TenantConnectionStringEntityTypeConfiguration())
                .ApplyConfiguration(new SystemEntityTypeConfiguration.PermissionGroupDefinitionEntityTypeConfiguration())
                .ApplyConfiguration(new SystemEntityTypeConfiguration.PermissionDefinitionEntityTypeConfiguration());
        }
        #endregion
    }

    #endregion
}