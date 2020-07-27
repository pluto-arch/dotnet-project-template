using PlutoNetCoreTemplate.Domain.DomainModels.Account;
using PlutoNetCoreTemplate.Domain.IRepositories;
using PlutoData;


namespace PlutoNetCoreTemplate.Infrastructure.Repositories
{
    public class UserRepository:Repository<UserEntity>, IUserRepository
    {
    }
}