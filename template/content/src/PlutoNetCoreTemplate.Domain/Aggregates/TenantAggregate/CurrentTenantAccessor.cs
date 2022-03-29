using System.Threading;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public class CurrentTenantAccessor : ICurrentTenantAccessor
    {
        private readonly AsyncLocal<TenantInfo> _currentScope = new();

        public event Func<IServiceScope, Task> OnScopeBind;

        public TenantInfo CurrentTenantInfo
        {
            get => _currentScope.Value;
            set
            {
                _currentScope.Value = value;
                OnScopeBind?.Invoke(value?.ServiceScope);
            }
        }
    }
}
