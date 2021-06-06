using System;
using System.Diagnostics.CodeAnalysis;

namespace PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate
{
    using Microsoft.Extensions.DependencyInjection;

    public class CurrentTenant : ICurrentTenant
    {
        private readonly ICurrentTenantAccessor _currentTenantAccessor;
        private readonly IServiceProvider _serviceProvider;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor,IServiceProvider serviceProvider)
        {
            _currentTenantAccessor = currentTenantAccessor;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool IsAvailable => !string.IsNullOrEmpty(Id)&&!string.IsNullOrWhiteSpace(Id);

        /// <inheritdoc />
        public string Name => _currentTenantAccessor.CurrentTenantInfo?.Name;

        public string Id => _currentTenantAccessor.CurrentTenantInfo?.Id;


        /// <summary>
        /// 切换租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDisposable Change(string id,string name="")
        {
            var parentScope = _currentTenantAccessor.CurrentTenantInfo;
            _currentTenantAccessor.CurrentTenantInfo = new TenantInfo(id,name);
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.CurrentTenantInfo = parentScope;
            });
        }


        /// <summary>
        /// 切换租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="scope">新增租户范围,不用手动释放</param>
        /// <returns></returns>
        public IDisposable Change(string id,string name,out IServiceScope scope)
        {
            var parentScope = _currentTenantAccessor.CurrentTenantInfo;
            _currentTenantAccessor.CurrentTenantInfo = new TenantInfo(id,name);
            scope = _serviceProvider.CreateScope();
            IServiceScope serviceScope = scope;
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.CurrentTenantInfo = parentScope;
                serviceScope?.Dispose();
            });
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
