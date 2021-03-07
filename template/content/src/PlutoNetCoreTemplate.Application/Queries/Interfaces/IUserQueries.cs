using PlutoData.Collections;
using PlutoNetCoreTemplate.Application.Dtos;
using PlutoNetCoreTemplate.Domain.Aggregates.Account;

namespace PlutoNetCoreTemplate.Application.Queries.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPagedList<UserEntity> GetUsers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetUser(object key);
    }
}