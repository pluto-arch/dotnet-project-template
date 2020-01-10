using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;

namespace Pluto.netcoreTemplate.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", PlutonetcoreTemplateDbContext.DEFAULT_SCHEMA);

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .UseHiLo("userseq", PlutonetcoreTemplateDbContext.DEFAULT_SCHEMA);
            builder.Ignore(b => b.DomainEvents);
            builder
                .Property<string>("_userName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(250)
                .HasColumnName(nameof(User.UserName));

            builder
                .Property<string>("_tel")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(20)
                .HasColumnName(nameof(User.Tel));


            var navigation = builder.Metadata.FindNavigation(nameof(User.UserBookItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);


            builder.Property(b => b.CreateTime).HasDefaultValue(DateTime.Now).ValueGeneratedOnAdd(); //在添加时生成值

            builder.Property(b => b.EditTime).HasDefaultValue(DateTime.Now).ValueGeneratedOnAddOrUpdate(); // 再添加和更新时生成值
        }
    }
}