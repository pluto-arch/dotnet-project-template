namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using System.Threading.Tasks;

    public interface IConnectionStringProvider
    {
        /// <summary>
        /// 获取对应名称的连接字符串
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        Task<string> GetAsync(string connectionStringName = null);
    }
}