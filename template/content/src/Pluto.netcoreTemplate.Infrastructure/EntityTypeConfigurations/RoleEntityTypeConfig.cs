using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pluto.netcoreTemplate.Domain.DomainModels.Account;


namespace Pluto.netcoreTemplate.Infrastructure.EntityTypeConfigurations
{
    public class RoleEntityTypeConfig : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RoleName).IsRequired(true).HasMaxLength(300);
            builder.Ignore(x => x.Users);
        }
    }
}