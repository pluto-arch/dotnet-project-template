namespace PlutoNetCoreTemplate.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.Permission;
    using PlutoData;

    public class PermissionGrantRepository :EfRepository<EfCoreDbContext, PermissionGrant>, IPermissionGrantRepository
    {
        /// <inheritdoc />
        public PermissionGrantRepository(EfCoreDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            return null;
        }

        /// <inheritdoc />
        public Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            return null;
        }

        /// <inheritdoc />
        public Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey,
            CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}