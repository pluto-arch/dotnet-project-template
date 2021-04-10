using System;

namespace PlutoNetCoreTemplate.Domain.Aggregates.System
{
    using PlutoData;

    public interface IUserRepository: IEfRepository<UserEntity>
    {
        
    }
}