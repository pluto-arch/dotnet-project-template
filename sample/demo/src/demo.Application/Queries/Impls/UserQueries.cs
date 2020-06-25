using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Application.Queries.Interfaces;
using Demo.Application.ResourceModels;
using Demo.Domain.AggregatesModel.UserAggregate;
using Demo.Infrastructure;
using PlutoData.Collections;
using PlutoData.Interface;


//  ===================
// 2020-03-24
//  ===================

namespace Demo.Application.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class UserQueries: IUserQueries
    {

        private readonly IRepository<UserEntity> _userRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserQueries(IUnitOfWork<DemoDbContext> unitOfWork)
        {
            _userRepository = unitOfWork.GetRepository<IUserRepository>();
        }


        /// <inheritdoc />
        public IPagedList<UserItemModel> GetUsers()
        {
            var pageList= _userRepository.GetPagedList<UserItemModel>(x => new UserItemModel{UserName=x.UserName,Email=x.Email},pageIndex:1,pageSize:20);
            return pageList;
        }

        /// <inheritdoc />
        public object GetUser(object key)
        {
            return _userRepository.Find(key);
        }
    }
}