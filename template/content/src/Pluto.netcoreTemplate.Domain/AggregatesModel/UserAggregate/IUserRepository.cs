using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.SeedWork;

namespace Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<UserEntity>
    {

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateAsync(UserEntity user, CancellationToken cancellationToken);


        /// <summary>
        ///  attach role for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddToRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken);


        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// 根据邮箱获取
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserEntity> GetByEmailAsync(string email, CancellationToken cancellationToken);




        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<UserEntity>> GetUsersInRole(string roleName, CancellationToken cancellationToken);



        /// <summary>
        /// 判断用户是有某个角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken);


        /// <summary>
        /// 更新标记安全戳
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetSecurityStampAsync(UserEntity user, string stamp, CancellationToken cancellationToken);

    }
}