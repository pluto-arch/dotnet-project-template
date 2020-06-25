using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Demo.Domain.SeedWork;
using Demo.Infrastructure.EntityTypeConfigurations;
using Demo.Infrastructure.Extensions;

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.AggregatesModel.UserAggregate;

namespace Demo.Infrastructure
{
    public class DemoDbContext : DbContext
    {

        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }


        #region Entitys and configuration  (OnModelCreating中配置了对应Entity 那么对应DbSet<>可以不写)
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfig());
        }
        #endregion

    }
}
