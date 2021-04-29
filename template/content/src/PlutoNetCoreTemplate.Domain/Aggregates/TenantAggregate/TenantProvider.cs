
namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TenantProvider:ITenantProvider
    {
        private readonly ICurrentTenant _currentTenant;

        public TenantProvider(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }


        public string GetTenantId() => _currentTenant?.Id;

    }
}