using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public interface ICurrentTenant
    {
        string Id { get; }

        IDisposable Change(string id);
    }
}
