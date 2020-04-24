using System;
using PlutoData.Interface;


namespace Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository: IRepository<UserEntity>
    {
        
    }
}