namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public class TenantInfo
    {
        public TenantInfo(string id, string name, Dictionary<string, string> connStr = null)
        {
            Id = id;
            Name = name;
            ConnectionStrings = connStr;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> ConnectionStrings { get; set; }

        public IServiceScope ServiceScope { get; private set; }

        public void BindServiceScope(IServiceScope scoped)
        {
            ServiceScope = scoped;
        }
    }
}