using System;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using Microsoft.Extensions.DependencyInjection;

    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        string Name { get; }

        string Id { get; }

        /// <summary>
        /// 切换租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IDisposable Change(string id, string name = null);

        /// <summary>
        /// 切换租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="scope">新增租户范围</param>
        /// <returns></returns>
        IDisposable Change(string id, string name, out IServiceScope scope);
    }
}
