namespace PlutoNetCoreTemplate.Infrastructure.Repositories.Dapper
{
    using PlutoData;
    using PlutoData.Interface;
    using PlutoNetCoreTemplate.Domain.DomainModels.Account;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
