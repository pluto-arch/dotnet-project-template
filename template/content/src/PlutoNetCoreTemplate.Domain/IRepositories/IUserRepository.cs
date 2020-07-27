using System;
using PlutoNetCoreTemplate.Domain.DomainModels.Account;
using PlutoData.Interface;


namespace PlutoNetCoreTemplate.Domain.IRepositories
{
    public interface IUserRepository: IRepository<UserEntity>
    {
        
    }
}