
using PlutoData;
using PlutoNetCoreTemplate.Domain.Aggregates.Account;
using PlutoNetCoreTemplate.Domain.IRepositories;

namespace PlutoNetCoreTemplate.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<EfCoreDbContext, UserEntity>, IUserRepository
    {
        public UserRepository(EfCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}