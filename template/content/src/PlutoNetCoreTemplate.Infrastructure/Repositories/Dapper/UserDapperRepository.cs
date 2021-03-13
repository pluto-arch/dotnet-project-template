using PlutoData;
using PlutoData.Interface;


namespace PlutoNetCoreTemplate.Infrastructure.Repositories.Dapper
{
    using Domain.Aggregates.System;

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
