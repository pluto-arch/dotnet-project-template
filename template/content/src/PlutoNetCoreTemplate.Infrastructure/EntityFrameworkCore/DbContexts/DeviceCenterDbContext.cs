using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Aggregates.PermissionGrant;
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.ProjectAggregate;
    using Domain.UnitOfWork;

    using System.Reflection;

    public class DeviceCenterDbContext : PlutoDbContext<DeviceCenterDbContext>, IUowDbContext
    {

        public DeviceCenterDbContext(DbContextOptions<DeviceCenterDbContext> options)
            : base(options)
        {
        }

        #region Entitys and configuration

        public DbSet<PermissionGrant> PermissionGrants { get; set; }

        public DbSet<Device> Device { get; set; }

        public DbSet<DeviceTag> DeviceTag { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<ProjectGroup> ProjectGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        #endregion
    }


    ///// <summary>
    ///// 动态获取efcore的IModel  会造成性能下降
    ///// 这里返回租户提供程序，从而每次都走 OnModelCreating  表名就可以按租户进行分表了
    ///// TabRoute 是dbcontext公开的分表路由，这个可以自定义
    ///// </summary>
    //public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    //{
    //    public object Create(DbContext context)
    //    {
    //        return context is DeviceCenterDbContext shardingContext
    //            ? (context.GetType(), TabRoute: shardingContext.TabRoute)
    //            : (object)context.GetType();
    //    }
    //}

}
