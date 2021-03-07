using System;

using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class TenantConnectionString: BaseEntity
    {
        public virtual string TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { TenantId, Name };
        }
    }
}
