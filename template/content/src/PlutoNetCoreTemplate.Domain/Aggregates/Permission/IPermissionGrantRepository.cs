namespace PlutoNetCoreTemplate.Domain.Aggregates.Permission
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using SeedWork;

    public interface IPermissionGrantRepository:IPlutoNetCoreTemplateEfRepository<PermissionGrant>
    {
        Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey, CancellationToken cancellationToken = default);

        Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default);

        Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey, CancellationToken cancellationToken = default);
    }
}