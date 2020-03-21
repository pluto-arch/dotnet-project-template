using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.API.Models;
using Pluto.netcoreTemplate.API.Models.Requests;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        public UserController(IMediator mediator, ILogger<UserController> logger, EventIdProvider eventIdProvider) : base(mediator, logger, eventIdProvider)
        {
        }


        /// <summary>
        /// 获取所有用户  GET: api/user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse Users()
        {
            return ApiResponse.Success(new string[] { "user1", "user2", "user3" });
        }

        /// <summary>
        /// 根据id获取一个用户 GET: api/users/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ApiResponse User(int id)
        {
            return ApiResponse.Success("user1");
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
        /// <param name="value"></param>
        [HttpPost]
        public ApiResponse Post([FromBody]CreateUserRequest request)
        {
            return ApiResponse.Success("success");
        }

        // PUT: api/Demo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/demo/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
