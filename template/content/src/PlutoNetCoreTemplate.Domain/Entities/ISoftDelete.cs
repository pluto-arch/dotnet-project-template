namespace PlutoNetCoreTemplate.Domain.Entities
{
    /// <summary>
    /// 软删除
    /// </summary>
    public interface ISoftDelete
    {
        bool Deleted { get; set; }
    }
}
