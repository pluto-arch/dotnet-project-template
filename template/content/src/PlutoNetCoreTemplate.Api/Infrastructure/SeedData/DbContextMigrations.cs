namespace PlutoNetCoreTemplate.Api.SeedData
{
    using Microsoft.EntityFrameworkCore;


    public static class DbContextMigrations
    {
        /// <summary>
        /// 迁移数据库
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="webHost"></param>
        public static void MigrateDbContext<TContext>(this IHost webHost)
           where TContext : DbContext
        {
            using var scope = webHost.Services.CreateScope();
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
                    if (context != null && context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                        var script = context.Database.GenerateCreateScript();
                        logger.LogInformation("已迁移数据库 {DbContextName}, 执行脚本：{script}", typeof(TContext).Name, script);
                    }
                    else
                    {
                        logger.LogInformation("不需要迁移 {DbContextName}", typeof(TContext).Name);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "迁移数据库时出错 {DbContextName}", typeof(TContext).Name);
                }
            }
        }
    }
}
