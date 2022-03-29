namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class TenantConfiguration
    {
        public string TenantId { get; set; }

        public string TenantName { get; set; } = string.Empty;

        public Dictionary<string, string> ConnectionStrings { get; set; }
    }
}
