namespace PlutoNetCoreTemplate.Infrastructure.EntityTypeConfigurations
{
    using Domain.Aggregates.PermissionGrant;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<PermissionGrant>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<PermissionGrant> builder)
        {
            builder.ToTable("PermissionGrant");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(300).IsRequired(true);
            builder.Property(x => x.ProviderName).HasMaxLength(300).IsRequired(true);
            builder.Property(x => x.ProviderKey).HasMaxLength(300).IsRequired(true);
        }
    }
}