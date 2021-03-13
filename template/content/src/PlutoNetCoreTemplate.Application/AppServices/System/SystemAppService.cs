namespace PlutoNetCoreTemplate.Application.AppServices
{
    using Domain.Aggregates.System;
    using Domain.Services.Account;
    using PlutoData.Collections;
    using PlutoData.Specifications;

    public class SystemAppService:ISystemAppService
    {

        private readonly ISystemDomainService _accountDomainService;

        public SystemAppService(ISystemDomainService accountDomainService)
        {
            _accountDomainService = accountDomainService;
        }

        /// <inheritdoc />
        public IPagedList<UserEntity> GetPageList(int pageNo, int pageSize)
        {
            var pageList= _accountDomainService.GetUserPageList(new UserSpecification(), 1,20);
            return pageList;
        }

        /// <inheritdoc />
        public UserEntity GetUser(object key)
        {
            return _accountDomainService.Find(key);
        }
    }
}