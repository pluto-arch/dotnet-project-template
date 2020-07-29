using System;
using System.Collections.Generic;
using System.Linq;
using PlutoNetCoreTemplate.Application.Queries.Interfaces;
using PlutoNetCoreTemplate.Application.ResourceModels;
using PlutoNetCoreTemplate.Domain.DomainModels.Account;
using PlutoNetCoreTemplate.Domain.IRepositories;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData.Collections;
using PlutoData.Interface;


//  ===================
// 2020-03-24
//  ===================

namespace PlutoNetCoreTemplate.Application.Queries
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
        public UserQueries(IUnitOfWork<EfCoreDbContext> unitOfWork)
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