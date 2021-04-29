namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITenantProvider
    {
        string GetTenantId();
    }
}