namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using Constants;

    using Microsoft.Extensions.Configuration;

    using System.Threading.Tasks;


    /// <summary>
    /// 默认的连接字符串提供程序
    /// 读取配置文件
    /// </summary>
    public class DefaultConnectionStringProvider : IConnectionStringProvider
    {
        protected readonly IConfiguration _configuration;

        public DefaultConnectionStringProvider(IConfiguration configuration) => _configuration = configuration;

        public virtual Task<string> GetAsync(string connectionStringName = null)
        {
            connectionStringName ??= DbConstants.DefaultConnectionStringName;
            return Task.FromResult(_configuration.GetConnectionString(connectionStringName));
        }
    }
}