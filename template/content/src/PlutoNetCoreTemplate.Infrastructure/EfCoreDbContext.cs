using MediatR;

using Microsoft.EntityFrameworkCore;
using PlutoNetCoreTemplate.Infrastructure.EntityTypeConfigurations;
using PlutoNetCoreTemplate.Domain.DomainModels.Account;

namespace PlutoNetCoreTemplate.Infrastructure
{
    public class EfCoreDbContext : DbContext
    {

        public EfCoreDbContext(DbContextOptions<EfCoreDbContext> options)
            : base(options)
        {
        }


        #region Entitys and configuration  (OnModelCreating中配置了对应Entity 那么对应DbSet<>可以不写)
        public DbSet<UserEntity> Users { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfig());
        }
        #endregion

    }
}
