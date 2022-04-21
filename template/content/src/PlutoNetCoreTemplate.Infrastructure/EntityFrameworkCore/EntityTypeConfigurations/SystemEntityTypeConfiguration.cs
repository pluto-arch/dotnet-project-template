using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

namespace PlutoNetCoreTemplate.Infrastructure.EntityTypeConfigurations
{
    using Domain.Aggregates.SystemAggregate;

    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    using System.Collections.Generic;
    using System.Text.Json;

    public static class SystemEntityTypeConfiguration
    {
        /// <summary>
        /// 租户
        /// </summary>
        public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant>
        {
            public void Configure(EntityTypeBuilder<Tenant> builder)
            {
                builder.ToTable("Tenants");
                builder.HasKey(e => e.Id);
                builder.Property(x => x.Id).ValueGeneratedNever();
                builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
                builder.Property(e => e.CreateTime).HasDefaultValueSql("GETDATE()");
                builder.HasMany(e => e.ConnectionStrings).WithOne().HasForeignKey(e => e.TenantId).IsRequired();
                builder.HasIndex(u => u.Name);
            }
        }

        /// <summary>
        /// 租户连接字符串
        /// </summary>
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


        /// <summary>
        /// 权限组
        /// </summary>
        public class PermissionGroupDefinitionEntityTypeConfiguration : IEntityTypeConfiguration<PermissionGroupDefinition>
        {
            public void Configure(EntityTypeBuilder<PermissionGroupDefinition> builder)
            {
                builder.ToTable("PermissionGroupDefinition");
                builder.HasKey(e => e.Id);
                builder.Property(x => x.Id).UseIdentityColumn(1, 1);
                builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
                builder.Property(e => e.DisplayName).IsRequired().HasMaxLength(128);
                builder.Property(e => e.CreateTime).HasDefaultValueSql("GETDATE()");
                builder.Property(e => e.Url).HasMaxLength(128);
                builder.HasMany(e => e.Permissions)
                    .WithOne(x => x.Group)
                    .IsRequired(false)
                    .HasForeignKey(x => x.GroupId);
                builder.HasIndex(u => u.Name);
            }
        }

        /// <summary>
        /// 权限
        /// </summary>
        public class PermissionDefinitionEntityTypeConfiguration : IEntityTypeConfiguration<PermissionDefinition>
        {
            public void Configure(EntityTypeBuilder<PermissionDefinition> builder)
            {
                builder.ToTable("PermissionDefinition");
                builder.HasKey(e => e.Id);
                builder.Property(x => x.Id).UseIdentityColumn(1, 1);
                builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
                builder.Property(e => e.DisplayName).IsRequired().HasMaxLength(128);
                builder.Property(e => e.Url).HasMaxLength(128);
                builder.Property(e => e.IsEnabled).HasDefaultValue(1);

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var dicConvert = new ValueConverter<List<string>, string>(v => JsonSerializer.Serialize(v, options), v => JsonSerializer.Deserialize<List<string>>(v, options));
                builder.Property(e => e.AllowedProviders).HasConversion(dicConvert);
                builder.HasIndex(u => u.Name);
            }
        }
    }

}
