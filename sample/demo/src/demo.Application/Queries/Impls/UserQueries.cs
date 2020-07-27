using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Demo.Application.Queries.Interfaces;
using Demo.Application.ResourceModels;
using Demo.Domain.AggregatesModel.UserAggregate;
using Demo.Infrastructure;
using Demo.Infrastructure.Providers;
using Grpc.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserQueries> _logger;
        private readonly EventIdProvider _eventIdProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserQueries(IUnitOfWork<DemoDbContext> unitOfWork, ILogger<UserQueries> logger, EventIdProvider eventIdProvider)
        {
            _logger = logger;
            _eventIdProvider = eventIdProvider;
            _userRepository = unitOfWork.GetRepository<IUserRepository>();
        }


        /// <inheritdoc />
        public IPagedList<UserItemModel> GetUsers()
        {
            _logger.LogInformation(_eventIdProvider.EventId,$"获取所有用户：GetUsers");


            var adadad= _userRepository.GetAll(x => EF.Functions.Like("Email", "%69178145%"));


            var pageList= _userRepository.GetPagedList<UserItemModel>(
                x => new UserItemModel{UserName=x.UserName,Email=x.Email},
                predicate:z=>EF.Functions.Like("Email", $"%61%")
                ,pageIndex:1,pageSize:20);



            _logger.LogInformation(_eventIdProvider.EventId, "获取所有用户结果：{@pageList}", pageList);
            return pageList;
        }

        /// <inheritdoc />
        public object GetUser(object key)
        {
            return _userRepository.Find(key);
        }
    }
}