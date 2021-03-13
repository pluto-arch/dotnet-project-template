
using PlutoData;

namespace PlutoNetCoreTemplate.Infrastructure.Repositories
{
    using Domain.Aggregates.System;

    public class UserRepository : EfRepository<EfCoreDbContext, UserEntity>, IUserRepository
    {
        public UserRepository(EfCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}