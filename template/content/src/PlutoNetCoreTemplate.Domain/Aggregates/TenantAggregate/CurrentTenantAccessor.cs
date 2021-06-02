using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class CurrentTenantAccessor: ICurrentTenantAccessor
    {
        private readonly AsyncLocal<TenantInfo> _currentScope = new();

        public TenantInfo CurrentTenantInfo { get => _currentScope.Value; set => _currentScope.Value = value; }
    }
}
