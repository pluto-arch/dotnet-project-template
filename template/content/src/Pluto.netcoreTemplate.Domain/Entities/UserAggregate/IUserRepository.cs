using System.Linq;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.SeedWork;

namespace Pluto.netcoreTemplate.Domain.Entities.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        User Add(User user);

        void Update(User user);

        Task<User> GetAsync(int userId);
        Task<(int total, IQueryable<User> list)> GetListAsync(int page, int rows);
    }
}