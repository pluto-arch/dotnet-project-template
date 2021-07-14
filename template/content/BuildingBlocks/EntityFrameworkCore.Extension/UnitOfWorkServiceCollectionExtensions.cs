using EntityFrameworkCore.Extension.UnitOfWork.Uows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;


namespace EntityFrameworkCore.Extension.UnitOfWork
{
    /// <summary>
    /// 
    /// </summary>
    public static class UnitOfWorkServiceCollectionExtensions
    {

        /// <summary>
        /// 添加仓储
        /// </summary>
        public static void AddRepository(this IServiceCollection services, Assembly assembly = null, bool repositoryScoped = false)
        {
            assembly ??= Assembly.GetEntryAssembly();
            var implTypes = assembly?.GetTypes().Where(c => !c.IsInterface && c.Name.EndsWith("Repository")).ToList();
            if (implTypes == null)
            {
                return;
            }

            foreach (var impltype in implTypes)
            {
                var interfaces = impltype.GetInterfaces()
                                         .Where(c => c.Name.StartsWith("I") && c.Name.EndsWith("Repository"));
                if (!interfaces.Any())
                    continue;
                foreach (var inter in interfaces)
                {
                    if (repositoryScoped)
                    {
                        services.AddScoped(inter, impltype);
                    }
                    else
                    {
                        services.AddTransient(inter, impltype);
                    }
                }
            }
        }

        #region private

        /// <summary>
        /// 添加unitofwork
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEfUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            return services;
        }

        #endregion
    }
}