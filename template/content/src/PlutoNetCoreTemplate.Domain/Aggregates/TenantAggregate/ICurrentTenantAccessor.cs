namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public interface ICurrentTenantAccessor
    {
        TenantInfo CurrentTenantInfo { get; set; }

        event Func<IServiceScope,Task> OnScopeBind;
    }
}
