using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PlutoNetCoreTemplate.Extensions
{
    public static class WebHostExtension
    {
        public static void MigrateDbContext<TContext>(this IWebHost webHost,
                                                      Action<TContext, IServiceProvider, IWebHostEnvironment> seeder)
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = webHost.Services.GetService<IWebHostEnvironment>();
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
                        seeder?.Invoke(context, webHost.Services, env); // 种子数据初始化
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