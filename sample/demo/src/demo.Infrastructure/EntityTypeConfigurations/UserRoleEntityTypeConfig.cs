using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Demo.Domain.AggregatesModel.UserAggregate;


namespace Demo.Infrastructure.EntityTypeConfigurations
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