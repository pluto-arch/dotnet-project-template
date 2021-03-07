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
        private readonly AsyncLocal<string> _currentScope = new AsyncLocal<string>();

        public string TenantId { get => _currentScope.Value; set => _currentScope.Value = value; }
    }
}
