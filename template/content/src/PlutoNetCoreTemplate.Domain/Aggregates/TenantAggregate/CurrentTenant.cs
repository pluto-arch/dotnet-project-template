using System;
using System.Diagnostics.CodeAnalysis;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class CurrentTenant : ICurrentTenant
    {
        private readonly ICurrentTenantAccessor _currentTenantAccessor;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor) => _currentTenantAccessor = currentTenantAccessor;

        public string Id => _currentTenantAccessor.TenantId;

        public IDisposable Change(string id)
        {
            var parentScope = _currentTenantAccessor.TenantId;
            _currentTenantAccessor.TenantId = id;
            return new DisposeAction(() => _currentTenantAccessor.TenantId = parentScope);
        }


        public class DisposeAction : IDisposable
        {
            private readonly Action _action;

            public DisposeAction([NotNull] Action action) => _action = action;

            void IDisposable.Dispose()
            {
                _action();
                GC.SuppressFinalize(this);
            }
        }
    }
}
