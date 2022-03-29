namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    /// <summary>
    /// 配置文件中的租户配置
    /// </summary>
    public class TenantConfigurationOptions
    {
        public TenantConfiguration[] Tenants { get; set; }
    }
}
