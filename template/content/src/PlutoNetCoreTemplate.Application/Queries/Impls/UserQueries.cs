using PlutoData.Collections;
using PlutoData.Interface;
using PlutoData.Uows;
using PlutoNetCoreTemplate.Application.Dtos;
using PlutoNetCoreTemplate.Application.Queries.Interfaces;
using PlutoNetCoreTemplate.Domain.Aggregates.Account;
using PlutoNetCoreTemplate.Domain.IRepositories;
using PlutoNetCoreTemplate.Domain.Services.Account;
using PlutoNetCoreTemplate.Infrastructure;



namespace PlutoNetCoreTemplate.Application.Queries.Impls
{
    /// <summary>
    /// 仅仅查询-默认不启用追踪
    /// 方法指定 disableTracking:true
    /// </summary>
    public class UserQueries: IUserQueries
    {

        private readonly IAccountDomainService _accountDomainService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountDomainService"></param>
        public UserQueries(IAccountDomainService accountDomainService)
        {
            _accountDomainService = accountDomainService;
        }


        /// <inheritdoc />
        public IPagedList<UserEntity> GetUsers()
        {
            var pageList= _accountDomainService.GetUserPageList(new UserSpecification(4), 1,20);
            return pageList;
        }

        /// <inheritdoc />
        public object GetUser(object key)
        {
            return _accountDomainService.Find(key);
        }

    }
}