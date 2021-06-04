using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using Microsoft.Extensions.DependencyInjection;

    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        string Name { get; }

        string Id { get; }

        IDisposable Change(string id,string name=null);

        IDisposable Change(string id,string name,out IServiceScope scope);
    }
}
