namespace PlutoNetCoreTemplate.Api.Extensions.SeedData
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbSeedData
    {
        /// <summary>
        /// 种子数据初始化
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task<IApplicationBuilder> DataSeederAsync(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            var seeders = serviceScope.ServiceProvider.GetServices<IDataSeedProvider>();
            if (!seeders.Any())
            {
                return app;
            }

            foreach (var seeder in seeders.OrderByDescending(x => x.Sorts).ToList())
            {
                await seeder.SeedAsync(serviceScope.ServiceProvider);
            }

            return app;
        }
    }
}