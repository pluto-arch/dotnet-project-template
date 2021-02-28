namespace PlutoNetCoreTemplate.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using PlutoNetCoreTemplate.Domain.Services.Account;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<IAccountDomainService, AccountDomainService>();
            return services;
        }
    }
}
