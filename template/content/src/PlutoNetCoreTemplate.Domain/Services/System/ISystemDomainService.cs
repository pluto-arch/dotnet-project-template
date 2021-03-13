
using PlutoData.Collections;
using PlutoData.Specifications;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    using Aggregates.System;

    /// <summary>
    /// 
    /// </summary>
    public interface ISystemDomainService
    {
        /// <summary>
        /// 查询用户分页列表
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<UserEntity> GetUserPageList(ISpecification<UserEntity> spec,int pageNo,int pageSize);
        UserEntity Find(object key);
    }
}
