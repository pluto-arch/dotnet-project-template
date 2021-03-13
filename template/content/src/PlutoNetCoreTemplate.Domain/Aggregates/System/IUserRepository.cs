using System;
using PlutoData.Interface;

namespace PlutoNetCoreTemplate.Domain.Aggregates.System
{
    public interface IUserRepository: IEfRepository<UserEntity>
    {
        
    }
}