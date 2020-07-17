using System;
using Pluto.netcoreTemplate.Domain.DomainModels.Account;
using PlutoData.Interface;


namespace Pluto.netcoreTemplate.Domain.IRepositories
{
    public interface IUserRepository: IRepository<UserEntity>
    {
        
    }
}