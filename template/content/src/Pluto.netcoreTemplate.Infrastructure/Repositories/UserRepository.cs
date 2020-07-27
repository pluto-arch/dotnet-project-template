using Pluto.netcoreTemplate.Domain.DomainModels.Account;
using Pluto.netcoreTemplate.Domain.IRepositories;
using PlutoData;


namespace Pluto.netcoreTemplate.Infrastructure.Repositories
{
    public class UserRepository:Repository<UserEntity>, IUserRepository
    {
    }
}