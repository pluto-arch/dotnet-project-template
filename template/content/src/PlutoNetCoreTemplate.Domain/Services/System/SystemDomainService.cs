
using PlutoData.Collections;
using PlutoData.Interface;
using PlutoData.Specifications;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    using Aggregates.System;

    public class SystemDomainService : ISystemDomainService
    {
        private readonly IEfRepository<UserEntity> _userRepository;
        /* can use dapper */
        //private readonly IDapperRepository<UserEntity> _userDapperRep;

        public SystemDomainService(
            IEfRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public UserEntity Find(object key)
        {
            return _userRepository.Find(key);
        }

        ///<inheritdoc />
        public IPagedList<UserEntity> GetUserPageList(ISpecification<UserEntity> spec, int pageNo, int pageSize)
        {
            return _userRepository.GetPageList(spec, pageNo, pageSize);
        }

    }
}
