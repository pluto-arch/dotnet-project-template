using System;
using Microsoft.EntityFrameworkCore;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;
using PlutoData;


namespace Pluto.netcoreTemplate.Infrastructure.Repositories
{
    public class UserRepository:Repository<UserEntity>, IUserRepository
    {
    }
}