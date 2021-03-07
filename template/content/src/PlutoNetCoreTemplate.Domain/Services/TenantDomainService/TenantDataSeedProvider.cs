using System;
using System.Threading.Tasks;

using PlutoData.Interface;
using PlutoData.Uows;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.SeedWork;

namespace PlutoNetCoreTemplate.Domain.Services.TenantDomainService
{
    public class TenantDataSeedProvider : IDataSeedProvider
    {
        private readonly IEfRepository<Tenant> _rep;

        public TenantDataSeedProvider(IEfRepository<Tenant> userRepository)
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
