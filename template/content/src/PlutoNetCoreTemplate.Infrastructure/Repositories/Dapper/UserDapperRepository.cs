using PlutoData;
using PlutoData.Interface;

using PlutoNetCoreTemplate.Domain.Aggregates.Account;

namespace PlutoNetCoreTemplate.Infrastructure.Repositories.Dapper
{
    public interface IUserDapperRepository:IDapperRepository<UserEntity>
    {
    }


    public class UserDapperRepository : DapperRepository<UserEntity>, IUserDapperRepository
    {
        public UserDapperRepository(DapperDbContext dapperDb) : base(dapperDb)
        {
        }
    }
}
