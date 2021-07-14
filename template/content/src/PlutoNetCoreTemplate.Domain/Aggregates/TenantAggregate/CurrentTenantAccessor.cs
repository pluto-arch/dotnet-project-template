using System.Threading;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class CurrentTenantAccessor : ICurrentTenantAccessor
    {
        private readonly AsyncLocal<TenantInfo> _currentScope = new();

        public TenantInfo CurrentTenantInfo { get => _currentScope.Value; set => _currentScope.Value = value; }
    }
}
