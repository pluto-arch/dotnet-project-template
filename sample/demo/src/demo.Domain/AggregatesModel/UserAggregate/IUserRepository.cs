using System;
using PlutoData.Interface;


namespace Demo.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository: IRepository<UserEntity>
    {
        
    }
}