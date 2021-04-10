using System;
using System.Threading.Tasks;
using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.SeedWork;

namespace PlutoNetCoreTemplate.Domain.Services.TenantDomainService
{
    using Microsoft.EntityFrameworkCore;
    using PlutoData;

    public class TenantDataSeedProvider : IDataSeedProvider
    {
        private readonly IPlutoNetCoreTemplateEfRepository<Tenant> _rep;

        public TenantDataSeedProvider(IPlutoNetCoreTemplateEfRepository<Tenant> userRepository)
        {
            _rep = userRepository;
        }


        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _rep.LongCountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var t = new Tenant
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name=$"tenant_{i}"
                    };
                    await _rep.InsertAsync(t);
                    //user.AddDomainEvent(new ProductCreatedDomainEvent(user));
                }
            }
        }

    }
}
