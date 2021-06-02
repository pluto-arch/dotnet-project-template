namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class TenantInfo
    {
        public TenantInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}