using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.SeedWork;

using System;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Services.TenantDomainService
{
    using Microsoft.EntityFrameworkCore;
    public class TenantDataSeedProvider : IDataSeedProvider
    {
        private readonly ISystemBaseRepository<Tenant> _tenants;

        public TenantDataSeedProvider(ISystemBaseRepository<Tenant> tenants)
        {
            _tenants = tenants;
        }

        public int Sorts => 10000000;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _tenants.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            var t1 = new Tenant { Id = "T20210602000001", Name = "租户一", };
            t1.AddConnectionStrings("Default", "Server=127.0.0.1,1433;Database=Pnct_T20210602000001;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            var t2 = new Tenant { Id = "T20210602000002", Name = "租户二", };
            t2.AddConnectionStrings("Default", "Server=127.0.0.1,1433;Database=Pnct_T20210602000002;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            await _tenants.InsertAsync(t1, true);
            await _tenants.InsertAsync(t2, true);
        }

    }
}
