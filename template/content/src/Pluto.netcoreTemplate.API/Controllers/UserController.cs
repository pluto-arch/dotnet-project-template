using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.API.Models;
using Pluto.netcoreTemplate.API.Models.Requests;
using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Application.Queries.Interfaces;
using Pluto.netcoreTemplate.Infrastructure.Providers;

namespace Pluto.netcoreTemplate.API.Controllers
{
    /// <summary>
    /// Demo 控制器
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UserController : ApiBaseController<UserController>
    {

        private readonly IUserQueries _userQueries;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        public UserController(
            IMediator mediator, 
            ILogger<UserController> logger, 
            EventIdProvider eventIdProvider, 
            IUserQueries userQueries) : base(mediator, logger, eventIdProvider)
        {
            _userQueries = userQueries;
        }


        /// <summary>
        /// 获取所有用户  GET: api/users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse Users()
        {
            var users= _userQueries.GetUsers();
            return ApiResponse.Success(users);
        }

        /// <summary>
        /// 根据id获取一个用户 GET: api/users/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ApiResponse Users(int id)
        {
            var users = _userQueries.GetUser(id);
            return ApiResponse.Success(users);
        }

        /// <summary>
        /// 获取用户的所有角色 / GET: api/users/{id}/roles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/roles")]
        public ApiResponse UserRoles(int id)
        {
            return ApiResponse.Success(new { User="user1",Roles=new string[] { "role1","role2" }});
        }



        /// <summary>
        /// 创建用户 POST: api/users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Post([FromBody]CreateUserRequest request)
        {
            var res = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString("N"), request.Password));
            if (res)
            {
                return ApiResponse.Success("创建成功");
            }
            return ApiResponse.DefaultFail("创建失败");
        }

        /// <summary>
        /// 全部更新 PUT: api/users/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public ApiResponse Put(int id, [FromBody]PutUserRequest request)
        {
            return ApiResponse.DefaultFail("更新成功");
        }

        /// <summary>
        /// 删除一个用户 DELETE: api/demo/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var res = await _mediator.Send(new DeleteUserCommand(id));
            if (res)
            {
                return ApiResponse.Success("创建成功");
            }
            return ApiResponse.DefaultFail("创建失败");
        }

    }
}
