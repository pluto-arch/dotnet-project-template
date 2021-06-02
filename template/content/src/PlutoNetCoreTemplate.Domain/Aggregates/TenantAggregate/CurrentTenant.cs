using System;
using System.Diagnostics.CodeAnalysis;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    public class CurrentTenant : ICurrentTenant
    {
        private readonly ICurrentTenantAccessor _currentTenantAccessor;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor) => _currentTenantAccessor = currentTenantAccessor;

        /// <inheritdoc />
        public bool IsAvailable => !string.IsNullOrEmpty(Id)&&!string.IsNullOrWhiteSpace(Id);

        /// <inheritdoc />
        public string Name => _currentTenantAccessor.CurrentTenantInfo?.Name;

        public string Id => _currentTenantAccessor.CurrentTenantInfo?.Id;

        public IDisposable Change(string id,string name=null)
        {
            var parentScope = _currentTenantAccessor.CurrentTenantInfo;
            _currentTenantAccessor.CurrentTenantInfo = new TenantInfo(id,name);
            return new DisposeAction(() => _currentTenantAccessor.CurrentTenantInfo = parentScope);
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
