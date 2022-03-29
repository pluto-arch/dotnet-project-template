namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using Microsoft.Extensions.Configuration;

    using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
    using PlutoNetCoreTemplate.Infrastructure.Constants;

    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    /// <summary>
    /// 获取租户连接字符串
    /// </summary>
    public class TenantConnectionStringProvider : DefaultConnectionStringProvider
    {
        private readonly ICurrentTenant _currentTenant;
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> TenantConnStringCache = new();
        public TenantConnectionStringProvider(ICurrentTenant currentTenant, IConfiguration configuration) : base(configuration)
        {
            _currentTenant = currentTenant;
        }

        public override async Task<string> GetAsync(string connectionStringName = null)
        {
            connectionStringName ??= DbConstants.DefaultConnectionStringName;
            if (_currentTenant.IsAvailable)
            {
                if (TenantConnStringCache.TryGetValue(_currentTenant.Id, out var conns))
                {
                    return conns.FirstOrDefault(x => x.Key == connectionStringName).Value;
                }

                if (_currentTenant.ConnectionStrings is null or { Count: <= 0 })
                {
                    return await Default(connectionStringName);
                }

                if (!_currentTenant.ConnectionStrings.ContainsKey(connectionStringName))
                {
                    return await Default(connectionStringName);
                }

                string connectionString = _currentTenant.ConnectionStrings[connectionStringName];
                if (connectionString is not null)
                {
                    Cache(connectionStringName, connectionString);
                    return connectionString;
                }
                return await Default(connectionStringName);
            }
            return await base.GetAsync(connectionStringName);
        }


        private async Task<string> Default(string connectionStringName)
        {
            var str = await base.GetAsync(connectionStringName);
            Cache(connectionStringName, str);
            return str;
        }

        private void Cache(string connectionStringName, string s)
        {
            TenantConnStringCache.AddOrUpdate(
                                        _currentTenant.Id,
                                        new Dictionary<string, string> { { connectionStringName, s } },
                                        (key, connects) =>
                                        {
                                            if (connects == null || !connects.Any())
                                            {
                                                connects = new Dictionary<string, string> { { connectionStringName, s } };
                                            }
                                            if (!connects.ContainsKey(connectionStringName))
                                            {
                                                connects.Add(connectionStringName, s);
                                            }
                                            return connects;
                                        });
        }
    }

    internal class ConnectionStringCacheModel
    {

        public ConnectionStringCacheModel(string connectionStringName, string s)
        {
            ConnName = connectionStringName;
            ConnStr = s;
        }

        public string ConnName { get; set; }

        public string ConnStr { get; set; }
    }
}
