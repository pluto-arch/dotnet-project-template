using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public interface ICurrentTenantAccessor
    {
        TenantInfo CurrentTenantInfo { get; set; }
    }
}
