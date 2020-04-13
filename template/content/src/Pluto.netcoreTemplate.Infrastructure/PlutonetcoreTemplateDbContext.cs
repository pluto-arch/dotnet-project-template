using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Pluto.netcoreTemplate.Domain.SeedWork;
using Pluto.netcoreTemplate.Infrastructure.EntityTypeConfigurations;
using Pluto.netcoreTemplate.Infrastructure.Extensions;

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;

namespace Pluto.netcoreTemplate.Infrastructure
{
    public class PlutonetcoreTemplateDbContext : DbContext
    {

        public const string DEFAULT_SCHEMA = "dbo";

        public PlutonetcoreTemplateDbContext(DbContextOptions<PlutonetcoreTemplateDbContext> options)
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
