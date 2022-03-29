namespace PlutoNetCoreTemplate.Domain.Aggregates.PermissionGrant
{
    using Repositories;

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPermissionGrantRepository : IRepository<PermissionGrant>
    {
        Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey, CancellationToken cancellationToken = default);

        Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default);

        Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey, CancellationToken cancellationToken = default);
    }
}