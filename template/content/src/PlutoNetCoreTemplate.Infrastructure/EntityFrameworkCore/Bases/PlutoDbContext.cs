using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.Entities;
using PlutoNetCoreTemplate.Domain.UnitOfWork;
using PlutoNetCoreTemplate.Infrastructure.Extensions;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    /// <summary>
    /// 基础多租户dbcontext
    /// </summary>
    public class PlutoDbContext<TContext> : DbContext where TContext : DbContext, IUowDbContext
    {
        public PlutoDbContext(DbContextOptions<TContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                // 多租户
                if (item.ClrType.IsAssignableTo(typeof(IMultiTenant)))
                {
                    modelBuilder.Entity(item.ClrType).AddQueryFilter<IMultiTenant>(e => e.TenantId == this.GetService<ICurrentTenant>().Id);
                }

                // 软删除
                if (item.ClrType.IsAssignableTo(typeof(ISoftDelete)))
                {
                    modelBuilder.Entity(item.ClrType).AddQueryFilter<ISoftDelete>(e => !e.Deleted);
                }
            }
        }

    }
}