namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System.Threading.Tasks;

    public interface ITenantProvider
    {
        /// <summary>
        /// 初始化租户
        /// </summary>
        /// <returns></returns>
        Task<TenantInfo> InitTenant(string tenantId);
    }
}