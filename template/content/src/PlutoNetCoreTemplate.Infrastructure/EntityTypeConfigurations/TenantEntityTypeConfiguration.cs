using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

namespace PlutoNetCoreTemplate.Infrastructure.EntityTypeConfigurations
{
    public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
            builder.HasMany(e => e.ConnectionStrings).WithOne().HasForeignKey(e => e.TenantId).IsRequired();
            builder.HasIndex(u => u.Name);
        }
    }


    public class TenantConnectionStringEntityTypeConfiguration : IEntityTypeConfiguration<TenantConnectionString>
    {
        public void Configure(EntityTypeBuilder<TenantConnectionString> builder)
        {
            builder.ToTable("TenantConnectionStrings");
            builder.HasKey(x => new { x.TenantId, x.Name });
            builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
            builder.Property(e => e.Value).IsRequired().HasMaxLength(1024);
        }
    }
}
