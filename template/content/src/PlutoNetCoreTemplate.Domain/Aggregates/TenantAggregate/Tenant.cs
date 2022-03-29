using PlutoNetCoreTemplate.Domain.Entities;

using System;
using System.Collections.Generic;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class Tenant : BaseAggregateRoot<string>
    {
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public List<TenantConnectionString> ConnectionStrings { get; protected set; } = new List<TenantConnectionString>();


        public void AddConnectionStrings(string connectionName, string value)
        {
            ConnectionStrings.Add(new TenantConnectionString(connectionName, value));
        }
    }
}
