namespace PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate
{
    using Entities;

    public class DeviceTag : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}