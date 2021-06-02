using System;
using System.Threading.Tasks;
using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.SeedWork;

namespace PlutoNetCoreTemplate.Domain.Services.TenantDomainService
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    public class TenantDataSeedProvider : IDataSeedProvider
    {
        private readonly IPlutoNetCoreTemplateBaseRepository<Tenant> _rep;

        public TenantDataSeedProvider(IPlutoNetCoreTemplateBaseRepository<Tenant> userRepository)
        {
            _rep = userRepository;
        }

        public int Sorts => 10000000;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _rep.IgnoreQueryFilters().LongCountAsync() > 0)
            {
                return;
            }

            var list = new List<Tenant>();
            for (int i = 0; i < 10; i++)
            {
                var t = new Tenant
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name=$"tenant_{i}",
                };
                list.Add(t);
            }
            await _rep.InsertAsync(list,true);
        }

    }
}
