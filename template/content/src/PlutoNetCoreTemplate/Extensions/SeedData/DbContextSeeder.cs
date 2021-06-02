namespace PlutoNetCoreTemplate.Api.Extensions.SeedData
{
    using System;
    using System.Linq;
    using EntityFrameworkCore.Extension.Uows;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PlutoNetCoreTemplate.Domain.SeedWork;
    using PlutoNetCoreTemplate.Infrastructure;

    public static class DbContextSeeder
    {
        public static void MigrateDbContext<TContext>(this IHost webHost)
           where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = webHost.Services.GetService<IHostEnvironment>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                if (env.IsDevelopment())
                {
                    var context = services.GetService<TContext>();
                    logger.LogWarning("环境:{@env},执行{@context}的迁移", env.EnvironmentName, typeof(TContext).Name);
                    try
                    {
                        logger.LogInformation("开始迁移数据库 {DbContextName}", typeof(TContext).Name);
                        // 存在未提交到数据库的迁移
                        if (context.Database.GetPendingMigrations().Any())
                        {
                            // 进行迁移
                            context.Database.Migrate();
                            logger.LogInformation("已迁移数据库 {DbContextName}", typeof(TContext).Name);
                        }
                        else
                        {
                            logger.LogInformation("不需要迁移 {DbContextName}", typeof(TContext).Name);
                        }
                        var dataSeedProviders = services.GetServices<IDataSeedProvider>();
                        logger.LogWarning("执行种子数据：{@seeds}", dataSeedProviders.Select(x=>x.GetType().Name));
                        foreach (IDataSeedProvider dataSeedProvider in dataSeedProviders)
                        {
                            dataSeedProvider.SeedAsync(services).Wait();
                        }
                        var uow = services.GetService<IUnitOfWork<PlutoNetTemplateDbContext>>();
                        uow?.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "迁移数据库时出错 {DbContextName}", typeof(TContext).Name);
                    }
                }
            }
        }
    }
}
