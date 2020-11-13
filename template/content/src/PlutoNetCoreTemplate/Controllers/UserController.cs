using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Application.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PlutoData.Collections;
using PlutoNetCoreTemplate.Application.CommandBus.Commands;
using PlutoNetCoreTemplate.Application.ResourceModels;
using PlutoNetCoreTemplate.Models;
using PlutoNetCoreTemplate.Models.Requests;

namespace PlutoNetCoreTemplate.Controllers
{
	/// <summary>
	/// Demo 控制器
	/// </summary>
	[Route("api/users")]
	[ApiController]
	public class UserController : BaseController<UserController>
	{
		private readonly IUserQueries _userQueries;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="logger"></param>
		/// <param name="userQueries"></param>
		public UserController(
			IMediator mediator,
			ILogger<UserController> logger,
			IUserQueries userQueries) : base(mediator, logger)
		{
			_userQueries = userQueries;
		}


		/// <summary>
		/// 获取所有用户  GET: api/users
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Users()
		{
			_logger.LogInformation("获取用户");
			var users = _userQueries.GetUsers();
			return Ok(ApiResponse.Success(users));
		}

		/// <summary>
		/// 根据id获取一个用户 GET: api/users/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public IActionResult Users(int id)
		{
			var users = _userQueries.GetUser(id);
			return Ok(ApiResponse.Success(users));
		}

		/// <summary>
		/// 创建用户 POST: api/users
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest request)
		{
			var res = await _mediator.Send(new CreateUserCommand(request.UserName, request.Password));
			return Ok(ApiResponse.Success(res));
		}

		/// <summary>
		/// 全部更新
		/// </summary>
		/// <param name="id"></param>
		/// <param name="request"></param>
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] PutUserRequest request)
		{
			return Ok(ApiResponse.Success("更新成功"));
		}

		/// <summary>
		/// 删除一个用户 DELETE: api/demo/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _mediator.Send(new DeleteUserCommand(id));
			return Ok(ApiResponse.Success(res));
		}
	}
}