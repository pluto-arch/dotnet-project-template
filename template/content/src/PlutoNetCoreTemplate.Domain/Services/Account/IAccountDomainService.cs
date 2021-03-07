
using PlutoData.Collections;
using PlutoData.Specifications;

using PlutoNetCoreTemplate.Domain.Aggregates.Account;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    /// <summary>
    /// Account服务
    /// </summary>
    public interface IAccountDomainService
    {
        /// <summary>
        /// 查询用户分页列表
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<UserEntity> GetUserPageList(ISpecification<UserEntity> spec,int pageNo,int pageSize);
        object Find(object key);
    }
}
