namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public interface ITenantProvider
    {
        string GetTenantId();
    }
}