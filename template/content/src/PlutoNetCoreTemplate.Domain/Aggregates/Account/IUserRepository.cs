using System;
using PlutoData.Interface;
using PlutoNetCoreTemplate.Domain.Aggregates.Account;

namespace PlutoNetCoreTemplate.Domain.IRepositories
{
    public interface IUserRepository: IEfRepository<UserEntity>
    {
        
    }
}