namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using Constants;

    using Domain.Aggregates.TenantAggregate;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    using System.Linq;
    using System.Threading.Tasks;


    /// <summary>
    /// 配置文件中单独配置节的多租户数据提供者
    /// Tenants 配置节
    /// </summary>
    public class AppSettingTenantConnectionStringProvider : DefaultConnectionStringProvider
    {
        private readonly ICurrentTenant _currentTenant;

        private readonly TenantConfigurationOptions _tenantStoreOptions;

        public AppSettingTenantConnectionStringProvider(IOptions<TenantConfigurationOptions> options, ICurrentTenant currentTenant, IConfiguration configuration) : base(configuration)
        {
            _currentTenant = currentTenant;
            _tenantStoreOptions = options.Value;
        }

        public override Task<string> GetAsync(string connectionStringName = null)
        {
            connectionStringName ??= DbConstants.DefaultConnectionStringName;

            if (_currentTenant.IsAvailable)
            {
                var tenantConfig = _tenantStoreOptions.Tenants?.SingleOrDefault(t => t.TenantId == _currentTenant.Id);

                string connectionString = tenantConfig?.ConnectionStrings?[connectionStringName];

                if (connectionString is not null)
                {
                    return Task.FromResult(connectionString);
                }
            }

            return base.GetAsync(connectionStringName);
        }
    }
}