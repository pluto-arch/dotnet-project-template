using PlutoData;


namespace PlutoNetCoreTemplate.Infrastructure.Repositories.Dapper
{
    using DapperCore;
    using Domain.Aggregates.System;

    public class UserDapperRepository : BaseDapperRepository<PlutoNetCoreDapperDbContext,UserEntity>, IUserDapperRepository
    {
        public UserDapperRepository(PlutoNetCoreDapperDbContext dapperDb) : base(dapperDb)
        {
        }
    }
}
