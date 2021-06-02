namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using System.Linq;
    using System.Threading.Tasks;
    using Constants;
    using Domain.Aggregates.TenantAggregate;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class TenantConnectionStringProvider: DefaultConnectionStringProvider
    {
        private readonly ICurrentTenant _currentTenant;

        private readonly TenantStoreOptions _tenantStoreOptions;

        public TenantConnectionStringProvider(IOptions<TenantStoreOptions> options, ICurrentTenant currentTenant, IConfiguration configuration) : base(configuration)
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