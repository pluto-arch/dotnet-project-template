using PlutoData.Collections;
using PlutoNetCoreTemplate.Application.Dtos;


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
        IPagedList<UserItemDto> GetUsers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetUser(object key);
    }
}