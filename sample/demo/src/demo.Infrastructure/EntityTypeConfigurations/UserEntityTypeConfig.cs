using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Demo.Domain.AggregatesModel.UserAggregate;


namespace Demo.Infrastructure.EntityTypeConfigurations
{
    public class UserEntityTypeConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired(true).HasMaxLength(300);
            builder.Property(x => x.Email).IsRequired(true).HasMaxLength(200);
            builder.Ignore(x => x.Roles);
        }
    }
}