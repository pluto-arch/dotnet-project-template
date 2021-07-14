namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public interface ICurrentTenantAccessor
    {
        TenantInfo CurrentTenantInfo { get; set; }
    }
}
