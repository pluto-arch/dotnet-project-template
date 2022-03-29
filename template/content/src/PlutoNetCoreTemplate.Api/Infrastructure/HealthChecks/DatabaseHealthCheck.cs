﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using PlutoNetCoreTemplate.Infrastructure.Constants;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Api.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public DatabaseHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(DbConstants.DefaultConnectionStringName) ?? throw new ArgumentNullException("连接字符串为空");
        }

        /// <inheritdoc />
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "select 1+1";
                        await command.ExecuteScalarAsync(cancellationToken);
                    }

                    return new HealthCheckResult(
                        HealthStatus.Healthy,
                        description: $"sql server 健康状态",
                        exception: null,
                        data: null);
                }
            }
            catch (Exception ex)
            {
                // todo send notification to DevOps
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}