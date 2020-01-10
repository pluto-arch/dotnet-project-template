using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;


namespace Pluto.netcoreTemplate.Application.Queries.UserQueries
{
    public class UserQuery
    {
        private readonly IUserRepository _userRepository;
        public UserQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> GetByIdAsync(int id)
        {
            var res = await _userRepository.GetAsync(id);
            return new
            {
                Id=res?.Id,
                UserName=res?.UserName,
                Tel=res?.Tel,
                Status=res?.Status,
                OpenTime=res?.CreateTime
            };
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <returns></returns>
        public async Task<(int total, IEnumerable<object> list)> GetListAsync(int index,int size)
        {
            var res = await _userRepository.GetListAsync(index, size);
            return (res.total, res.list);
        }

    }
}