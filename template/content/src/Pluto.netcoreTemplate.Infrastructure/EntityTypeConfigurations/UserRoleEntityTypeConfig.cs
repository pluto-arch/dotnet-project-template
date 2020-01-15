using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pluto.netcoreTemplate.Domain.Entities.Account;


namespace Pluto.netcoreTemplate.Infrastructure.EntityTypeConfigurations
{
    public class UserRoleEntityTypeConfig : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(r => new
            {
                UserId = r.UserId,
                RoleId = r.RoleId
            });

            builder.HasOne(pt => pt.User)
                .WithMany(t => t.Roles)
                .HasForeignKey(pt => pt.UserId);

            builder.HasOne(pt => pt.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(pt => pt.RoleId);
        }
    }
}