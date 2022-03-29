namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Repositories
{
    using Domain.Aggregates.PermissionGrant;
    using Domain.UnitOfWork;

    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class PermissionGrantRepository : EFCoreRepository<DeviceCenterDbContext, PermissionGrant, int>, IPermissionGrantRepository
    {
        public PermissionGrantRepository(IUnitOfWork<DeviceCenterDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            return await DbSet.OrderBy(x => x.Id).FirstOrDefaultAsync(s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey, cancellationToken);
        }

        public async Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey).ToListAsync(cancellationToken);
        }

        public async Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey).ToListAsync(cancellationToken);
        }
    }
}