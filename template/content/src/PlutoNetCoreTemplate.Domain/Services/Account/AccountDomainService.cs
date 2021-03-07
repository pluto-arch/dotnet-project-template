
using PlutoData.Collections;
using PlutoData.Interface;
using PlutoData.Specifications;

using PlutoNetCoreTemplate.Domain.Aggregates.Account;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    public class AccountDomainService : IAccountDomainService
    {
        private readonly IEfRepository<UserEntity> _userRepository;
        private readonly IDapperRepository<UserEntity> _userDapperRep;

        public AccountDomainService(
            IEfRepository<UserEntity> userRepository,
            IDapperRepository<UserEntity> dapperRepository)
        {
            _userRepository = userRepository;
            _userDapperRep = dapperRepository;
        }

        public object Find(object key)
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
