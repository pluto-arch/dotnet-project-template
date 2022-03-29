namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Repositories;

    using Microsoft.EntityFrameworkCore;

    using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

    using System.Linq;
    using System.Threading.Tasks;

    public class EFCoreTenantProvider : ITenantProvider
    {
        private readonly IRepository<Tenant> _tenants;

        public EFCoreTenantProvider(IRepository<Tenant> tenants)
        {
            _tenants = tenants;
        }

        public async Task<TenantInfo> InitTenant(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            var t = await _tenants.AsNoTracking().Include(x => x.ConnectionStrings).FirstOrDefaultAsync(x => x.Id == tenantId);
            if (t == null)
            {
                return null;
            }

            var tenant = new TenantInfo(tenantId, t.Name) { ConnectionStrings = t.ConnectionStrings.ToDictionary(k => k.Name, v => v.Value) };
            return tenant;
        }
    }
}
