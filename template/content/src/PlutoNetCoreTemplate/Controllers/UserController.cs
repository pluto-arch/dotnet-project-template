using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MediatR;
using PlutoNetCoreTemplate.Application.Command;

namespace PlutoNetCoreTemplate.Controllers
{
    using System.IO;
    using Application.AppServices;
    using PlutoNetCoreTemplate.Infrastructure.Commons;

    /// <summary>
    /// Demo 控制器
    /// </summary>
    [Route("api/users")]
	[ApiController]
	public class UserController : BaseController<UserController>
	{
		private readonly ISystemAppService _systemAppService;


		/// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="systemAppService"></param>
		public UserController(
			IMediator mediator,
			ILogger<UserController> logger, ISystemAppService systemAppService) : base(mediator, logger)
        {
            _systemAppService = systemAppService;
        }


		/// <summary>
		/// 获取所有用户  GET: api/users
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ServiceResponse<object> Users()
		{
			var users = _systemAppService.GetPageList(1,20);
			return ServiceResponse<object>.Success(users);
		}

		/// <summary>
		/// 根据id获取一个用户 GET: api/users/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ServiceResponse<object> Users(int id)
		{
			var users = _systemAppService.GetUser(id);
            if (users==null)
            {
                throw new InvalidDataException("无此数据");
            }
            var response = ServiceResponse<object>.Success(users);
            return response;
		}

		/// <summary>
		/// 创建用户 POST: api/users
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ServiceResponse<string>> PostAsync([FromBody]CreateUserCommand request)
		{
			var res = await _mediator.Send(request);
            var response = ServiceResponse<string>.Success(res.ToString());
			return response;
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