using System;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{

    using System.Collections.Generic;

    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        string Name { get; }

        string Id { get; }

        public Dictionary<string, string> ConnectionStrings { get; }



        /// <summary>
        /// 准备租户环境
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        IDisposable Reserve(TenantInfo tenant);

        /// <summary>
        /// 切换租户环境
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        IDisposable Change(TenantInfo tenant);
    }
}
