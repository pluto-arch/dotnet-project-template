
using PlutoData.Collections;
using PlutoData.Specifications;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    using System.Threading.Tasks;
    using Aggregates.System;
    using Dapper;
    using Microsoft.EntityFrameworkCore;
    using PlutoData;
    using SeedWork;

    public class SystemDomainService : ISystemDomainService
    {
        private readonly IPlutoNetCoreTemplateEfRepository<UserEntity> _userRepository;
        /* can use dapper */
        private readonly IPlutoNetCoreTemplateDapperRepository<UserEntity> _userDapperRep;

        public SystemDomainService(
            IPlutoNetCoreTemplateEfRepository<UserEntity> userRepository,
            IPlutoNetCoreTemplateDapperRepository<UserEntity> userDapperRep)
        {
            _userRepository = userRepository;
            _userDapperRep = userDapperRep;
        }

        public async Task<UserEntity> Find(object key)
        {
            return await _userRepository.FindAsync(x=>x.Id==(long)key);
        }

        ///<inheritdoc />
        public IPagedList<UserEntity> GetUserPageList(ISpecification<UserEntity> spec, int pageNo, int pageSize)
        {
            return _userRepository.GetPageList(spec, pageNo, pageSize);
        }

        public async Task<int?> InsertAsync(UserEntity user)
        {
            return await _userDapperRep.ExecuteAsync(async conn => await conn.InsertAsync(user));
        }

    }
}
