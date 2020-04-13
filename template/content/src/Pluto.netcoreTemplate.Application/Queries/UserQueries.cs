using System;
using System.Collections.Generic;
using System.Linq;
using Pluto.netcoreTemplate.Application.Queries.Interfaces;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;


//  ===================
// 2020-03-24
//  ===================

namespace Pluto.netcoreTemplate.Application.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class UserQueries: IUserQueries
    {

        private readonly IQueryable<UserEntity> _userEntities;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        public UserQueries()
        {
        }


        /// <inheritdoc />
        public IEnumerable<object> GetUsers()
        {
            return _userEntities.ToList();
        }

        /// <inheritdoc />
        public object GetUser(object key)
        {
           return _userEntities.FirstOrDefault(x => x.Id == (int)key);
        }
    }
}