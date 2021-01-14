using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Application.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PlutoData.Collections;
using PlutoNetCoreTemplate.Application.Command;

namespace PlutoNetCoreTemplate.Controllers
{
    using Application.Dtos;

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
			return Ok("");
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
            var response = ResponseDto<string>.Success("12312");
            return Ok(response);
		}

		/// <summary>
		/// 创建用户 POST: api/users
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> PostAsync()
		{
			var res = await _mediator.Send(new CreateUserCommand("request.UserName", "request.Password"));
            var response = ResponseDto<string>.Success("12312");
			return Ok(response);
		}


        /// <summary>
        /// 全部更新
        /// </summary>
        [HttpPut("{id}")]
		public IActionResult Put()
		{
			return Ok();
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
			return Ok();
		}
	}
}