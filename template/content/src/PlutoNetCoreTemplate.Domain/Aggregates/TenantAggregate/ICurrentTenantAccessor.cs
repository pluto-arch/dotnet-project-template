namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public interface ICurrentTenantAccessor
    {
        TenantInfo CurrentTenantInfo { get; set; }

        event Func<IServiceScope, Task> OnScopeBind;
    }
}
