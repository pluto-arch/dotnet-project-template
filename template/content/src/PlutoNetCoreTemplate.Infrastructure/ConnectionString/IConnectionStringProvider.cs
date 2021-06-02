namespace PlutoNetCoreTemplate.Infrastructure.ConnectionString
{
    using System.Threading.Tasks;

    public interface IConnectionStringProvider
    {
        Task<string> GetAsync(string connectionStringName = null);
    }
}