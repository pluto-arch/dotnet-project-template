using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;

namespace Pluto.netcoreTemplate.Infrastructure.EntityConfigurations
{
    public class UserBookItemTypeConfig : IEntityTypeConfiguration<UserBook>
    {
        public void Configure(EntityTypeBuilder<UserBook> builder)
        {
            builder.ToTable("UserBooks", PlutonetcoreTemplateDbContext.DEFAULT_SCHEMA);

            builder.HasKey(b => b.Id).HasAnnotation("SqlServer:Identity","1,1");
            builder.Property(b => b.Id)
                .UseHiLo("userbookseq", PlutonetcoreTemplateDbContext.DEFAULT_SCHEMA);
            builder.Ignore(b => b.DomainEvents);


            builder.HasOne(x => x.User)
                .WithMany(x => x.UserBookItems)
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);


            builder
                .Ignore(b => b.BookName)
                .Property<string>("_bookName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(250)
                .HasColumnName(nameof(UserBook.BookName));

            builder
                .Ignore(b => b.Price)
                .Property<decimal>("_price")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName(nameof(UserBook.Price));

        }
    }
}