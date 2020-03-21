using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.SeedWork;

namespace Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate
{
    public interface IRoleRepository : IRepository<RoleEntity>
    {
        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task CreateAsync(RoleEntity role, CancellationToken cancellationToken);


        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// 根据邮箱获取
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleEntity> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<RoleEntity>> GetUserRolesAsync(UserEntity user, CancellationToken cancellationToken);
    }
}