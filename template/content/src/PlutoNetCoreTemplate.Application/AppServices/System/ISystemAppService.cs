namespace PlutoNetCoreTemplate.Application.AppServices
{
    using Domain.Aggregates.System;
    using Domain.Services.Account;
    using PlutoData.Collections;
    using PlutoData.Specifications;

    public interface ISystemAppService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPagedList<UserEntity> GetPageList(int pageNo,int pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        UserEntity GetUser(object key);
    }
}