using System;
using Microsoft.EntityFrameworkCore;
using Demo.Domain.AggregatesModel.UserAggregate;
using PlutoData;


namespace Demo.Infrastructure.Repositories
{
    public class UserRepository:Repository<UserEntity>, IUserRepository
    {
    }
}