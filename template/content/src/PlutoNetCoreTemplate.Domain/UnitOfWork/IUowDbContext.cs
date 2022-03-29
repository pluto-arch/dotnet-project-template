using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.UnitOfWork
{
    public interface IUowDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}