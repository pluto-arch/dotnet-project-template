using System;
using System.Collections.Generic;
using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class Tenant:BaseEntity<string>
    {
        public string Name { get; set; } = null!;

        public List<TenantConnectionString> ConnectionStrings { get; protected set; } = new List<TenantConnectionString>();
    }
}
