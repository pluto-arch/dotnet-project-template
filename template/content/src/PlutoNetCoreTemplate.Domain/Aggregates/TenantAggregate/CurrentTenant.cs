using System;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using Microsoft.Extensions.DependencyInjection;

    using PlutoNetCoreTemplate.Domain.SeedWork;

    using System.Collections.Generic;

    public class CurrentTenant : ICurrentTenant
    {
        private readonly ICurrentTenantAccessor _currentTenantAccessor;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor,IServiceScopeFactory serviceScopeFactory)
        {
            _currentTenantAccessor = currentTenantAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc />
        public bool IsAvailable => !string.IsNullOrEmpty(Id) && !string.IsNullOrWhiteSpace(Id);

        /// <inheritdoc />
        public string Name => _currentTenantAccessor.CurrentTenantInfo?.Name;

        public string Id => _currentTenantAccessor.CurrentTenantInfo?.Id;

        public Dictionary<string, string> ConnectionStrings =>
            _currentTenantAccessor.CurrentTenantInfo.ConnectionStrings;


        /// <summary>
        /// 准备租户
        /// </summary>
        /// <returns></returns>
        public IDisposable Reserve(TenantInfo tenant)
        {
            var parentScope = _currentTenantAccessor.CurrentTenantInfo;
            tenant.BindServiceScope(null);
            _currentTenantAccessor.CurrentTenantInfo = tenant;
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.CurrentTenantInfo = parentScope;
            });
        }



        /// <summary>
        /// 切换租户
        /// </summary>
        /// <returns></returns>
        public IDisposable Change(TenantInfo tenant)
        {
            var parentScope = _currentTenantAccessor.CurrentTenantInfo;
            var currentScoped = _serviceScopeFactory.CreateScope();
            tenant.BindServiceScope(currentScoped);
            _currentTenantAccessor.CurrentTenantInfo = tenant;
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.CurrentTenantInfo = parentScope;
                currentScoped?.Dispose();
            });
        }
    }


    /// <summary>
    /// 无租户，efcore design factory 初始化使用
    /// </summary>
    public class NullCurrentTenant : ICurrentTenant
    {
        public bool IsAvailable => default;

        public string Name => default;

        public string Id => default;

        public Dictionary<string, string> ConnectionStrings => default;

        public IDisposable Reserve(TenantInfo tenant)
        {
            return new DisposeAction(() => { });
        }

        public IDisposable Change(TenantInfo tenant)
        {
            return new DisposeAction(() => { });
        }


        public static ICurrentTenant DefaultCurrentTenant => new NullCurrentTenant();
    }
}
