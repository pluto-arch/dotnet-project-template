namespace PlutoNetCoreTemplate.Domain.Entities
{
    /// <summary>
    /// 多租户
    /// </summary>
    public interface IMultiTenant
    {
        string TenantId { get; set; }
    }
}
