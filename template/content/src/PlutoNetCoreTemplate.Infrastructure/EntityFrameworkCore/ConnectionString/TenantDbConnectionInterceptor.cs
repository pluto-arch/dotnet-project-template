namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using Microsoft.EntityFrameworkCore.Diagnostics;

    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// DbConnection 侦听器
    /// </summary>
    public class TenantDbConnectionInterceptor : DbConnectionInterceptor
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly string _connectionStringName;

        public TenantDbConnectionInterceptor(IConnectionStringProvider connectionStringProvider, string connName)
        {
            _connectionStringProvider = connectionStringProvider;
            _connectionStringName = connName;
        }

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            connection.ConnectionString = _connectionStringProvider.GetAsync(_connectionStringName).Result;
            return base.ConnectionOpening(connection, eventData, result);
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
        {
            connection.ConnectionString = await _connectionStringProvider.GetAsync(_connectionStringName);
            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }
    }
}