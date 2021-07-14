
using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public sealed class TenantConnectionString : BaseEntity
    {
        public TenantConnectionString() { }


        public TenantConnectionString(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string TenantId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { TenantId, Name };
        }
    }
}
