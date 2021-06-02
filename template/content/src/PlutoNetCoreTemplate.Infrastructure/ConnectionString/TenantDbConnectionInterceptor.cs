namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    /// <summary>
    /// DbConnection 侦听器
    /// </summary>
    public class TenantDbConnectionInterceptor: DbConnectionInterceptor
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public TenantDbConnectionInterceptor(IConnectionStringProvider connectionStringProvider) => _connectionStringProvider = connectionStringProvider;

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            connection.ConnectionString = _connectionStringProvider.GetAsync().Result;
            return base.ConnectionOpening(connection, eventData, result);
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
        {
            connection.ConnectionString = await _connectionStringProvider.GetAsync();
            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }
    }
}